using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.interfaces;

public interface IAddProductUseCase
{
    Task ExecuteAsync(Product product);
}