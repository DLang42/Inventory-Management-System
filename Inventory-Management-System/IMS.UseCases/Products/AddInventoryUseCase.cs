using IMS.CoreBusiness;
using IMS.UseCases.Inventories.interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{
    public class AddProductUseCase : IAddProductUseCase
    {

        private readonly IProductRepository productRepository;
        
        public AddProductUseCase(IProductRepository productRepository)
        {
                this.productRepository = productRepository;
        }
        
        public async Task ExecuteAsync(Product product)
        {
            await this.productRepository.AddProductAsync(product);
        }
    }    
}

