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
        _charaStorage = GetMockCharaStorage();
        var charaStorageView = canvas.transform.Find("Body/CharaStoragePanel/View/Viewport/Content").GameObject();
        var charaContainer = GameObject.Find("CharaContainer");
        var charaCardPrefab = Resources.Load<GameObject>("UI/CharaCard");
        var charaPrefab = Resources.Load<GameObject>("Chara/CharaPrefab");
        var tmp = 0;
        foreach (var data in _charaStorage)
        {
            {
                var go = Instantiate(charaCardPrefab, charaStorageView.transform);
                go.GetComponent<CharaCard>().SetName(data.name);
            }
            {
                var go = Instantiate(charaPrefab, charaContainer.transform);
                go.GetComponent<CharaBehaviour>().SetCharaData(data);
                go.transform.position = new Vector3(tmp, 0, 0);
                tmp += 2;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private List<CharaData> GetMockCharaStorage()
    {
        var c = new List<CharaData>();
        c.Add(new CharaData
        {
            name = "Chara1",
            battleData = new BattleData
            {
                atk = 10,
                movspd = 1,
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
        c.Add(new CharaData
        {
            name = "Chara2",
            battleData = new BattleData
            {
                atk = 20,
                movspd = 1,
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
        return c;
    }

    private UnityAction CharaStorageButtonClick()
    {
        return () => { _charaStoragePanel.SetActive(!_charaStoragePanel.activeSelf); };
    }
}