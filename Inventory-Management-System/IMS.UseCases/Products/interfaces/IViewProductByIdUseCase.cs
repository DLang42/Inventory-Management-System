using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.interfaces;

public interface IViewProductByIdUseCase
{
    Task<Product?> ExecuteAsync(int productId);
}