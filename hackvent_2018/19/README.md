# Day 19: PromoCode

I was delighted to see a WebAssembly challenge. I planned to work on one myself. I wasn't able to solve the hard one, my
bruteforce algorithm took too long to solve it. In this writeup I will discuss the easier version.

First of all we use the [wasm2wat](https://webassembly.github.io/wabt/demo/wasm2wat/) of the [WebAssembly Binary
Toolkit](https://github.com/WebAssembly/wabt) to get the text format of the webassembly. The following part is important
for us:

```wat
(func $_checkPromoCode (export "_checkPromoCode") (type $t1) (param $p0 i32) (result i32)
    (local $l0 i32) (local $l1 i32) (local $l2 i32) (local $l3 i32) (local $l4 i32) (local $l5 i32) (local $l6 i32) (local $l7 i32) (local $l8 i32) (local $l9 i32) (local $l10 i32) (local $l11 i32) (local $l12 i32) (local $l13 i32) (local $l14 i32) (local $l15 i32) (local $l16 i32) (local $l17 i32) (local $l18 i32) (local $l19 i32) (local $l20 i32) (local $l21 i32) (local $l22 i32) (local $l23 i32) (local $l24 i32) (local $l25 i32) (local $l26 i32) (local $l27 i32) (local $l28 i32) (local $l29 i32) (local $l30 i32) (local $l31 i32) (local $l32 i32) (local $l33 i32) (local $l34 i32) (local $l35 i32) (local $l36 i32) (local $l37 i32) (local $l38 i32) (local $l39 i32) (local $l40 i32) (local $l41 i32) (local $l42 i32) (local $l43 i32) (local $l44 i32) (local $l45 i32) (local $l46 i32) (local $l47 i32) (local $l48 i32)
    (set_local $l46
      (get_local $l11))
    (set_local $l1
      (call $f32
        (get_local $l46)))
    (set_local $l2
      (i32.eq
        (get_local $l1)
        (i32.const 15)))
    (block $B1
      (if $I2
        (get_local $l2)
        (then
          (set_local $l43
            (i32.const 0))
          (loop $L3
            (block $B4
              (set_local $l3
                (get_local $l43))
              (set_local $l4
                (get_local $l11))
              (set_local $l5
                (call $f32
                  (get_local $l4)))
              (set_local $l6
                (i32.lt_u
                  (get_local $l3)
                  (get_local $l5)))
              (if $I5
                (i32.eqz
                  (get_local $l6))
                (then
                  (br $B4)))
              (set_local $l7
                (get_local $l11))
              (set_local $l8
                (get_local $l43))
              (set_local $l9
                (i32.add
                  (get_local $l7)
                  (get_local $l8)))
              (set_local $l10
                (i32.load8_s
                  (get_local $l9)))
              (set_local $l12
                (i32.shr_s
                  (i32.shl
                    (get_local $l10)
                    (i32.const 24))
                  (i32.const 24)))
              (set_local $l13
                (i32.xor
                  (get_local $l12)
                  (i32.const 90)))
              (set_local $l44
                (get_local $l13))
              (set_local $l14
                (get_local $l44))
              (set_local $l15
                (get_local $l43))
              (set_local $l16
                (i32.add
                  (get_local $l33)
                  (i32.shl
                    (get_local $l15)
                    (i32.const 2))))
              (set_local $l17
                (i32.load
                  (get_local $l16)))
              (set_local $l18
                (i32.ne
                  (get_local $l14)
                  (get_local $l17)))
              (if $I6
                (get_local $l18)
                (then
                  (set_local $l47
                    (i32.const 5))
                  (br $B4)))
              (set_local $l19
                (get_local $l43))
              (set_local $l20
                (i32.add
                  (get_local $l19)
                  (i32.const 1)))
              (set_local $l43
                (get_local $l20))
              (br $L3)))
          (if $I7
            (i32.eq
              (get_local $l47)
              (i32.const 5))
            (then
              (set_local $l0
                (get_local $l42))
              (set_local $l41
                (get_local $l0))
              (set_global $g10
                (get_local $l48))
              (return
                (get_local $l41))))
          (set_local $l45
            (i32.const 0))
          (loop $L8
            (set_local $l21
              (get_local $l45))
            (set_local $l23
              (i32.lt_s
                (get_local $l21)
                (i32.const 30)))
            (if $I9
              (i32.eqz
                (get_local $l23))
              (then
                (br $B1)))
            (set_local $l24
              (get_local $l45))
            (set_local $l25
              (i32.add
                (get_local $l22)
                (get_local $l24)))
            (set_local $l26
              (i32.load8_s
                (get_local $l25)))
            (set_local $l27
              (i32.shr_s
                (i32.shl
                  (get_local $l26)
                  (i32.const 24))
                (i32.const 24)))
            (set_local $l28
              (get_local $l11))
            (set_local $l29
              (get_local $l45))
            (set_local $l30
              (i32.and
                (i32.rem_s
                  (get_local $l29)
                  (i32.const 15))
                (i32.const -1)))
            (set_local $l31
              (i32.add
                (get_local $l28)
                (get_local $l30)))
            (set_local $l32
              (i32.load8_s
                (get_local $l31)))
            (set_local $l34
              (i32.shr_s
                (i32.shl
                  (get_local $l32)
                  (i32.const 24))
                (i32.const 24)))
            (set_local $l35
              (i32.xor
                (get_local $l27)
                (get_local $l34)))
            (set_local $l36
              (i32.and
                (get_local $l35)
                (i32.const 255)))
            (set_local $l37
              (get_local $l45))
            (set_local $l38
              (i32.add
                (get_local $l42)
                (get_local $l37)))
            (i32.store8
              (get_local $l38)
              (get_local $l36))
            (set_local $l39
              (get_local $l45))
            (set_local $l40
              (i32.add
                (get_local $l39)
                (i32.const 1)))
            (set_local $l45
              (get_local $l40))
            (br $L8)
            (unreachable))
          (unreachable))))
      )
```

At first the length of the input gets compared. It has to be 15 otherwise `HV18-TRYH-ARDE-RTRY_HARD_ER!!` gets printed.
Then each character of the promo code gets xored with 90 and compared with a value from the linear memory. The code can
easily be reversed by hand:

```javascript
const data = "\\1feS\\0c\\18\\1fz!\\04A:!\\06rY=IVv\\18<C:+A6\\00\\0d\\5ct\\00\\00\\0d\\00\\00\\00i\\00\\00\\008\\00\\00\\00n\\00\\00\\00o\\00\\00\\007\\00\\00\\00k\\00\\00\\00)\\00\\00\\00\\08\\00\\00\\006\\00\\00\\00#\\00\\00\\00\\1c\\00\\00\\00n\\00\\00\\00o\\00\\00\\00.\\00\\00\\00\\00\\00\\00\\00HV18-TRYH-ARDE-RTRY_HARD_ER!!\\00\\00\\00\\05\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\01\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\02\\00\\00\\00\\03\\00\\00\\00\\08\\05\\00\\00\\00\\04\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\01\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\0a\\ff\\ff\\ff\\ff\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\00\\80\\04";
const decodedData = [];

var matchingHex = false;
var part = '';

for (var i = 0; i < data.length; i++) {
    var current = data.charAt(i);

    if (current == '\\') {
        matchingHex = true;
    } else {
        if (matchingHex) {
            if (part.length == 0) {
                part += current;
            } else {
                decodedData.push(parseInt(part + current, 16));

                matchingHex = false;
                part = '';
            }
        } else {
            decodedData.push(current.charCodeAt(0));
        }
    }
}

const key = [];
for (var j = 0; j < 15; j++) {
    key.push(String.fromCharCode(decodedData[8 + j << 2] ^ 90));
}

console.log(key.join(''));
```

The first part just decodes the data segment that we found in the text format of the WebAssembly. The second part
generates the key.
