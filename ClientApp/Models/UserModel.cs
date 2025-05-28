using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.Models;

public class UserModel : ObservableObject
{
    public string FullName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}