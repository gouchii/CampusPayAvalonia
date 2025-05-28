using System;
using ClientApp.Shared.Enums.Transaction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.Models;

public class TransactionModel : ObservableObject
{
    public string? SenderName { get; set; } = string.Empty;

    public string? ReceiverName { get; set; } = string.Empty;


    public TransactionType Type { get; set; }


    public PaymentMethod Method { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }


    public TransactionStatus Status { get; set; }

    public string? VerificationToken { get; set; }
    public DateTime? TokenGeneratedAt { get; set; }

    public string TransactionRef { get; set; } = string.Empty;

    public string? DisplayTitle { get; set; }
    public string? DisplayAmount { get; set; }

    public decimal CurrentBalance { get; set; }
}