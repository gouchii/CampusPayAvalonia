using System;
using System.Collections.Generic;

using ClientApp.Shared.Enums.Transaction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClientApp.Shared.DTOs.TransactionDto;

public class TransactionDto
{
    public string? SenderName { get; set; } = string.Empty;

    public string? ReceiverName { get; set; } = string.Empty;

    [JsonConverter(typeof(StringEnumConverter))]
    public TransactionType Type { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMethod Method { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TransactionStatus Status { get; set; }

    public string? VerificationToken { get; set; }
    public DateTime? TokenGeneratedAt { get; set; }

    public string TransactionRef { get; set; } = string.Empty;

    public List<TransactionRelationDto> ParentRelations { get; set; } = new();
    public List<TransactionRelationDto> ChildRelations { get; set; } = new();
}