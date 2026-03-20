using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.interfaces;

public interface IDeleteInventoryUseCase
{
    Task ExecuteAsync(int inventoryId);
}