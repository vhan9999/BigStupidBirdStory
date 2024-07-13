using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManage : MonoBehaviour
{
    public static List<CharaBehaviour> CharaList = new List<CharaBehaviour>();

    void Start()
    {
        UpdateList();
    }

    private void UpdateList()
    {
        
        if(transform.childCount != CharaList.Count)
        {
            CharaList.Clear();
            foreach (Transform child in transform)
            {
                CharaList.Add(child.GetComponent<CharaBehaviour>());
            }
        }
    }
}
