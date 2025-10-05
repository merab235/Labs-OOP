using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    class Program
    {
        private static List<Product> products = new List<Product>
        {
            new Product("Кола", 150m, 5),
            new Product("Чипсы", 100m, 3),
            new Product("Шоколад", 80m, 7),
            new Product("Вода", 50m, 10)
        };

        private static decimal currentBalance = 0;
        private static decimal totalRevenue = 0; // Собранные деньги
        private static readonly decimal[] coinValues = { 1m, 2m, 5m, 10m, 50m, 100m }; // Монеты в рублях
        private const string AdminPassword = "admin123"; // Пароль администратора

        static void Main(string[] args)
        {
            Console.WriteLine("🎰 ТОРГОВЫЙ АВТОМАТ");
            Console.WriteLine("===================");

            bool running = true;
            
            while (running)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        InsertCoins();
                        break;
                    case "3":
                        BuyProduct();
                        break;
                    case "4":
                        ReturnCoins();
                        break;
                    case "5":
                        AdminMode();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        break;
                }
            }

            Console.WriteLine("👋 Спасибо за использование автомата!");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine($"\n💰 Текущий баланс: {currentBalance}₽");
            Console.WriteLine("\n📋 ГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("1. 📦 Показать товары");
            Console.WriteLine("2. 💰 Вставить монеты");
            Console.WriteLine("3. 🛒 Купить товар");
            Console.WriteLine("4. 🔄 Вернуть монеты");
            Console.WriteLine("5. 🔧 Режим администратора");
            Console.WriteLine("6. 🚪 Выйти");
            Console.Write("Ваш выбор: ");
        }

        static void ShowProducts()
        {
            Console.WriteLine("\n📦 ДОСТУПНЫЕ ТОВАРЫ:");
            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i]}");
            }
        }

        static void InsertCoins()
        {
            Console.WriteLine("\n💰 ВСТАВИТЬ МОНЕТЫ:");
            Console.WriteLine("1. 1 рубль");
            Console.WriteLine("2. 2 рубля");
            Console.WriteLine("3. 5 рублей");
            Console.WriteLine("4. 10 рублей");
            Console.WriteLine("5. 50 рублей");
            Console.WriteLine("6. 100 рублей");
            Console.Write("Выберите монету: ");
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 6)
            {
                currentBalance += coinValues[choice - 1];
                Console.WriteLine($"✅ Монета {coinValues[choice - 1]}₽ вставлена. Баланс: {currentBalance}₽");
            }
            else
            {
                Console.WriteLine("❌ Неверный выбор монеты.");
            }
        }

        static void BuyProduct()
        {
            if (products.All(p => p.Quantity <= 0))
            {
                Console.WriteLine("❌ Все товары распроданы.");
                return;
            }

            ShowProducts();
            Console.Write("Выберите товар (номер): ");
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= products.Count)
            {
                Product product = products[choice - 1];
                
                if (product.Quantity <= 0)
                {
                    Console.WriteLine("❌ Товар распродан.");
                    return;
                }

                if (currentBalance >= product.Price)
                {
                    // Успешная покупка
                    product.Quantity--;
                    totalRevenue += product.Price; // Добавляем в кассу
                    decimal change = currentBalance - product.Price;
                    currentBalance = 0;
                    
                    Console.WriteLine($"\n✅ Покупка успешна! Вы купили: {product.Name}");
                    if (change > 0)
                    {
                        Console.WriteLine($"💰 Сдача: {change}₽");
                    }
                    Console.WriteLine("🎉 Приятного аппетита!");
                }
                else
                {
                    Console.WriteLine($"❌ Недостаточно средств. Не хватает {product.Price - currentBalance}₽");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный выбор товара.");
            }
        }

        static void ReturnCoins()
        {
            if (currentBalance > 0)
            {
                Console.WriteLine($"💰 Монеты возвращены: {currentBalance}₽");
                currentBalance = 0;
            }
            else
            {
                Console.WriteLine("❌ Нет монет для возврата.");
            }
        }

        // 🔧 РЕЖИМ АДМИНИСТРАТОРА
        static void AdminMode()
        {
            Console.Write("\n🔐 Пароль администратора: ");
            string password = Console.ReadLine();

            if (password != AdminPassword)
            {
                Console.WriteLine("❌ Неверный пароль.");
                return;
            }

            bool inAdminMode = true;
            while (inAdminMode)
            {
                Console.WriteLine($"\n🔧 РЕЖИМ АДМИНИСТРАТОРА - Касса: {totalRevenue}₽");
                Console.WriteLine("1. 📦 Пополнить товары");
                Console.WriteLine("2. 💵 Собрать деньги");
                Console.WriteLine("3. 📊 Посмотреть статистику");
                Console.WriteLine("4. 🔙 Вернуться в главное меню");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RestockProducts();
                        break;
                    case "2":
                        CollectMoney();
                        break;
                    case "3":
                        ShowStatistics();
                        break;
                    case "4":
                        inAdminMode = false;
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор.");
                        break;
                }
            }
        }

        static void RestockProducts()
        {
            Console.WriteLine("\n📦 ПОПОЛНЕНИЕ ЗАПАСОВ:");
            ShowProducts();
            Console.Write("Выберите товар для пополнения (номер): ");
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= products.Count)
            {
                Console.Write("Количество для добавления: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    products[choice - 1].Quantity += quantity;
                    Console.WriteLine($"✅ {quantity} {products[choice - 1].Name} добавлено. Новый запас: {products[choice - 1].Quantity}");
                }
                else
                {
                    Console.WriteLine("❌ Неверное количество.");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный выбор товара.");
            }
        }

        static void CollectMoney()
        {
            if (totalRevenue > 0)
            {
                Console.WriteLine($"\n💵 СБОР ДЕНЕГ");
                Console.WriteLine($"Сумма для сбора: {totalRevenue}₽");
                Console.Write("Подтвердить сбор (Д/Н): ");
                
                if (Console.ReadLine().ToUpper() == "Д")
                {
                    Console.WriteLine($"✅ {totalRevenue}₽ успешно собрано!");
                    totalRevenue = 0;
                }
                else
                {
                    Console.WriteLine("❌ Сбор отменен.");
                }
            }
            else
            {
                Console.WriteLine("❌ Нет денег для сбора.");
            }
        }

        static void ShowStatistics()
        {
            Console.WriteLine("\n📊 СТАТИСТИКА:");
            Console.WriteLine($"💰 Денег в кассе: {totalRevenue}₽");
            Console.WriteLine($"📦 Товаров продано сегодня: {CalculateTotalSold()}");
            Console.WriteLine("\n📈 СОСТОЯНИЕ ЗАПАСОВ:");
            foreach (var product in products)
            {
                string status = product.Quantity > 0 ? "✅ В наличии" : "❌ Распродан";
                Console.WriteLine($"{product.Name}: {product.Quantity} единиц - {status}");
            }
        }

        static int CalculateTotalSold()
        {
            // Упрощенный расчет
            int total = 0;
            foreach (var product in products)
            {
                total += (10 - product.Quantity); // Предполагаем начальный запас 10
            }
            return total;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Name} - {Price}₽ - Запас: {Quantity}";
        }
    }
}