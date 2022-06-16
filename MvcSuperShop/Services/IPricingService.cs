using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;

namespace MvcSuperShop.Services;

public interface IPricingService
{
    IEnumerable<ProductServiceModel> CalculatePrices(IEnumerable<ProductServiceModel> products, CurrentCustomerContext context);
    public bool AgreementIsValid(Agreement agreement);

}