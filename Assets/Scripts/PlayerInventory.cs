using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        instance = this;
    }

    public void Add(string tag)
    {
        if (!items.ContainsKey(tag))
            items[tag] = 0;

        items[tag]++;
    }

    public bool Has(string tag, int amount)
    {
        return items.ContainsKey(tag) && items[tag] >= amount;
    }

    public void Remove(string tag, int amount)
    {
        if (Has(tag, amount))
            items[tag] -= amount;
    }

    // 🔥 อันนี้แหละที่ขาด
    public int Get(string tag)
    {
        if (items.ContainsKey(tag))
            return items[tag];

        return 0;
    }
}