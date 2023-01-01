// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.9;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract SantaCoin is ERC20 {
    event CoinsBought(address who, uint256 amount);
    event CoinsSold(address who, uint256 amount);

    constructor() ERC20("SantaCoin", "SANTA") {}

    function buyCoins() external payable {
        _mint(msg.sender, msg.value);
        emit CoinsBought(msg.sender, msg.value);
    }

    function sellCoins(uint256 amount) external {
        _burn(msg.sender, amount);
        (bool success, ) = payable(msg.sender).call{value: amount}("");
        require(success, "Failed to transfer");
        emit CoinsSold(msg.sender, amount);
    }
}
