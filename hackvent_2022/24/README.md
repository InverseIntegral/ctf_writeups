# 24 - It's about time for some RSA

## Description

Level: Leet^2<br/>
Author: LogicalOverflow

Santa is giving autographs! And at the end of the signing session he'll also give out the flag! But better hurry; as
Santa has lot's to do this time of year, he can only spent so much time to giving out autographs.

PS: Thanks to the latest in cloning technology, there are six Santas, so up to six signing session can take place at the
same time!

## Solution

This challenge was extremly difficult, probably the hardest HACKvent challenge I have seen so far. I got first blood
on this challenge but it definitely took a while (around 20h). This challenge consisted of two parts, the first one was
reversing the WebAssembly code and then cracking the RSA cryptosystem using a side-channel vulnerability.

### Reversing

Initially, we were given two sources for this challenge. A rust file as well as a WebAssembly file. The important part
of the Rust code is the following:

```rust
fn write_energy(stream: &mut TcpStream, ctx: &mut impl AsStoreMut, instance: &Instance) -> Result<(), std::io::Error> {
    match metering::get_remaining_points(ctx, &instance) {
        MeteringPoints::Remaining(p) => writeln!(stream, "Santa has {} energy remaining. Don't over-tax santa", p),
        MeteringPoints::Exhausted => { writeln!(stream, "Santa died due to exhaustion. What have you done?!") }
    }
}

fn cost_function(op: &Operator) -> u64 {
    use Operator::*;
    match op {
        // maths is hard and santa finds substractions exhausting
        I64Sub => 0x20, _ => 1
    }
}

fn handle_conn(mut stream: TcpStream, wasm_bytes: &Vec<u8>) -> Result<(), Box<dyn Error>> {
    writeln!(stream, "Welcome to Santas Signing Session")?;
    writeln!(stream, "")?;

    let meter = Arc::new(Metering::new(u64::MAX, cost_function));

    let mut tcp_in = stream.try_clone()?;
    let mut tcp_in = BufReader::new(&mut tcp_in)
        .lines()
        .map(|result| result.unwrap());

    let mut compiler_config = Cranelift::default();
    compiler_config.push_middleware(meter);
    let mut store = Store::new(compiler_config);
    let module = Module::new(&store, wasm_bytes)?;

    let mut input = WasiBidirectionalSharedPipePair::new().with_blocking(false);
    let output = WasiBidirectionalSharedPipePair::new().with_blocking(false);
    let wasi_env = WasiState::new("wasm-server")
        .stdin(Box::new(input.clone()))
        .stdout(Box::new(output.clone()))
        .finalize(&mut store)?;
    let mut out_lines = BufReader::new(output)
        .lines()
        .map(|l| l.unwrap().trim().to_string());

    let imports = wasi_env.import_object(&mut store, &module)?;

    let instance = Instance::new(&mut store, &module, &imports)?;

    let memory = instance.exports.get_memory("memory")?;
    wasi_env.data_mut(&mut store).set_memory(memory.clone());

    let start = instance.exports.get_function("_start")?;
    let encrypt = instance.exports.get_function("encrypt")?;
    let sign = instance.exports.get_function("sign")?;

    start.call(&mut store, &[])?;

    writeln!(stream, "{}", out_lines.next().ok_or("")?)?;
    writeln!(stream, "{}", out_lines.next().ok_or("")?)?;
    writeln!(stream, "{}", out_lines.next().ok_or("")?)?;
    // santa doesn't have all day
    metering::set_remaining_points(&mut store, &instance, 75_000_000_000);
    write_energy(&mut stream, &mut store, &instance)?;

    writeln!(stream, "")?;

    writeln!(stream, "Do you want an autograph before you pick up your flag? [yN]")?;

    loop {
        {
            let s = tcp_in.next().ok_or("")?;
            if !s.starts_with("y") && !s.starts_with("Y") {
                break;
            }
        }

        writeln!(stream, "What's your name for the autograph?")?;

        let name = tcp_in.next().ok_or("")?;
        let name = name.trim().as_bytes();
        let name = match hex::decode(name) {
            Ok(v) => v,
            Err(_) => name.to_vec(),
        };
        writeln!(input, "{}", hex::encode(name))?;
        sign.call(&mut store, &[])?;
        writeln!(stream, "Here is your autograph: {}", out_lines.next().ok_or("")?)?;
        write_energy(&mut stream, &mut store, &instance)?;
        if match metering::get_remaining_points(&mut store, &instance) {
            // should be sufficent for one more signature
            MeteringPoints::Remaining(p) => p < 11_000_000,
            MeteringPoints::Exhausted => return Err("Santa died".into()),
        } {
            writeln!(stream, "Santa is running out of energy, so no more autographs")?;
            break;
        }

        writeln!(stream, "Do you want another autograph? [yN]")?;
    }

    // if you get here, santa will make sure to also give you your flag
    metering::set_remaining_points(&mut store, &instance, u64::MAX);

    writeln!(input, "{}", "HVXX{NOT_THE_FLAG}")?;
    encrypt.call(&mut store, &[])?;

    writeln!(stream, "And here is your flag: {}", out_lines.next().ok_or("")?)?;
    writeln!(stream, "Have a nice day!")?;

    stream.shutdown(std::net::Shutdown::Both)?;
    Ok(())
}
```

The rust code access three functions of the WebAssembly: `sign`, `encrypt` and `start`. `start` generates a new RSA
key-pair for each connection and we are given the 160 MSBs of `p` as well as `n` which is 768 bits long. After that we
can sign messages until we run out of energy. The energy is measured in WASM instructions where subtractions decrease
the energy by 32. Finally, we are given the RSA encrypted flag.

Based on these informations I immediately guessed that we have to apply the [Coppershmith
method](https://en.wikipedia.org/wiki/Coppersmith_method) to factor `n`. Unfortunately, 160 bits are not enough to apply
the method yet. At this point I started reversing the WebAssembly code for a while. During the second day we were given
the WASM code though. So I will directly refer to the code given to us:

```rust
fn input() -> String {
    let mut v = String::new();
    io::stdin().read_line(&mut v).unwrap();
    match v.strip_suffix("\n") {
        Some(s) => s.to_string(),
        None => v,
    }
}

const KEYSIZE: usize = 768;
const HB_COUNT: usize = (KEYSIZE / 64) / 4 * 3;

static mut PRIV_KEY: Option<RsaPrivateKey> = None;
static mut PUB_KEY: Option<RsaPublicKey> = None;

#[no_mangle]
pub extern "C" fn encrypt() {
    let mut rng = rand::thread_rng();
    let pub_key = unsafe { PUB_KEY.as_mut().unwrap() };

    let in_str = input();
    let bytes = in_str.as_bytes();
    let enc_data = pub_key
        .encrypt(&mut rng, PaddingScheme::new_pkcs1v15_encrypt(), &bytes[..])
        .expect("failed to encrypt");

    println!("{}", hex::encode(enc_data));
}

fn calc_block(us: &[u32]) -> u64 {
    let mut s1 = 0u64;
    let mut s2 = 0u64;
    for v in us {
        let v = *v as u64;
        s1 = (s1 + v) & 0xffff_ffff;
        s2 = (s2 + s1) & 0xffff_ffff;
    }
    return (s2 << 32) | s1;
}

fn hash(msg: Vec<u8>) -> Vec<u8> {
    let mut segs = Vec::with_capacity(HB_COUNT);
    for _ in 0..HB_COUNT {
        segs.push(Vec::new());
    }
    for (i, b) in msg.iter().enumerate() {
        segs.get_mut(i % HB_COUNT).unwrap().push(*b);
    }
    let mut digest = Vec::with_capacity(HB_COUNT * 8);
    for mut seg in segs {
        let m = seg.len() % 4;
        if m != 0 {
            seg.extend(vec![0u8; 4 - m]);
        }
        let seg = seg
            .chunks(4)
            .map(|d| u32::from_le_bytes(d.try_into().unwrap()))
            .collect::<Vec<_>>();
        digest.extend(calc_block(&seg).to_le_bytes());
    }
    digest
}

#[no_mangle]
pub extern "C" fn sign() {
    let priv_key = unsafe { PRIV_KEY.as_mut().unwrap() };

    let bytes = hex::decode(input()).expect("failed to decode");
    let sig: Result<_, Box<dyn Error>> = (|| {
        let hashed = hash(bytes);

        let ciphertext = hashed;
        let pad_size = priv_key.size();

        let c = BigUint::from_bytes_be(&ciphertext);
        let m = internals::decrypt::<ThreadRng>(None, priv_key, &c)?;
        let m_bytes = m.to_bytes_be();
        let plaintext = internals::left_pad(&m_bytes, pad_size);

        Ok(plaintext)
    })();
    match sig {
        Ok(enc_data) => println!("{}", hex::encode(enc_data)),
        Err(err) => println!("err: {}", err),
    }
}

fn main() {
    let mut rng = rand::thread_rng();
    let mut priv_key = RsaPrivateKey::new(&mut rng, KEYSIZE).expect("failed to generate a key");
    let pub_key = RsaPublicKey::from(&priv_key);

    if let Err(e) = priv_key.precompute() {
        panic!("failed to precompute for crt: {}", e);
    }

    println!("modulus: n = 0x{}", priv_key.n().to_str_radix(16));
    println!("exponent: e = 0x{}", priv_key.e().to_str_radix(16));
    println!(
        "prime: p = 0x{}...",
        &priv_key.primes()[0].clone().to_str_radix(16)[..40]
    );

    unsafe {
        PRIV_KEY = Some(priv_key);
        PUB_KEY = Some(pub_key);
    }
}
```

The code is actually quite simple. The signatures are generated using the optimized
[RSA-CRT](https://en.wikipedia.org/wiki/RSA_(cryptosystem)#Using_the_Chinese_remainder_algorithm). If we inspect the
source code of the libraries used, we can also see that [it uses montgomery
multiplications](https://github.com/rust-num/num-bigint/blob/master/src/biguint/power.rs#L140) to make the calculations
faster. This will be important later on because the exploit that we use here only works for this scenario.

Our input is first hashed using a custom hash function and then signed. Checking some input-output pairs of the hash
function reveals that it's not really a hash function but rather an encoding which can be reversed. This will also be
important later on for our exploit.

### Finding the right paper

After googling for quite a while I came across [this paper by Werner
Schindler](https://link.springer.com/content/pdf/10.1007/3-540-44499-8_8.pdf) that describes the exact scenario of our
challenge. A short checklist:

- We have an RSA cryptosystem
- We use RSA-CRT for our decryption (signing)
- The decryption (signing) uses mongomery's algorithm
- We can measure the time (or in our case energy) of the operations
- **We control the input of the decryption**
  - Since our input is first passed through a "hash" function, this is not immediately given.
    But since the "hash" function is reversible, we can actually control the input.

Perfect, since we meet all the requirements, we can now find an implementation of the above paper and we are done.
Right? Well, there's no implementation to be found and we have to do this on our own. This is, however, not trivial
since there's quite a few pitfalls.

The basic idea of the paper is that we can use the extra time required by the reduction performed during the mongomery
multiplication to leak bits of the RSA prime `p`. The algorithm described in the paper starts by finding an interval
`[l, r]` that contains (a multiple of) the prime `p`. It then proceeds by diving that interval into two parts and
determining which new interval contains (the multiple of) `p`. This step essentially halves the search space. The
algorithm presented in the paper does this until all of `p` is known but we can also stop earlier and apply coppersmith
to find `p`.

### Undoing the hash function

To be able to apply the attack of the paper it is essential that we can control the input passed to the decryption
(signing). Therefore, we have to reverse the "hash" function first. If we then pass `hash'(msg)` to the signing service,
the message that is actually signed will be `hash(hash'(msg)) = msg` which we control. 

The relevant code for the hash function is the following:

```rust
const keysize: usize = 768;
const hb_count: usize = (keysize / 64) / 4 * 3;

fn calc_block(us: &[u32]) -> u64 {
    let mut s1 = 0u64;
    let mut s2 = 0u64;
    for v in us {
        let v = *v as u64;
        s1 = (s1 + v) & 0xffff_ffff;
        s2 = (s2 + s1) & 0xffff_ffff;
    }
    return (s2 << 32) | s1;
}

fn hash(msg: vec<u8>) -> vec<u8> {
    let mut segs = vec::with_capacity(hb_count);
    for _ in 0..hb_count {
        segs.push(vec::new());
    }
    for (i, b) in msg.iter().enumerate() {
        segs.get_mut(i % hb_count).unwrap().push(*b);
    }
    let mut digest = vec::with_capacity(hb_count * 8);
    for mut seg in segs {
        let m = seg.len() % 4;
        if m != 0 {
            seg.extend(vec![0u8; 4 - m]);
        }
        let seg = seg
            .chunks(4)
            .map(|d| u32::from_le_bytes(d.try_into().unwrap()))
            .collect::<vec<_>>();
        digest.extend(calc_block(&seg).to_le_bytes());
    }
    digest
}
```

The `hash` function operates by taking an arbitrary length byte array and returning a 72 byte long array. It does thit
by first splitting the input into 9 segments where the input element at index `i` is added to the segment `i % 9`. The
segments that do not contain a multiple of 4 elements are padded with zeros. We can neglect this fact for now because it
won't matter for our purposes. Each segment is then again split into blocks of 4 bytes which are converted into `32u`s
and finally those are passed into `calc_block`. The final digest that is returned is simply the concatenation of the
result of `calc_block`.

`calc_block` is the interesting part of the hash function. It takes an array of `u32`s and performs some artihmetic and
finally returns a `u64`. This part of the hash function is only reversible if the length of `us` is smaller or equal to
2. To show why it must be smaller, we can assume that `us` has three elements. In that case, the resuling `s1` and `s2`
would look like this:   

```rust
s1 = us[0] + us[1] + us[2];
s2 = 3 * us[0] + 2 * us[1] + us[2];
```

In this case we have three unknowns but only two equations which is not solvable of course. Therefore, let's assume that
the length of `us` is always two. In that case the function simply becomes:

```rust
fn calc_block(us: &[u32]) -> u64 {
    let s1 = (us[0] + us[1]) & 0xffff_ffff;
    let s2 = (2 * us[0] + us[1]) & 0xffff_ffff;
    return (s2 << 32) | s1;
}
```

This can be reversed easily:

```python
# revert to us
s2 = block[:8]
s1 = block[8:]

u0 = s2 - s1
u1 = s2 - 2 * u0

u0 = u0 & 0xffffffff
u1 = u1 & 0xffffffff
```

With this in place, we are ready to tackle the paper.

### Parameters

The paper uses several parameters which are needed for the algorithm.

`R` is the modulus used in the Montgomery's algorithm. In our case this is `2**384` as can be calculated [based on the
source code](https://github.com/rust-num/num-bigint/blob/master/src/biguint/monty.rs#L153).

`Beta` is calculated as `sqrt(n/R**2)` since `R = 2**384` and `n` is roughly `2**768` we get `Beta = sqrt(2**768/2**768)
= 1`. This will make some computations easier later on.

`c_ER` is the timing difference that it takes to perform one extra reduction during the mongomery multiplication.
This constant can be deterimed by counting the instructions in the WASM code (and counting subtractions as 32
instructions) or by simply taking a lot of measurements and taking the average difference. I chose to do the former and
obtained around a value of 400 for `c_ER`.

`T(u)` denotes the time (or rather the energy) needed to compute the signature for input `u`.

`b` is the size of the sliding window used in the mongomery modular exponentiation. This special case is described in
chapter 7 of the paper and needs to be handled differently. In our case `b = 4` [as can be seen in the source
code](https://github.com/rust-num/num-bigint/blob/a7303960225f61a91e7ed21a416e1a93fa474fd4/src/biguint/monty.rs#L134).

### Implementation

The implementation closely follows the phase 2 of the basic scheme presented in Chapter 4 of [the
paper](https://link.springer.com/content/pdf/10.1007/3-540-44499-8_8.pdf):

```python
while True:
    mid = (right + left) // 2

    if is_in(left, mid):
        # continue in [left, mid]
    elif is_in(mid, right):
        # continue in [mid, right]
    else:
       # Backtrack because our previous decision was incorret
```

The function `is_in` checks if the interval contains a multiple of `p`. To get started, however, we do need an initial
interval for our multiple of `p`. Fortunately, we are given the high bits of `p`. With those we already have an interval
that we can use:

```python
left = p << 224
right = (p + 1) << 224
```

Now, only the `is_in` function is left:

```python  
def is_in(left, right):
    N = 7
    case_A = 0
    counter = 0

    while (True):
        diff = T(right) - T(left)

        if diff < -0.25 * c_ER * (768 / (b * 2 ** (b + 1)) + 2**b - 3):
            counter = counter - 1
        else:
            counter = counter + 1

        if (counter  >= N):
            return False
        elif (counter <= -1 * N):
            return True
        left += 1
        right += 1
```

This function takes multiple measurement and uses the formula described in chapter 7 (which also considers the sliding
window case). If enough measurements for one case are taken, we take a decision and otherwise we take more measurements.
With this all the pieces are implemented and we can perform the coppersmith attack to obtain `p`: 

```python
def coppersmith(p_high):
    P.<x> = PolynomialRing(Zmod(n))
    f = p_high + x

    sol = f.small_roots(beta=0.5)

    if sol != []:
        p = p_high + sol[0]
        q = int(n) // int(p)
        
        assert(p.is_prime())
        assert(ZZ(q).is_prime())
        assert(p * q == n)
        
        e = 0x10001
        d = pow(e, -1, (p - 1) * (q - 1))
        m = pow(ct, d, n)
        print(long_to_bytes(m))
```

### Adjustments

To get the attack to work I had to introduce some adjustments:
- I had to cache time measurements (the return values of `T(n)`)
- I had to remember the offsets that were already measured in `is_in` to avoid duplicate measurements during the backtracking phase
- I used `[2 * left, 2 * right]` for the initial interval because the algorithm does not work if the ratio `R / gcd(R,
  left % p)` is small, which it is in our case.

The complete solution can be found in [solve.sage](solve.sage). I ran the exploit 6 times in parallel since it
occasionally failed. After around 10 minutes I got the flag: `HV22{S4n74s_t1m3_i5_up0n_u5!}`.

