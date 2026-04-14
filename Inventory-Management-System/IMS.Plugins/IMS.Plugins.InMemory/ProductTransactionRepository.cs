using System.Runtime.InteropServices.JavaScript;
using IMS.CoreBusiness;
using IMS.CoreBusiness.Validations;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class ProductTransactionRepository : IProductTransactionRepository
{

    private List<ProductTransaction> productTransactions = new List<ProductTransaction>();
    
    private readonly IProductRepository productRepository;
    private readonly IInventoryTransactionRepository inventoryTransactionRepository;
    private readonly IInventoryRepository inventoryRepository;
    
    public ProductTransactionRepository(IProductRepository productRepository, IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository)
    {
        this.productRepository = productRepository;
        this.inventoryTransactionRepository = inventoryTransactionRepository;
        this.inventoryRepository = inventoryRepository;
    }
    
    public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
    {
        var prod = await productRepository.GetProductByIdAsync(product.ProductId);
        if (prod != null)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory != null)
                {
                    // Add inventory transaction
                    inventoryTransactionRepository.ProduceAsync(productionNumber, 
                        pi.Inventory, 
                        pi.InventoryQuantity * quantity, 
                        doneBy, -1);
                    // Decrease the inventories
                    var inv = await inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }
        // Add product transaction
        productTransactions.Add(new ProductTransaction
        {
            ProductionNumber = productionNumber,
            ProductId = product.ProductId,
            QuantityBefore =  product.Quantity,
            ActivityType = ProductTransactionType.ProductProduct,
            QuantityAfter =  product.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            Product = product,
        });
    }
}