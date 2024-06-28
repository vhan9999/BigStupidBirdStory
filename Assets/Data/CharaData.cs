using System;

namespace Player.save
{
    [Serializable]
    internal struct CharaData
    {
        public string name;
        public BattleData battleData;
        public MaxableNumber energy;
        public MaxableNumber hp;
        public MaxableNumber hunger;
        public MaxableNumber mood;
    }
}