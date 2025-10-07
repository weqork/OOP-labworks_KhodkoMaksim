using System;
using System.Globalization;
using System.Linq;

namespace VendingMachineApp;

public class userMode
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var vm = new vendingMachine();
        vm.AddProduct("Вода", 35, 5);
        vm.AddProduct("Кока кова", 50, 3);
        vm.AddProduct("Пепси кова", 45, 5);
        vm.AddProduct("Чоколад", 65, 2);
        vm.AddProduct("Чипсики", 45, 4);

        Console.WriteLine("=== ВЕНДИНГОВЫЙ АВТОМАТ ===");
        Console.WriteLine("Команды:");
        Console.WriteLine(" products — показать товары");
        Console.WriteLine(" insert <номинал> — внести монету (1,2,5,10,20,50)");
        Console.WriteLine(" balance — посмотреть сколько денег внесено");
        Console.WriteLine(" buy <id> — купить товар");
        Console.WriteLine(" cancel — вернуть внесённые деньги");
        Console.WriteLine(" exit — выход");
        Console.WriteLine(" (введите 'password' для входа в режим администратора)");

        while (true)
        {
            Console.Write(" > ");

            var input = Console.ReadLine();
            if (input == null) continue;
            input = input.Trim();
            if (input.Length == 0) continue;

            if (input.Equals("password"))
            {
               
                adminMode.Run(vm);
                continue;
            }

            var parts = input.Split(' ');
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "help":
                case "h":
                    PrintHelp();
                    break;

                case "products":
                    PrintProducts(vm);
                    break;

                case "insert":
                    HandleInsert(vm, parts);
                    break;

                case "balance":
                    Console.WriteLine($"Баланс: {vm.InsertedAmount:F2}₽");
                    break;

                case "buy":
                    HandleBuy(vm, parts);
                    break;

                case "cancel":
                    HandleCancel(vm);
                    break;

                case "exit":
                    Console.WriteLine("До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите help для справки.");
                    break;
            }
        }
    }
    

    private static void PrintHelp()
    {
        Console.WriteLine("Команды:");
        Console.WriteLine(" products                  — показать товары");
        Console.WriteLine(" insert <номинал>          — внести монету (1,2,5,10,20,50)");
        Console.WriteLine(" balance                   — посмотреть сколько денег внесено");
        Console.WriteLine(" buy <id>                  — купить товар");
        Console.WriteLine(" cancel                    — вернуть внесённые деньги");
        Console.WriteLine(" exit                      — выход");
        Console.WriteLine(" password                  — вход в режим администратора");
    }

    private static void PrintProducts(vendingMachine vm)
    {
        Console.WriteLine("Список товаров:");
        foreach (var p in vm.Products)
            Console.WriteLine($"{p.Id}. {p.Name} — {p.Price:F2}₽ ({p.Quantity} шт.)");
    }

    private static void HandleInsert(vendingMachine vm, string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("Использование: insert <номинал>\nПример: insert 10");
            return;
        }

        try
        {
            var normalized = parts[1].Replace(',', '.');
            decimal coin = decimal.Parse(normalized, CultureInfo.InvariantCulture);

            decimal[] allowed = { 1, 2, 5, 10, 20, 50 };
            if (!allowed.Contains(coin))
            {
                Console.WriteLine("Такой номинал не принимается! Доступно: 1, 2, 5, 10, 20, 50.");
                return;
            }

            vm.InsertCoin(coin);
            Console.WriteLine($"Баланс: {vm.InsertedAmount:F2}₽");
        }
        catch
        {
            Console.WriteLine("Ошибка: номинал должен быть числом. Пример: insert 10");
        }
    }

    private static void HandleBuy(vendingMachine vm, string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("Использование: buy <id>\nПример: buy 2");
            return;
        }

        try
        {
            int id = int.Parse(parts[1]);
            if (id <= 0)
            {
                Console.WriteLine("ID товара должен быть положительным целым числом.");
                return;
            }

            decimal result = vm.BuyProduct(id);

            if (result == -1m)
            {
                Console.WriteLine("Такого товара нет.");
            }
            else if (result == -2m)
            {
                Console.WriteLine("Товар закончился.");
            }
            else if (result < 0m)
            {
                Console.WriteLine($"Недостаточно средств. Нужно ещё {Math.Abs(result):F2}₽.");
            }
            else
            {
                var product = vm.Products.FirstOrDefault(p => p.Id == id);
                var productName = product?.Name ?? "товар";
                Console.WriteLine($"Вы получили {productName}. Спасибо за покупку!");
                if (result > 0m)
                    Console.WriteLine($"Ваша сдача: {result:F2}₽");
            }
        }
        catch
        {
            Console.WriteLine("Ошибка: ID товара должен быть числом. Пример: buy 2");
        }
    }

    private static void HandleCancel(vendingMachine vm)
    {
        decimal refund = vm.Cancel();
        if (refund > 0m)
            Console.WriteLine($"Возврат внесённых средств: {refund:F2}₽");
        else
            Console.WriteLine("Возвращать нечего.");
    }
}
