using AroundYou.Models.Enums;
using Godot;
using System.Collections.Generic;

namespace AroundYou.Models
{
    public class Drop
    {
        public PackedScene PackedScene { get; set; }
        public ERarity Rarity { get; set; }
        public float ChanceToDrop { get; set; }
        public float PickupRange { get; set; }
    }
}
