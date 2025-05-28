using System;
using System.ComponentModel.DataAnnotations;
using ClientApp.Attributes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.Models;

public partial class AmountModel : ObservableValidator
{
    [ObservableProperty] [NotifyDataErrorInfo] [Required(ErrorMessage = "Amount is required")]
    private Decimal _amount;

    public void Validate()
    {
        ValidateAllProperties();
    }
}