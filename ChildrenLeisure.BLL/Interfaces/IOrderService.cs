using ChildrenLeisure.BLL.DTOs;
using System.Collections.Generic;

namespace ChildrenLeisure.BLL.Interfaces
{
    public interface IOrderService
    {
        OrderDto CreateOrder(
            string customerName,
            string phoneNumber,
            bool isBirthdayParty,
            int? fairyCharacterId = null,
            int[] attractionIds = null);

        OrderDto GetOrderLazy(int orderId);
        OrderDto GetOrderEager(int orderId);
        OrderDto ConfirmOrder(int orderId);
        List<OrderDto> GetAllOrders();
    }
}