using ClientApp.Models;
using ClientApp.Shared.DTOs.TransactionDto;

namespace ClientApp.Mappers;

public static class TransactionMapper
{
    public static TransactionModel ToTransactionModel(this TransactionDto transactionDto)
    {
        return new TransactionModel
        {
            SenderName = transactionDto.SenderName,
            ReceiverName = transactionDto.ReceiverName,
            Type = transactionDto.Type,
            Method = transactionDto.Method,
            Amount = transactionDto.Amount,
            CreatedAt = transactionDto.CreatedAt,
            Status = transactionDto.Status,
            VerificationToken = transactionDto.VerificationToken,
            TokenGeneratedAt = transactionDto.TokenGeneratedAt,
            TransactionRef = transactionDto.TransactionRef
        };
    }
}