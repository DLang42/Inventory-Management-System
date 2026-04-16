using IMS.CoreBusiness;
using IMS.UseCases.Activities.interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities;

public class SellProductUseCase : ISellProductUseCase
{
    private readonly IProductTransactionRepository productTransactionRepository;
    private readonly IProductRepository productRepository;
    
    public SellProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
    {
        this.productTransactionRepository = productTransactionRepository;
        this.productRepository = productRepository;
    }
    
    public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        // Sell product
        await productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);
        
        // Decrease quantity of product
        product.Quantity -= quantity;
        await productRepository.UpdateProductAsync(product);
    }
}