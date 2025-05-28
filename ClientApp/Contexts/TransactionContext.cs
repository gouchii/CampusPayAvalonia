using System;
using ClientApp.Shared.DTOs.TransactionDto;

namespace ClientApp.Contexts;

public class TransactionContext
{
    public TransactionMode Mode { get; set; }
    public TransactionDto TransactionDto { get; set; } = new();
    public decimal CurrentBalance { get; set; }


    public BasePaymentRequestDto? ToDto()
    {
        if (TransactionDto.VerificationToken == null) return null;
            return Mode switch
            {
                TransactionMode.SendQr => new QrPaymentRequestDto { TransactionRef = TransactionDto.TransactionRef, Token = TransactionDto.VerificationToken },
                // TransactionMode.ReceiveRfid => new TransactionDto { Type = "receive", Method = "rfid", Amount = Amount, Rfid = RfidTag },
                //
                // TransactionMode.SendRfid => new TransactionDto { Type = "send", Method = "rfid", Amount = Amount, Rfid = RfidTag },
                // TransactionMode.SendQr => new TransactionDto { Type = "send", Method = "qr", Amount = Amount, QrCode = QrCode },
                // TransactionMode.SendUsername => new TransactionDto { Type = "send", Method = "username", Amount = Amount, Username = Username },
                _ => throw new InvalidOperationException("Invalid transaction mode")
            };
    }
    public void Clear()
    {
        TransactionDto = new TransactionDto();
        CurrentBalance = 0;
        Mode = default;
    }

}
