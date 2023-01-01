// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.9;

import "./SantaCoin.sol";

contract NiceListV2 {
    address public immutable owner;

    uint128 public buyInAmount;
    uint64 public christmasTimestamp;
    uint8 public unlocked = 1;
    SantaCoin public santaCoin;

    mapping(address => uint256) public buyIns;
    mapping(address => uint256) public niceListV2;

    error TransferFailed(address from, address to, uint256 amount);
    error NotEnoughFunds(uint256 required, uint256 available);
    error WithdrawFailed(string reason);

    event NewSantaCoin(address indexed previousCoin, address indexed newCoin);
    event NewChristmas(uint64 previousTimestamp, uint64 newTimestamp);
    event NewBuyInAmount(uint128 previousAmount, uint128 newAmount);
    event Withdraw(address indexed target, uint256 amount);
    event BuyIn(address indexed who, uint256 amount);
    event NiceListV2Entered(address indexed who);
    event UserWithdraw(address indexed who, uint256 amount, bool inETH);

    constructor(
        uint64 _christmasTimestamp,
        uint128 _buyInAmount,
        SantaCoin _santaCoin
    ) {
        owner = msg.sender;
        christmasTimestamp = _christmasTimestamp;
        buyInAmount = _buyInAmount;
        santaCoin = _santaCoin;
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    modifier nonReentrant() {
        require(unlocked == 1, "Reentrancy");
        unlocked = 2;
        _;
        unlocked = 1;
    }

    // @notice sets a new santa coin address
    // @param address _santaCoin new ERC20 coin
    function setSantaCoin(SantaCoin _santaCoin) external onlyOwner {
        emit NewSantaCoin(address(santaCoin), address(_santaCoin));
        santaCoin = _santaCoin;
    }

    // @notice sets a new christmas timestamp. Only callable by owner
    // @param uint64 _christmasTimestamp New Timestamp
    function setNewChristmas(uint64 _christmasTimestamp) external onlyOwner {
        emit NewChristmas(christmasTimestamp, _christmasTimestamp);
        christmasTimestamp = _christmasTimestamp;
    }

    // @notice sets a new buy-in amount. Only callable by owner
    // @param uint128 _buyInAmount New buy-in amount
    function setNewBuyInAmount(uint128 _buyInAmount) external onlyOwner {
        emit NewBuyInAmount(buyInAmount, _buyInAmount);
        buyInAmount = _buyInAmount;
    }

    // @notice Withdraw santa coins from contract. Only callable by owner
    // @param address target Send coins to
    // @param uint256 amount Amount of coins to send
    function withdrawOwner(address target, uint256 amount) external onlyOwner {
        bool success = santaCoin.transfer(target, amount);
        if (!success) {
            revert TransferFailed(address(this), target, amount);
        }
        emit Withdraw(target, amount);
    }

    // @notice Buys into the NiceList.
    // @param uint256 amount Amount of SantaCoins to use
    // @dev calls internal _buyIn function. Requires previous allowance set
    function buyIn(uint256 amount) external {
        bool success = santaCoin.transferFrom(
            msg.sender,
            address(this),
            amount
        );
        if (!success) {
            revert TransferFailed(msg.sender, address(this), amount);
        }

        _buyIn(msg.sender, amount);
    }

    // @notice handles the buy-in and sets the user on the nice list if enough coins are spent
    // @param address user Which user buys in
    // @param uint256 amount Amount of coins sent
    function _buyIn(address user, uint256 amount) private {
        uint256 userBuyInAmount = buyIns[user] + amount;
        if (
            userBuyInAmount > buyInAmount - 1 &&
            niceListV2[user] != christmasTimestamp
        ) {
            niceListV2[user] = christmasTimestamp;
            userBuyInAmount -= buyInAmount;
            emit NiceListV2Entered(user);
        }
        emit BuyIn(user, amount);
        buyIns[user] = userBuyInAmount;
    }

    // @notice Checks if the user is nice
    // @param address user User to check
    function isNice(address user) external view returns (bool) {
        return niceListV2[user] == christmasTimestamp;
    }

    // @notice Withdraw the remaning buy-in from the contract in SANTA coins
    // @param uint256 amount How many coins should be withdrawn
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
        emit UserWithdraw(msg.sender, amount, false);
    }

    // @notice Withdraw the remaning buy-in from the contract in ETH
    // @param uint256 amount How many ETH should be withdrawn
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

    receive() external payable {}
}