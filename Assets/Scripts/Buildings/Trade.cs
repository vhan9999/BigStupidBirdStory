using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Trade : BuildingOBJ
{
    private List<CharaBehaviour> charaList = new List<CharaBehaviour>();
    private Dictionary<Item,int> buyList = new Dictionary<Item, int>();//int -> player want to buy
    void Start()
    {
        AddItem1();
        InvokeRepeating("Trading", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CharaIn(CharaBehaviour chara)
    {
        charaList.Add(chara);
        chara.gameObject.SetActive(false);
    }
    void CharaOut(CharaBehaviour chara)
    {
        charaList.Remove(chara);
        chara.gameObject.SetActive(true);
    }
    void Trading()
    {
        foreach(CharaBehaviour chara in charaList)
        {
            bool isTradeThisTime = false;
            foreach (Item item in chara.bag.Keys)
            {
                if (buyList[item] > 0)//trade success
                {
                    isTradeThisTime = true;
                    int tradeNum;

                    if (chara.bag[item] >= buyList[item]) tradeNum = buyList[item];
                    else tradeNum = chara.bag[item];

                    buyList[item] -= tradeNum;
                    chara.bag[item] -= tradeNum;

                    if(chara.bag[item] <= 0) chara.bag.Remove(item);

                    break; // to extend the trade time, every one sell a item per sec
                }
            }
            if (!isTradeThisTime) CharaOut(chara);
        }
    }

    void AddItem1()//add items in item list
    {
        buyList.Add(Item.CreateInstance("item1", 10), 0);
        buyList.Add(Item.CreateInstance("item1", 27), 0);
        buyList.Add(Item.CreateInstance("item1", 3), 0);
    }
}
