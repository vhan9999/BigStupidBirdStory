using System.Collections.Generic;
using DefaultNamespace;
using Player.save;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    private List<CharaData> _charaStorage;
    private GameObject _charaStoragePanel;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        var b = canvas.transform.Find("Body/CharaStorageButton").GetComponent<Button>();
        b.onClick.AddListener(CharaStorageButtonClick());
        _charaStoragePanel = canvas.transform.Find("Body/CharaStoragePanel").GameObject();
        _charaStorage = new List<CharaData>();
        _charaStorage.Add(new CharaData
        {
            name = "Chara1",
            battleData = new BattleData
            {
                atk = 10,
                def = 10,
                cri = 10,
                spd = 10,
                dodge = 10
            },
            energy = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hp = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hunger = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            mood = new MaxableNumber
            {
                now = 100,
                max = 100
            }
        });
        _charaStorage.Add(new CharaData
        {
            name = "Chara2",
            battleData = new BattleData
            {
                atk = 20,
                def = 20,
                cri = 20,
                spd = 20,
                dodge = 20
            },
            energy = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hp = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hunger = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            mood = new MaxableNumber
            {
                now = 100,
                max = 100
            }
        });

        var charaStorageView = canvas.transform.Find("Body/CharaStoragePanel/View/Viewport/Content").GameObject();
        var charaCardPrefab = Resources.Load<GameObject>("UI/CharaCard");
        foreach (var data in _charaStorage)
        {
            var go = Instantiate(charaCardPrefab, charaStorageView.transform);
            go.GetComponent<CharaCard>().SetName(data.name);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private UnityAction CharaStorageButtonClick()
    {
        return () => { _charaStoragePanel.SetActive(!_charaStoragePanel.activeSelf); };
    }
}