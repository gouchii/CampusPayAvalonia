using System.ComponentModel.DataAnnotations;

namespace ClientApp.Shared.DTOs.QR;

public class QrCodeDataDto
{
    [Required] public string TransactionRef { get; set; } = string.Empty;
}