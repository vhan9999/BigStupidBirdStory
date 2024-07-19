using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    //picture

    public static Item CreateInstance(string name, int price)
    {
        Item instance = CreateInstance<Item>();
        instance.itemName = name;
        instance.price = price;
        return instance;
    }
}
