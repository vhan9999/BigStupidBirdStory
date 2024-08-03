using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TradeUIManager : MonoBehaviour
{
    [SerializeField] private Trade trade;


    public Canvas canvas;
    private GameObject tradePanel;

    private GameObject content;//item scroll

    private GameObject preOrder;//pre-order ui
    private GameObject imageNum;
    private GameObject itemName;
    private GameObject preOrderNum;
    private GameObject totalPrice;
    private GameObject calculator;
    private GameObject preOrderDone;

    private string inputString = "0";

    public GameObject itemButtonPrefab;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tradePanel = canvas.transform.Find("TradeUI").gameObject;

        content = tradePanel.transform.Find("CustomPart/ItemScroll/Viewport/Content").gameObject;

        preOrder = tradePanel.transform.Find("PreOrder").gameObject;
        imageNum = preOrder.transform.Find("Image/ImageNum").gameObject;
        itemName = preOrder.transform.Find("name").gameObject;
        preOrderNum = preOrder.transform.Find("count").gameObject;
        totalPrice = preOrder.transform.Find("price").gameObject;
        calculator = preOrder.transform.Find("Calculator").gameObject;
        preOrderDone = preOrder.transform.Find("Pre-OrderButton").gameObject;
    }

    // Update is called once per frame
    public void UpdateItems(Dictionary<Item, int> buyList)
    {
        foreach(Transform item in content.GetComponentsInChildren<Transform>())
        {
            if(item == content.transform) continue;
            Destroy(item.gameObject);
        }
        foreach(Item item in buyList.Keys)
        {
            Debug.Log("aaa");
            GameObject itemPrefab = Instantiate(itemButtonPrefab, content.transform);
            Button button = itemPrefab.GetComponent<Button>();
            button.onClick.AddListener(delegate { OpenPreOrder(item, buyList[item]); });

            itemPrefab.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemName;

            //TODO : image
            if(buyList[item] < 0)
                itemPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "¡Û";
            else
                itemPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = buyList[item].ToString();
        }
    }

    public void OpenUI()
    {
        tradePanel.SetActive(true);
    }

    public void OpenPreOrder(Item item, int nowCount)
    {
        
        inputString = "0";
        preOrder.SetActive(true);
        if(nowCount < 0)
            imageNum.GetComponent<TextMeshProUGUI>().text = "¡Û";
        else
            imageNum.GetComponent<TextMeshProUGUI>().text = nowCount.ToString();
        itemName.GetComponent<TextMeshProUGUI>().text = item.itemName;
        preOrderNum.GetComponent<TextMeshProUGUI>().text = "Count : " + 0.ToString();
        totalPrice.GetComponent<TextMeshProUGUI>().text = "Total Price : " + 0.ToString();

        preOrderDone.GetComponent<Button>().onClick.AddListener( delegate { PreOrderDone(item); });
        foreach (Button button in calculator.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(delegate { SetPreOrderCount(button.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(), item.price); });
        }
    }

    public void SetPreOrderCount(TextMeshProUGUI tmp, int price)
    {
        string s = tmp.text;
        if (s == "¡Û")
        {
            inputString = "¡Û";
        }
        else if(s == "Reset")
        {
            inputString = "0";
        }
        else
        {
            if(inputString == "0")
            {
                inputString = s;
            }
            else if (inputString == "¡Û")
            {
                inputString = s;
            }
            else
            {
                inputString += s;
            }
        }
        preOrderNum.GetComponent<TextMeshProUGUI>().text = "Count : " + inputString;

        if (inputString == "¡Û")
            totalPrice.GetComponent<TextMeshProUGUI>().text = "Total Price : " + "¡Û";
        else
            totalPrice.GetComponent<TextMeshProUGUI>().text = "Total Price : " + (int.Parse(inputString) * price).ToString();

    }

    public void PreOrderDone(Item item)
    {
        int count;
        if (inputString == "¡Û")
            count = -1;
        else
            count = int.Parse(inputString);
        trade.PreOrder(item, count);
    }
}
