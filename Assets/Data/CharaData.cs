using System;

namespace Player.save
{
    [Serializable]
    public struct CharaData
    {
        public string name;
        public BattleData battleData;
        public MaxableNumber energy;
        public MaxableNumber hp;
        public MaxableNumber hunger;
        public MaxableNumber mood;
    }
}