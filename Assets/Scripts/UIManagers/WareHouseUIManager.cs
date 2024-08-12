using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WareHouseUIManage : MonoBehaviour
{
    public Canvas canvas;
    private GameObject itemPanel;

    private GameObject content;//item scroll

    public GameObject itemButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        itemPanel = canvas.transform.Find("WareHouseUI").gameObject;

        content = itemPanel.transform.Find("CustomPart/ItemScroll/Viewport/Content").gameObject;
    }

    public void UpdateItems(Dictionary<Item, int> itemList)
    {
        foreach (Transform item in content.GetComponentsInChildren<Transform>())
        {
            if (item == content.transform) continue;
            Destroy(item.gameObject);
        }
        foreach (Item item in itemList.Keys)
        {
            Debug.Log("aaa");
            GameObject itemPrefab = Instantiate(itemButtonPrefab, content.transform);
            Button button = itemPrefab.GetComponent<Button>();
            //button.onClick.AddListener(delegate { OpenPreOrder(item, buyList[item]); });

            itemPrefab.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.itemName;

            //TODO : image
            itemPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = itemList[item].ToString();
        }
    }

    public void OpenUI()
    {
        itemPanel.SetActive(true);
    }
}
