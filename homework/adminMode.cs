using System;

namespace VendingMachineApp;

public static class adminMode
{
    public static void Run(vendingMachine vm)
    {
        Console.WriteLine("\n=== АДМИНИСТРАТОРСКИЙ РЕЖИМ ===");
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" list");
        Console.WriteLine(" restock <id> <qty>");
        Console.WriteLine(" price <id> <newPrice>");
        Console.WriteLine(" income");
        Console.WriteLine(" collect");
        Console.WriteLine(" back");
        Console.WriteLine(" help");

        while (true)
        {
            Console.Write("admin> ");

            var input = Console.ReadLine();
            if (input == null) continue;
            input = input.Trim();
            if (input.Length == 0) continue;

            if (input.Equals("back", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Выход из админ-режима.");
                return;
            }

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "help":
                    PrintDetailedHelp();
                    break;

                case "list":
                    Console.WriteLine("Список товаров:");
                    foreach (var p in vm.Products)
                        Console.WriteLine($"{p.Id}. {p.Name} — {p.Price:F2}₽ ({p.Quantity} шт.)");
                    break;

                case "restock":
                    if (parts.Length < 3)
                    {
                        Console.WriteLine("Использование: restock <id> <qty>\nПример: restock 1 5");
                        break;
                    }
                    try
                    {
                        int idR = int.Parse(parts[1]);
                        int qty = int.Parse(parts[2]);

                        if (idR <= 0 || qty <= 0)
                        {
                            Console.WriteLine("Ошибка: <id> и <qty> должны быть положительными целыми числами.");
                            break;
                        }

                        bool ok = vm.RestockProduct(idR, qty);
                        if (ok)
                            Console.WriteLine($"Добавлено {qty} шт. товара с id={idR}.");
                        else
                            Console.WriteLine("Товар с таким id не найден.");
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка: аргументы должны быть целыми числами.");
                    }
                    break;

                case "price":
                    if (parts.Length < 3)
                    {
                        Console.WriteLine("Использование: price <id> <newPrice>\nПример: price 2 45");
                        break;
                    }
                    try
                    {
                        int idP = int.Parse(parts[1]);
                        decimal newPrice = decimal.Parse(parts[2].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

                        if (idP <= 0 || newPrice <= 0)
                        {
                            Console.WriteLine("Ошибка: id и цена должны быть положительными числами.");
                            break;
                        }

                        bool ok = vm.SetPrice(idP, newPrice);
                        if (ok)
                            Console.WriteLine($"Новая цена для id={idP}: {newPrice:F2}₽");
                        else
                            Console.WriteLine("Товар с таким id не найден.");
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка: введите корректные числовые значения (пример: price 2 45).");
                    }
                    break;

                case "income":
                    Console.WriteLine($"\n📊 Текущая выручка: {vm.EarnedMoney:F2}₽");
                    break;

                case "collect":
                    {
                        decimal amount = vm.CollectMoney();
                        if (amount > 0m)
                            Console.WriteLine($"Выдана администратору выручка: {amount:F2}₽.");
                        else
                            Console.WriteLine("Выручка пустая. Собирать нечего.");
                    }
                    break;

                default:
                    Console.WriteLine("Неизвестная команда администратора. Введите 'help' для описания всех команд.");
                    break;
            }
        }
    }

    private static void PrintDetailedHelp()
    {
        Console.WriteLine("\n=== СПРАВКА ПО КОМАНДАМ АДМИНИСТРАТОРА ===");
        Console.WriteLine("  list");
        Console.WriteLine("    Показать список всех товаров, их количество и текущие цены.");
        Console.WriteLine();
        Console.WriteLine("  restock <id> <qty>");
        Console.WriteLine("    Пополнить запас товара с указанным идентификатором <id> на <qty> штук.");
        Console.WriteLine("    Пример: restock 1 5 — добавит 5 единиц товара с id=1.");
        Console.WriteLine();
        Console.WriteLine("  price <id> <newPrice>");
        Console.WriteLine("    Установить новую цену товара <id> в рублях.");
        Console.WriteLine("    Пример: price 2 45 — установить цену 45₽ для товара с id=2.");
        Console.WriteLine();
        Console.WriteLine("  income");
        Console.WriteLine("    Показать текущую выручку (накопленные средства от покупок).");
        Console.WriteLine();
        Console.WriteLine("  collect");
        Console.WriteLine("    Выдать администратору накопленную выручку (обнулить счётчик выручки).");
        Console.WriteLine();
        Console.WriteLine("  back");
        Console.WriteLine("    Выйти из режима администратора и вернуться в режим покупателя.");
        Console.WriteLine();
        Console.WriteLine("  help");
        Console.WriteLine("    Показать это подробное описание всех команд администратора.");
        Console.WriteLine("===========================================");
    }
}
