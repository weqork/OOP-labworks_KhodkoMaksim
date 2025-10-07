namespace VendingMachineApp;

public class VendingMachine
{
    public List<Product> Products { get; } = new();
    public decimal InsertedAmount { get; private set; }
    public decimal EarnedMoney { get; private set; }

    public void AddProduct(string name, decimal price, int quantity)
    {
        int id;

        if (Products.Count == 0)
        {
            id = 1;
        }
        else
        {
            id = Products.Max(p => p.Id) + 1;
        }

        Products.Add(new Product { Id = id, Name = name, Price = price, Quantity = quantity });
    }

    public void PrintProducts()
    {
        Console.WriteLine("Список товаров:");
        foreach (var p in Products)
            Console.WriteLine($"{p.Id}. {p.Name} — {p.Price:F2}₽ ({p.Quantity} шт.)");
    }

    public bool InsertCoin(decimal coin)
    {
        decimal[] allowed = { 1, 2, 5, 10, 20, 50 };
        if (!allowed.Contains(coin))
        {
            Console.WriteLine("Такой номинал не принимается! Доступно: 1, 2, 5, 10, 20, 50.");
            return false;
        }

        InsertedAmount += coin;
        return true;
    }

    public void Cancel()
    {
        if (InsertedAmount > 0)
        {
            Console.WriteLine($"Возврат внесённых средств: {InsertedAmount:F2}₽");
            InsertedAmount = 0;
        }
        else
        {
            Console.WriteLine("Возвращать нечего.");
        }
    }

    public void BuyProduct(int id)
    {
        var p = Products.FirstOrDefault(x => x.Id == id);
        if (p == null)
        {
            Console.WriteLine("Такого товара нет.");
            return;
        }

        if (p.Quantity <= 0)
        {
            Console.WriteLine("Товар закончился.");
            return;
        }

        if (InsertedAmount < p.Price)
        {
            Console.WriteLine($"Недостаточно средств. Нужно ещё {p.Price - InsertedAmount:F2}₽.");
            return;
        }

        p.Quantity--;
        InsertedAmount -= p.Price;
        EarnedMoney += p.Price;

        Console.WriteLine($"Вы получили {p.Name}. Спасибо за покупку!");

        if (InsertedAmount > 0)
        {
            Console.WriteLine($"Ваша сдача: {InsertedAmount:F2}₽");
            InsertedAmount = 0;
        }
    }

    public void RestockProduct(int id, int quantity)
    {
        var p = Products.FirstOrDefault(x => x.Id == id);
        if (p != null)
        {
            p.Quantity += quantity;
            Console.WriteLine($"Добавлено {quantity} шт. товара '{p.Name}'. Теперь на складе: {p.Quantity}.");
        }
        else
        {
            Console.WriteLine("Товар с таким id не найден.");
        }
    }

    public void SetPrice(int id, decimal newPrice)
    {
        var p = Products.FirstOrDefault(x => x.Id == id);
        if (p != null)
        {
            p.Price = newPrice;
            Console.WriteLine($"Новая цена на '{p.Name}': {newPrice:F2}₽");
        }
        else
        {
            Console.WriteLine("Товар с таким id не найден.");
        }
    }

    public void CollectMoney()
    {
        Console.WriteLine($"Выдана администратору выручка: {EarnedMoney:F2}₽.");
        EarnedMoney = 0;
    }
}
    