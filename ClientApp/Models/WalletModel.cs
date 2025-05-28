using System;
using ClientApp.Shared.Enums.Wallet;

namespace ClientApp.Models;

public class WalletModel
{
    public DateTime CreatedAt { get; init; }
    public WalletType Type { get; set; }
    public decimal Balance { get; set; }
}