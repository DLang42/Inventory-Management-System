using System.Globalization;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        private readonly IInventoryRepository inventoryRepository;
        
        public ProductRepository(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
            _products = new List<Product>()
            {
                new Product {ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150},
                new Product {ProductId = 2, ProductName = "Car", Quantity = 10, Price = 25000},
            };
        }
        
        public Task AddProductAsync(Product product)
        {
            if (_products.Any((x =>
                    x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase))))
            {
                return Task.CompletedTask;
            }
            
            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId + 1;
            
            _products.Add(product);
            
            return Task.CompletedTask;
        }
        
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name = "")
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);
            
            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateProductAsync(Product product)
        {
            // To prevent different product from having the same name
            if (_products.Any(x =>
                    x.ProductId != product.ProductId && x.ProductName.Equals(product.ProductName,
                        StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;
            
            var newProduct = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (newProduct is not null)
            {
                newProduct.ProductName = product.ProductName;
                newProduct.Quantity = product.Quantity;
                newProduct.Price = product.Price;
                newProduct.ProductInventories = product.ProductInventories;
            }
            
            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = await Task.FromResult(_products.FirstOrDefault(x => x.ProductId == productId));
            Product? newProduct = null;
            if (product != null)
            {
                newProduct = new Product();
                newProduct.ProductId = product.ProductId;
                newProduct.ProductName = product.ProductName;
                newProduct.Quantity = product.Quantity;
                newProduct.Price = product.Price;
                newProduct.ProductInventories = new List<ProductInventory>();
                if (product.ProductInventories != null && product.ProductInventories.Count > 0)
                {
                    foreach (var productInventory in product.ProductInventories)
                    {
                        var newProductInventory = new ProductInventory
                        {
                            InventoryId = productInventory.InventoryId,
                            ProductId = productInventory.ProductId,
                            Product = product,
                            Inventory = new Inventory(),
                            InventoryQuantity =  productInventory.InventoryQuantity
                        };
                        if (productInventory.Inventory != null)
                        {
                            var inv = await inventoryRepository.GetInventoryByIdAsync(productInventory.Inventory.InventoryId);
                            if (inv != null)
                            {
                                newProductInventory.Inventory.InventoryId = inv.InventoryId;
                                newProductInventory.Inventory.InventoryName = inv.InventoryName;
                                newProductInventory.Inventory.Quantity = inv.Quantity;
                                newProductInventory.Inventory.Price = inv.Price;
                            }
                        }
                        newProduct.ProductInventories.Add(newProductInventory);
                    }
                }
            }

            return await Task.FromResult(newProduct);
        }

        public Task DeleteProductByIdAsync(int productId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == productId);
            if (product is not null)
            {
                _products.Remove(product);
            }
            
            return Task.CompletedTask;
        }
    }
}