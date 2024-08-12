using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WareHouse : BuildingOBJ
{
    private Dictionary<Item, int> ItemList = new Dictionary<Item, int>();
    [SerializeField] private WareHouseUIManage wareHouseUIManage;
    // Start is called before the first frame update
    void Start()
    {
        AddItem1();
    }

    // Update is called once per frame
    public override void Click()
    {
        wareHouseUIManage.OpenUI();
        wareHouseUIManage.UpdateItems(ItemList);
    }
    private void AddItem(Item item, int num)
    {
        Item buyitem = ItemList.Keys.FirstOrDefault(ware => item.itemName == ware.itemName);
        if (buyitem == null) {
            ItemList.Add(item, num);
        }
        else
        {
            ItemList[buyitem] += num;
        }
    }

    private void AddItem1()//add items in item list
    {
        ItemList.Add(Item.CreateInstance("item1", 10), 0);
        ItemList.Add(Item.CreateInstance("item2", 27), 0);
        ItemList.Add(Item.CreateInstance("item3", 3), 0);
    }
}
