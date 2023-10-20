using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolGeneric<T> where T : class
{
    private Dictionary<T, Item<T>> pooledItems = new Dictionary<T, Item<T>>();

    public T GetItemFromPool()
    {
        foreach (var pooledItem in pooledItems)
        {
            if (!pooledItem.Value.IsUsed)
            {
                pooledItem.Value.IsUsed = true;
                return pooledItem.Key;
            }
        }

        T item = CreateItem();

        Item<T> newItem = new Item<T>();
        newItem.IsUsed = true;
        newItem.ItemType = item; 
        pooledItems[item] = newItem;

        return item;
    }

    public void ReturnItem(T itemToReturn)
    {
        if (pooledItems.TryGetValue(itemToReturn, out var pooledItem))
        {
            pooledItem.IsUsed = false;
            Debug.Log("Item returned to the pool: " + itemToReturn);
        }
        else
        {
            Debug.Log("Item is not created yet");
        }
    }

    protected virtual T CreateItem()
    {
        // You can create a new item of the enum type here
        return default(T);
    }

    public void CleanupUnusedItems()
    {
        var unusedItems = new List<T>();
        foreach (var kvp in pooledItems)
        {
            if (!kvp.Value.IsUsed)
            {
                unusedItems.Add(kvp.Key);
            }
        }

        foreach (var item in unusedItems)
        {
            pooledItems.Remove(item);
        }
    }

    private class Item<U>
    {
        public bool IsUsed;
        public U ItemType; 
    }
}
