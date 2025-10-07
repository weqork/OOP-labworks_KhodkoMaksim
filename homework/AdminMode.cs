using System;

namespace VendingMachineApp;

public static class AdminMode
{
    public static void Run(VendingMachine vm)
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

            if (input.Equals("back"))
            {
                Console.WriteLine("Выход из админ-режима.");
                return;
            }

            var parts = input.Split(' ');
            if (parts.Length == 0) continue;

            var cmd = parts[0];

            switch (cmd)
            {
                case "help":
                    PrintDetailedHelp();
                    break;

                case "list":
                    Console.WriteLine();
                    vm.PrintProducts();
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

                        vm.RestockProduct(idR, qty);
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
                        decimal newPrice = decimal.Parse(parts[2].Replace(',', '.'));

                        if (idP <= 0 || newPrice <= 0)
                        {
                            Console.WriteLine("Ошибка: id и цена должны быть положительными числами.");
                            break;
                        }

                        vm.SetPrice(idP, newPrice);
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
                    vm.CollectMoney();
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
        Console.WriteLine("    Пример: restock 1 5  — добавит 5 единиц товара с id=1.");
        Console.WriteLine();
        Console.WriteLine("  price <id> <newPrice>");
        Console.WriteLine("    Установить новую цену товара <id> в рублях.");
        Console.WriteLine("    Пример: price 2 45  — установить цену 45₽ для товара с id=2.");
        Console.WriteLine();
        Console.WriteLine("  income");
        Console.WriteLine("    Показать текущую выручку.");
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
