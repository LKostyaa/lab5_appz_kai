using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.BLL.Interfaces;
using ChildrenLeisure.BLL.Services;
using ChildrenLeisure.DAL.Data;
using ChildrenLeisure.DAL.Interfaces;
using ChildrenLeisure.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ChildrenLeisure.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            // Unit of Work та сервіси
            var unitOfWork = new UnitOfWork(context);
            var pricingService = new PricingService();
            var entertainmentService = new EntertainmentService(unitOfWork);
            var orderService = new OrderService(unitOfWork, pricingService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДИТЯЧЕ ДОЗВІЛЛЯ ===");
                Console.WriteLine("1. Переглянути атракціони");
                Console.WriteLine("2. Переглянути казкових героїв");
                Console.WriteLine("3. Створити замовлення");
                Console.WriteLine("4. Підтвердити замовлення");
                Console.WriteLine("5. Переглянути замовлення (ліниве)");
                Console.WriteLine("6. Переглянути замовлення (жадібне)");
                Console.WriteLine("7. Переглянути всі замовлення");
                Console.WriteLine("0. Вийти");
                Console.Write("Вибір: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowAttractions(entertainmentService);
                            break;
                        case "2":
                            ShowCharacters(entertainmentService);
                            break;
                        case "3":
                            CreateOrder(orderService, entertainmentService);
                            break;
                        case "4":
                            ConfirmOrder(orderService);
                            break;
                        case "5":
                            ShowOrderLazy(orderService);
                            break;
                        case "6":
                            ShowOrderEager(orderService);
                            break;
                        case "7":
                            ShowAllOrders(orderService);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Невірний вибір.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сталася помилка: {ex.Message}");
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        static void ShowAttractions(IEntertainmentService service)
        {
            Console.WriteLine("=== АТРАКЦІОНИ ===");
            foreach (var a in service.GetAllAttractions())
            {
                Console.WriteLine($"{a.Id}. {a.Name} - {a.Description} ({a.Price} грн)");
            }
        }

        static void ShowCharacters(IEntertainmentService service)
        {
            Console.WriteLine("=== КАЗКОВІ ГЕРОЇ ===");
            foreach (var c in service.GetAllFairyCharacters())
            {
                Console.WriteLine($"{c.Id}. {c.Name} ({c.PricePerHour} грн/год) - {c.Description}");
            }
        }

        static void CreateOrder(IOrderService orderService, IEntertainmentService entertainmentService)
        {
            Console.WriteLine("=== СТВОРЕННЯ ЗАМОВЛЕННЯ ===");
            Console.Write("Ім'я замовника: ");
            string name = Console.ReadLine();

            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            Console.Write("Це день народження? (y/n): ");
            bool isBday = Console.ReadLine().Trim().ToLower() == "y";

            ShowCharacters(entertainmentService);
            Console.Write("ID казкового героя (або пропустіть): ");
            string charInput = Console.ReadLine();
            int? charId = int.TryParse(charInput, out int cId) ? cId : null;

            ShowAttractions(entertainmentService);
            Console.Write("ID атракціонів через кому (або пропустіть): ");
            var attrInput = Console.ReadLine();
            int[] attrIds = null;

            if (!string.IsNullOrWhiteSpace(attrInput))
            {
                attrIds = attrInput.Split(',')
                    .Select(s => int.TryParse(s.Trim(), out int val) ? val : -1)
                    .Where(id => id > 0)
                    .ToArray();
            }

            try
            {
                var order = orderService.CreateOrder(name, phone, isBday, charId, attrIds);
                Console.WriteLine($"ЗАМОВЛЕННЯ СТВОРЕНО. ЦІНА: {order.TotalPrice} грн. Статус: {order.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні замовлення: {ex.Message}");
            }
        }

        static void ConfirmOrder(IOrderService service)
        {
            Console.Write("Введіть ID замовлення для підтвердження: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var updated = service.ConfirmOrder(id);
                    Console.WriteLine($"Замовлення {id} підтверджено. Статус: {updated.Status}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Невірне значення ID.");
            }
        }

        static void ShowOrderLazy(IOrderService service)
        {
            Console.Write("ID замовлення: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var order = service.GetOrderLazy(id);
                if (order == null)
                {
                    Console.WriteLine("Замовлення не знайдено.");
                    return;
                }

                DisplayOrderDetails(order);
            }
        }

        static void ShowOrderEager(IOrderService service)
        {
            Console.Write("ID замовлення: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var order = service.GetOrderEager(id);
                if (order == null)
                {
                    Console.WriteLine("Замовлення не знайдено.");
                    return;
                }

                DisplayOrderDetails(order);
            }
        }

        static void ShowAllOrders(IOrderService service)
        {
            Console.WriteLine("=== ВСІ ЗАМОВЛЕННЯ ===");
            var orders = service.GetAllOrders();

            if (orders.Count == 0)
            {
                Console.WriteLine("Замовлень немає.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"#{order.Id} | {order.CustomerName} | {order.OrderDate:dd.MM.yyyy HH:mm} | {order.TotalPrice} грн | {order.Status}");
            }
        }

        static void DisplayOrderDetails(OrderDto order)
        {
            Console.WriteLine($"Замовлення #{order.Id}, клієнт: {order.CustomerName}");
            Console.WriteLine($"Дата: {order.OrderDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Телефон: {order.PhoneNumber}");
            Console.WriteLine($"День народження: {(order.IsBirthdayParty ? "Так" : "Ні")}");
            Console.WriteLine($"Герой: {order.FairyCharacter?.Name ?? "Немає"}");
            Console.WriteLine($"Статус: {order.Status}");
            Console.WriteLine($"Загальна ціна: {order.TotalPrice} грн");

            Console.WriteLine("Атракціони:");
            if (order.SelectedAttractions != null && order.SelectedAttractions.Any())
            {
                foreach (var a in order.SelectedAttractions)
                {
                    Console.WriteLine($"- {a.Name} ({a.Price} грн)");
                }
            }
            else
            {
                Console.WriteLine("- Немає вибраних атракціонів");
            }
        }
    }
}