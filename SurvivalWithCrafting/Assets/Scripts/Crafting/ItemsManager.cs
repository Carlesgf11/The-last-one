using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    static public List<ItemsProperties> items = new List<ItemsProperties>()
    {
        new ItemsProperties()
        {
            id = 0, name = "Metal", craftAmountExport = 0, imgId = 0, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 1, name = "Metal Bar", craftAmountExport = 0, imgId = 1, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 2, name = "Scarp", craftAmountExport = 0, imgId = 2, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 3, name = "Cloth", craftAmountExport = 0, imgId = 3, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 4, name = "String", craftAmountExport = 0, imgId = 4, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 5, name = "Gunpowder", craftAmountExport = 0, imgId = 5, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 6, name = "Bottle", craftAmountExport = 0, imgId = 6, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 7, name = "Chip", craftAmountExport = 0, imgId = 7, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 8, name = "Battery", craftAmountExport = 0, imgId = 8, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 9, name = "Gasoline", craftAmountExport = 0, imgId = 9, crafting = new List<CraftingProperties>()
        },
        new ItemsProperties()
        {
            id = 10, name = "Bullet", craftAmountExport = 5, imgId = 10, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 1, amount = 1 },
                new CraftingProperties{ id_Item = 5, amount = 1 },
            },
        },
        new ItemsProperties()
        {
            id = 11, name = "Bendages", craftAmountExport = 1, imgId = 11, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 3, amount = 1 },
                new CraftingProperties{ id_Item = 4, amount = 1 },
            },
        },
        new ItemsProperties()
        {
            id = 12, name = "Life best", craftAmountExport = 1, imgId = 12, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 0, amount = 3 },
                new CraftingProperties{ id_Item = 2, amount = 2 },
                new CraftingProperties{ id_Item = 3, amount = 2 },
                new CraftingProperties{ id_Item = 4, amount = 4 },
            },
        },
        new ItemsProperties()
        {
            id = 13, name = "Gun", craftAmountExport = 1, imgId = 13, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 1, amount = 2 },
                new CraftingProperties{ id_Item = 0, amount = 4 },
                new CraftingProperties{ id_Item = 2, amount = 3 },
            },
        },
        new ItemsProperties()
        {
            id = 14, name = "Climbing gloves", craftAmountExport = 14, imgId = 14, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 7, amount = 1 },
                new CraftingProperties{ id_Item = 3, amount = 1 },
                new CraftingProperties{ id_Item = 8, amount = 1 },
            },
        },
        new ItemsProperties()
        {
            id = 15, name = "Molotov", craftAmountExport = 1, imgId = 15, crafting = new List<CraftingProperties>()
            {
                new CraftingProperties{ id_Item = 6, amount = 1 },
                new CraftingProperties{ id_Item = 9, amount = 1 },
                new CraftingProperties{ id_Item = 3, amount = 1 },
            },
        },
    };

    static public ItemsProperties ColoneItem(ItemsProperties _item)
    {
        ItemsProperties newItem = new ItemsProperties()
        {
            id = _item.id,
            name = _item.name,
            craftAmountExport = _item.craftAmountExport,
            crafting = _item.crafting,
        };
        return newItem;
    }

    static public ItemsProperties GetItemById(int _id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].id == _id)
            {
                return ColoneItem(items[i]);
            }
        }
        return null;
    }
    static public string GetNameById(int _id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].id == _id)
            {
                return items[i].name;
            }
        }
        return "";
    }
}

[System.Serializable]
public class ItemsProperties
{
    public int id;
    public string name;
    public int imgId;
    public List<CraftingProperties> crafting;
    public int craftAmountExport;
}
[System.Serializable]
public class CraftingProperties
{
    public int id_Item;
    public int amount;
}
