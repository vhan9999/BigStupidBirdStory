namespace Player.save
{
    public enum MonsterType
    {
        WOLF
    }

    public struct MonsterData
    {
        public MonsterType type;
        public BattleData battleData;
        public MaxableNumber hp;
    }
}