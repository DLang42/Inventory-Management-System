using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.interfaces;

namespace IMS.UseCases.Reports;

public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
{
    private readonly IProductTransactionRepository productTransactionRepository;
    
    public SearchProductTransactionsUseCase(IProductTransactionRepository productTransactionRepository)
    {
        this.productTransactionRepository = productTransactionRepository;
    }

    public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(string productName, DateTime? dateFrom,
        DateTime? dateTo, ProductTransactionType? transactionType)
    {
        if (dateTo.HasValue)
        {
            dateTo = dateTo.Value.Date.AddDays(1);
        }

    return await productTransactionRepository.GetProductTransactionsAsync(productName, dateFrom, dateTo, 
            transactionType);
    }
}