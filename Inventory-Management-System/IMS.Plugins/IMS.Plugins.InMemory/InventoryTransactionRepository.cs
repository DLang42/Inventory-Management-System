using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    public List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();
    public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        this.inventoryTransactions.Add(new InventoryTransaction
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
}