using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModels.Validations;

public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var sellVIewModel = validationContext.ObjectInstance as SellViewModel;
        if (sellVIewModel != null && sellVIewModel.Product != null && sellVIewModel.Product.Quantity < sellVIewModel.QuantityToSell)
        {
            return new ValidationResult($"There isn't enough product. There is only {sellVIewModel.Product.Quantity} in the warehouse.", 
                new[] { validationContext.MemberName });
        }
        return ValidationResult.Success;
    }
}