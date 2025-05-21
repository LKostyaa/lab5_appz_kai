using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.BLL.Interfaces;
using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChildrenLeisure.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;

        public OrderService(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        public OrderDto CreateOrder(
            string customerName,
            string phoneNumber,
            bool isBirthdayParty,
            int? fairyCharacterId = null,
            int[] attractionIds = null)
        {
            var order = new Order
            {
                CustomerName = customerName,
                PhoneNumber = phoneNumber,
                OrderDate = DateTime.Now,
                IsBirthdayParty = isBirthdayParty,
                Status = OrderStatus.Pending
            };

            // Додавання казкового героя
            if (fairyCharacterId.HasValue)
            {
                var character = _unitOfWork.FairyCharacterRepository.GetById(fairyCharacterId.Value);
                if (character != null)
                {
                    order.FairyCharacterId = fairyCharacterId;
                    order.FairyCharacter = character;
                }
            }

            // Додавання атракціонів
            if (attractionIds != null && attractionIds.Length > 0)
            {
                order.SelectedAttractions = _unitOfWork.AttractionRepository
                    .GetAll()
                    .Where(a => attractionIds.Contains(a.Id))
                    .ToList();
            }

            // Конвертуємо в DTO для розрахунку ціни
            var orderDto = _mapper.Map<OrderDto>(order);

            // Розрахунок ціни
            order.TotalPrice = _pricingService.CalculateOrderPrice(orderDto);

            // Додаємо і зберігаємо в базу
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.Save();

            return _mapper.Map<OrderDto>(order);
        }

        public OrderDto GetOrderLazy(int orderId)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public OrderDto GetOrderEager(int orderId)
        {
            var order = _unitOfWork.OrderRepository
                .GetAll()
                .Where(o => o.Id == orderId)
                .Include(o => o.FairyCharacter)
                .Include(o => o.SelectedAttractions)
                .FirstOrDefault();

            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public OrderDto ConfirmOrder(int orderId)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Замовлення з ID {orderId} не знайдено");
            }

            order.Status = OrderStatus.Confirmed;
            _unitOfWork.OrderRepository.Update(order);
            _unitOfWork.Save();

            return _mapper.Map<OrderDto>(order);
        }

        public List<OrderDto> GetAllOrders()
        {
            return _unitOfWork.OrderRepository
                .GetAll()
                .AsNoTracking()
                .Select(o => _mapper.Map<OrderDto>(o))
                .ToList();
        }
    }
}
