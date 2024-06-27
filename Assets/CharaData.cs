using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct MaxableNumber
{
    public float now;
    public float max;
}

[Serializable]
struct BattleData
{
    public float atk;
    public float def;
    public float cri;
    public float spd;
    public float dodge;
}

[Serializable]
struct CharaData
{
    public string name;
    public BattleData battleData;
    public MaxableNumber hp;
    public MaxableNumber hunger;
    public MaxableNumber mood;
    public MaxableNumber energy;
}
