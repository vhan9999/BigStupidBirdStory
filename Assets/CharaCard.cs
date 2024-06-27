using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CharaCard : MonoBehaviour
    {
        public TMP_Text nameText;

        public void SetName(string name)
        {
            nameText.text = name;
        }
    }
}