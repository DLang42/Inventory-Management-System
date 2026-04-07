using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.interfaces;

public interface IEditProductUseCase
{
    Task ExecuteAsync(Product product);
}