using System.Runtime.InteropServices.JavaScript;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly IInventoryRepository inventoryRepository;
    public List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();

    public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }
    
    public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        inventoryTransactions.Add(new InventoryTransaction
        {
            PONumber = poNumber,
            Inventory = inventory,
            InventoryId = inventory.InventoryId,
            QuantityBefore =  inventory.Quantity,
            QuantityAfter =  inventory.Quantity + quantity,
            ActivityType = InventoryTransactionType.PurchaseInventory,
            TransactionDate =  DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });
    }

    public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy,
        double price)
    {
        inventoryTransactions.Add(new InventoryTransaction
        {
            ProductionNumber = productionNumber,
            Inventory = inventory,
            InventoryId = inventory.InventoryId,
            QuantityBefore =  inventory.Quantity,
            QuantityAfter =  inventory.Quantity - quantityToConsume,
            ActivityType = InventoryTransactionType.ProduceProduct,
            TransactionDate =  DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });
    }

    public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo,
        InventoryTransactionType? transactionType)
    {
        var inventories = (await inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();
        
        // SELECT * FROM inventorytransactions it JOIN inventories inv ON it.inventoryid = inv.inventoryid

        var query = from it in inventoryTransactions
            join inv in inventories on it.InventoryId equals inv.InventoryId
            where (string.IsNullOrWhiteSpace(inventoryName) ||
                   inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                  && (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                  (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                  (!transactionType.HasValue || it.ActivityType == transactionType)
                  select new InventoryTransaction
            {
                Inventory = inv,
                InventoryTransactionId = it.InventoryTransactionId,
                PONumber = it.PONumber,
                InventoryId =  it.InventoryId,
                QuantityBefore = it.QuantityBefore,
                ActivityType =  it.ActivityType,
                QuantityAfter =  it.QuantityAfter,
                TransactionDate = it.TransactionDate,
                DoneBy = it.DoneBy,
                UnitPrice = it.UnitPrice
            };
        
        return query;
    }
}