using System.ComponentModel.DataAnnotations;
using IMS.CoreBusiness;
using Inventory_Management_System.ViewModels.Validations;
using Microsoft.VisualBasic.CompilerServices;

namespace Inventory_Management_System.ViewModels;

public class ProduceViewModel
{
    [Required] public string ProductionNumber { get; set; } = string.Empty;
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You have to select an product.")]
    public int ProductId { get; set; }
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
    [Produce_EnsureEnoughInventoryQuantity]
    public int QuantityToProduce { get; set; }
    public Product? Product { get; set; } = null;
}