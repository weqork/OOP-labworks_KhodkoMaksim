namespace VendingMachineApp;

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var vm = new VendingMachine();
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
            if (input == null)
                continue;

            input = input.Trim();
            if (input.Length == 0)
                continue;

            if (input.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                AdminMode.Run(vm);
                continue;
            }

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "products":
                    vm.PrintProducts();
                    break;

                case "insert":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Использование: insert <номинал>\nПример: insert 10");
                        break;
                    }
                    try
                    {
                        var normalized = parts[1].Replace(',', '.');
                        decimal coin = decimal.Parse(normalized, System.Globalization.CultureInfo.InvariantCulture);

                        if (coin <= 0)
                        {
                            Console.WriteLine("Номинал должен быть положительным.");
                            break;
                        }

                        if (vm.InsertCoin(coin))
                            Console.WriteLine($"Баланс: {vm.InsertedAmount:F2}₽");
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка: номинал должен быть числом. Пример: insert 10");
                    }
                    break;

                case "balance":
                    Console.WriteLine($"Баланс: {vm.InsertedAmount:F2}₽");
                    break;

                case "buy":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Использование: buy <id>\nПример: buy 2");
                        break;
                    }
                    try
                    {
                        int id = int.Parse(parts[1]);
                        if (id <= 0)
                        {
                            Console.WriteLine("ID товара должен быть положительным целым числом.");
                            break;
                        }
                        vm.BuyProduct(id);
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка: ID товара должен быть числом. Пример: buy 2");
                    }
                    break;

                case "cancel":
                    vm.Cancel();
                    break;

                case "exit":
                    Console.WriteLine("До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }
}
