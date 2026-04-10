using IMS.CoreBusiness;
using IMS.UseCases.Inventories.interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class ViewProductByIdUseCase : IViewProductByIdUseCase
{
    private readonly IProductRepository productRepository;
    
    public ViewProductByIdUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    
    public async Task<Product?> ExecuteAsync(int productId)
    {
        return await this.productRepository.GetProductByIdAsync(productId);
    }
}