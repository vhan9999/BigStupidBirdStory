using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    private GameObject _charaStoragePanel;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        var b = canvas.transform.Find("Body/CharaStorageButton").GetComponent<Button>();
        b.onClick.AddListener(CharaStorageButtonClick());
        _charaStoragePanel = canvas.transform.Find("Body/CharaStoragePanel").GameObject();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private UnityAction CharaStorageButtonClick()
    {
        return () =>
        {
            _charaStoragePanel.SetActive(!_charaStoragePanel.activeSelf);
        };
    }
}