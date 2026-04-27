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

    public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        productTransactions.Add(new ProductTransaction
        {
            ActivityType = ProductTransactionType.SellProduct,
            SONumber =  salesOrderNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            QuantityAfter = product.Quantity - quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = unitPrice
        });
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo,
        ProductTransactionType? transactionType)
    {
        var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();
        
        // SELECT * FROM producttransactions it JOIN products inv ON it.productid = inv.productid

        var query = from pt in productTransactions
            join prod in products on pt.ProductId equals prod.ProductId
            where (string.IsNullOrWhiteSpace(productName) ||
                   prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                  && (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                  (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                  (!transactionType.HasValue || pt.ActivityType == transactionType)
            select new ProductTransaction
            {
                Product = prod,
                ProductTransactionId = pt.ProductTransactionId,
                SONumber = pt.SONumber,
                ProductId =  pt.ProductId,
                QuantityBefore = pt.QuantityBefore,
                ActivityType =  pt.ActivityType,
                QuantityAfter =  pt.QuantityAfter,
                TransactionDate = pt.TransactionDate,
                DoneBy = pt.DoneBy,
                UnitPrice = pt.UnitPrice
            };
        
        return query;
    }
}