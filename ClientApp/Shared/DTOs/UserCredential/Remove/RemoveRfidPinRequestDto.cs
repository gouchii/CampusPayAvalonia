using System.ComponentModel.DataAnnotations;

namespace ClientApp.Shared.DTOs.UserCredential.Remove;

public class RemoveRfidPinRequestDto : BaseRemoveCredentialRequestDto
{
    [Required] public string OldValue { get; set; } = string.Empty;
}