using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeUIManager : MonoBehaviour
{
    public Canvas canvas;
    private GameObject tradePanel;
    private GameObject content;
    public GameObject itemButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tradePanel = canvas.transform.Find("TradeUI").gameObject;
        content = tradePanel.transform.Find("Content").gameObject;
    }

    // Update is called once per frame
    public void UpdateItems(Dictionary<Item, int> buyList)
    {
        foreach(Item item in buyList.Keys)
        {
            GameObject p = Instantiate(itemButtonPrefab);
            p.transform.SetParent(content.transform);

            TextMeshPro name = p.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshPro>();
            name.text = item.name;

            //TODO : image
        }
    }

    public void OpenUI()
    {
        tradePanel.SetActive(true);
    }
}
