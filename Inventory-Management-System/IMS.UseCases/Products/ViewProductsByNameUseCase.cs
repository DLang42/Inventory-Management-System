using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
// using IMS.Plugins.InMemory;
using IMS.UseCases.Products;
using IMS.UseCases.Products.interfaces;

namespace IMS.UseCases.Products
{
    public class ViewProductsByNameUseCase : IViewProductsByNameUseCase
    {
        private readonly IProductRepository productRepository;
        
        public ViewProductsByNameUseCase(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        
        public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
        {
            return await productRepository.GetProductsByNameAsync(name);
        }
    }
}