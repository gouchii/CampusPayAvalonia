using System;
using ClientApp.Shared.Enums.Wallet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClientApp.Shared.DTOs.Wallet;

public class WalletDto
{
    public DateTime CreatedAt { get; init; }

    [JsonConverter(typeof(StringEnumConverter))]
    public WalletType Type { get; set; }

    public decimal Balance { get; set; }
}