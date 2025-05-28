using System.ComponentModel.DataAnnotations;

namespace ClientApp.Shared.DTOs.UserCredential.Register;

public class RegisterCredentialRequestDto
{
    [Required] public string Value { get; set; } = string.Empty;
}