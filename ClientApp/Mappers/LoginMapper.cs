using ClientApp.Models;
using ClientApp.Shared.DTOs.Authentication;

namespace ClientApp.Mappers;

public static class LogInMappers
{
    public static LoginRequestDto ToLoginRequestDto(this LoginModel loginModel)
    {
            return new LoginRequestDto
            {
                Username = loginModel.Username,
                Password = loginModel.Password
            };
    }
}