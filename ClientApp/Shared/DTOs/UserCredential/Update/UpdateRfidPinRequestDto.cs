using System.ComponentModel.DataAnnotations;

namespace ClientApp.Shared.DTOs.UserCredential.Update;

public class UpdateRfidPinRequestDto : BaseUpdateCredentialRequestDto
{
    [Required] public string OldValue { get; set; } = string.Empty;
}