using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.interfaces;

namespace IMS.UseCases.Reports;

public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionsUseCase
{
    private readonly IInventoryTransactionRepository inventoryTransactionRepository;
    
    public SearchInventoryTransactionsUseCase(IInventoryTransactionRepository inventoryTransactionRepository)
    {
        this.inventoryTransactionRepository = inventoryTransactionRepository;
    }

    public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateFrom,
        DateTime? dateTo, InventoryTransactionType? transactionType)
    {
        if (dateTo.HasValue)
        {
            dateTo = dateTo.Value.Date.AddDays(1);
        }

    return await inventoryTransactionRepository.GetInventoryTransactionsAsync(inventoryName, dateFrom, dateTo, 
            transactionType);
    }
}