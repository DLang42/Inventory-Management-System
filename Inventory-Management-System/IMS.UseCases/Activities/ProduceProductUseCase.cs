using IMS.CoreBusiness;
using IMS.UseCases.Activities.interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities;

public class ProduceProductUseCase : IProduceProductUseCase
{
    private readonly IProductRepository productRepository;
    private readonly IProductTransactionRepository productTransactionRepository;

    public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
    {
        this.productRepository = productRepository;
        this.productTransactionRepository = productTransactionRepository;
    }
    
    public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
    {
        // Add transaction record
        await productTransactionRepository.ProduceAsync(productionNumber,product, quantity, doneBy);
        // Decrease the quantity of inventories
    
        // Update the quantity of the product
        product.Quantity += quantity;
        await productRepository.UpdateProductAsync(product);
    }
}