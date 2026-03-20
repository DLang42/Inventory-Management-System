using IMS.CoreBusiness;
using IMS.UseCases.Inventories.interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class DeleteInventoryUseCase : IDeleteInventoryUseCase
{
    private readonly IInventoryRepository inventoryRepository;
    
    public DeleteInventoryUseCase(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }

    public async Task ExecuteAsync(int inventoryId)
    {
        await this.inventoryRepository.DeleteInventoryByIdAsync(inventoryId);
    }
}