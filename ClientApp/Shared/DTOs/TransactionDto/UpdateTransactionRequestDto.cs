using ClientApp.Shared.Enums.Transaction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClientApp.Shared.DTOs.TransactionDto;

public class UpdateTransactionRequestDto
{
    public decimal? Amount { get; init; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TransactionType? Type { get; init; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMethod? Method { get; init; }
}