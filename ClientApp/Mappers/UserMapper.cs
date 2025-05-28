using ClientApp.Models;
using ClientApp.Shared.DTOs.UserDto;

namespace ClientApp.Mappers;

public static class UserMapper
{
    public static UserModel ToUserModel(this UserDto userDto)
    {
        return new UserModel
        {
            FullName = userDto.FullName,
            UserName = userDto.UserName,
            Email = userDto.Email,
            Role = userDto.Role,
            CreatedAt = userDto.CreatedAt
        };
    }
}