using ChildrenLeisure.BLL.DTOs;

namespace ChildrenLeisure.BLL.Interfaces
{
    public interface IPricingService
    {
        decimal CalculateOrderPrice(OrderDto order);
    }
}