using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
// using IMS.Plugins.InMemory;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.interfaces;

namespace IMS.UseCases.Inventories
{
    public class ViewInventoryByNameUseCase : IViewInventoryByNameUseCase
    {
        private readonly IInventoryRepository inventoryRepository;
        
        public ViewInventoryByNameUseCase(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }
        
        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
        {
            return await inventoryRepository.GetInventoriesByNameAsync(name);
        }
    }
}