using Player.save;
using System;

namespace Enemy.save
{
    [Serializable]
    public struct EnemyData
    {
        public string name;
        public BattleData battleData;
        public MaxableNumber hp;
    }
}