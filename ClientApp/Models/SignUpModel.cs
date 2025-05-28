using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Data.Core.Plugins;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.Models;

public partial class SignUpModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Username is required")]
    private string _userName = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Full name is required")]
    private string _fullName = string.Empty;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress]
    private string _email = string.Empty;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Password is required")]
    private string _password = string.Empty;





    public void Validate()
    {
        ValidateAllProperties();
    }

}