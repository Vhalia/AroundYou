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
            return enemyType switch
            {
                EEnemyType.WALKER => "walkerEnemy",
                EEnemyType.SHOOTER => "shooterEnemy",
                _ => throw new ArgumentException("Unknown enemy type"),
            };
        }
    }
}
