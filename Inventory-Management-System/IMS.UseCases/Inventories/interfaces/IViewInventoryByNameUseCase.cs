using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.interfaces
{
    public interface IViewInventoryByNameUseCase
    {
        Task<IEnumerable<Inventory>> ExecuteAsync(string name = "");
    }
}