using System;
using ClientApp.Models;
using ClientApp.Shared.DTOs.Wallet;

namespace ClientApp.Mappers;

public static class WalletMapper
{
    public static WalletModel ToWalletModel(this WalletDto walletDto)
    {
        return new WalletModel
        {
            CreatedAt = walletDto.CreatedAt,
            Type = walletDto.Type,
            Balance = walletDto.Balance
        };
    }
}