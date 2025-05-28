using ClientApp.Models;
using ClientApp.Shared.DTOs.Authentication;

namespace ClientApp.Mappers;

public static class SignUpMapper
{

    public static SignUpRequestDto ToSignUpRequestDto(this SignUpModel signUpModel)
    {
        return new SignUpRequestDto
        {
            UserName = signUpModel.UserName,
            FullName = signUpModel.FullName,
            Email = signUpModel.Email,
            Password = signUpModel.Password
        };
    }
}