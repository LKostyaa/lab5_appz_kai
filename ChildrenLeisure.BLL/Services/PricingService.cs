using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.BLL.Interfaces;
using System.Linq;

namespace ChildrenLeisure.BLL.Services
{
    public class PricingService : IPricingService
    {
        public decimal CalculateOrderPrice(OrderDto order)
        {
            decimal totalPrice = 0;

            // Базова вартість днів народження вища
            if (order.IsBirthdayParty)
            {
                totalPrice += 500; // Базова вартість організації дня народження
            }

            // Додавання вартості атракціонів
            if (order.SelectedAttractions != null && order.SelectedAttractions.Any())
            {
                totalPrice += order.SelectedAttractions.Sum(a => a.Price);
            }

            // Додавання вартості казкового героя
            if (order.FairyCharacter != null)
            {
                totalPrice += order.FairyCharacter.PricePerHour;
            }

            return totalPrice;
        }
    }
}