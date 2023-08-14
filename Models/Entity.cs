using AroundYou.Models.Enums;
using Godot;

namespace AroundYou.Models
{
    public class Entity
    {
        public string Name { get; set; }
        public EEnemyType Type { get; set; }
        public int Cost { get; set; }
        public int MaxSpawnCount { get; set; }
        public PackedScene Scene { get; set; }
        public Entity()
        {
        }

        public Entity(string name, EEnemyType type, int cost, int maxSpawnCount, PackedScene scene)
        {
            Name = name;
            Type = type;
            Cost = cost;
            MaxSpawnCount = maxSpawnCount;
            Scene = scene;
        }
    }
}
