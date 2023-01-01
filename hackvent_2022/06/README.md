# 06 - privacy isn't given

## Description

Level: Easy<br/>
Author: HaCk0

As every good IT person, Santa doesn't have all his backups at one place. Instead, he spread them all over the world.
With this new blockchain unstoppable technology emerging (except Solana, this chain stops all the time) he tries to use
it as another backup space. To test the feasibility, he only uploaded one single flag. Fortunately for you, he doesn't
understand how blockchains work.

## Solution

For this challenge we are given the source code of a smart contract:

```
pragma solidity ^0.8.9;

contract NotSoPrivate {
    address private owner;
    string private flag;

    constructor(string memory _flag) {
        flag = _flag;
        owner = msg.sender;
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    function setFlag(string calldata _flag) external onlyOwner {
        flag = _flag;
    }
}
```

The goal is clear, we need to read out the content of the flag variable which is set to be `private`. 
Reading out arbitrary memory can be achieved easily through the web3 API:

```js
const Web3 = require('web3');
const web3 = new Web3("ws://43ac04d9-d135-4836-8c42-5fe471c8f60b.rdocker.vuln.land:8545");
web3.eth.getStorageAt("e78A0F7E598Cc8b0Bb87894B0F60dD2a88d6a8Ab",1).then(r => console.log(web3.utils.toAscii(r)))
```

This prints the flag `HV22{Ch41nS_ar3_Publ1C}`.
