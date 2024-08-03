using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Trade : BuildingOBJ
{
    private List<CharaBehaviour> charaList = new List<CharaBehaviour>();
    private Dictionary<Item,int> buyList = new Dictionary<Item, int>();//int -> player want to buy
    [SerializeField] private TradeUIManager tradeUIManager;
    void Start()
    {
        AddItem1();
        InvokeRepeating("Trading", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Click()
    {
        tradeUIManager.OpenUI();
        tradeUIManager.UpdateItems(buyList);
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

                    if (buyList[item] < 0) tradeNum = chara.bag[item];//infinite
                    if (chara.bag[item] >= buyList[item]) tradeNum = buyList[item];
                    else tradeNum = chara.bag[item];

                    buyList[item] -= tradeNum;
                    chara.bag[item] -= tradeNum;

                    if(chara.bag[item] <= 0) chara.bag.Remove(item);

                    break; // to extend the trade time, everyone sell a kind of item per sec
                }
            }
            if (!isTradeThisTime) CharaOut(chara);
        }
    }

    void AddItem1()//add items in item list
    {
        buyList.Add(Item.CreateInstance("item1", 10), 0);
        buyList.Add(Item.CreateInstance("item2", 27), 0);
        buyList.Add(Item.CreateInstance("item3", 3), 0);
    }

    public void PreOrder(Item item, int num)
    {
        buyList[item] = num;
        tradeUIManager.UpdateItems(buyList);
    }
}
