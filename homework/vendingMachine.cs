using System;
using System.Collections.Generic;

namespace VendingMachineApp;

public class vendingMachine
{
    public List<product> Products { get; } = new();
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
            int maxId = 0;
            foreach (var p in Products)
            {
                if (p.Id > maxId)
                    maxId = p.Id;
            }
            id = maxId + 1;
        }

        Products.Add(new product { Id = id, Name = name, Price = price, Quantity = quantity });
    }

    public bool InsertCoin(decimal coin)
    {
        InsertedAmount += coin;
        return true;
    }

    public decimal Cancel()
    {
        if (InsertedAmount > 0)
        {
            decimal returnAmount = InsertedAmount;
            InsertedAmount = 0;
            return returnAmount;
        }

        return 0;
    }

    public decimal BuyProduct(int id)
    {
        product p = null;
        foreach (var prod in Products)
        {
            if (prod.Id == id)
            {
                p = prod;
                break;
            }
        }

        if (p == null)
        {
            return -1m;
        }

        if (p.Quantity <= 0)
        {
            return -2m;
        }

        if (InsertedAmount < p.Price)
        {
            var missing = p.Price - InsertedAmount;
            return -missing;
        }

        p.Quantity--;
        EarnedMoney += p.Price;

        var change = InsertedAmount - p.Price;
        InsertedAmount = 0;

        if (change < 0)
            return 0;
        else
            return change;
    }

    public bool RestockProduct(int id, int quantity)
    {
        product p = null;
        foreach (var prod in Products)
        {
            if (prod.Id == id)
            {
                p = prod;
                break;
            }
        }

        if (p == null)
            return false;

        p.Quantity += quantity;
        return true;
    }

    public bool SetPrice(int id, decimal newPrice)
    {
        product p = null;
        foreach (var prod in Products)
        {
            if (prod.Id == id)
            {
                p = prod;
                break;
            }
        }

        if (p == null)
            return false;

        p.Price = newPrice;
        return true;
    }

    public decimal CollectMoney()
    {
        var money = EarnedMoney;
        EarnedMoney = 0;
        return money;
    }
}
