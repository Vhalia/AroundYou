using System;

namespace AroundYou.Models.Enums
{
    public enum EEnemyType
    {
        WALKER,
        SHOOTER
    }

    public static class EEnemyTypeExtensions
    {
        public static string ToKey(this EEnemyType enemyType)
        {
            switch(enemyType)
            {
                case EEnemyType.WALKER:
                    return "walkerEnemy";
                case EEnemyType.SHOOTER:
                    return "shooterEnemy";
                default:
                    throw new ArgumentException("Unknown enemy type");
            }
        }
    }
}
