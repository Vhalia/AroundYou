using Godot;

namespace AroundYou.Scripts.Singleton
{
    public partial class EventsBus : Node
    {
        [Signal]
        public delegate void BulletsInMagazineChangedEventHandler(int bulletsCount, int maxBullets);
        [Signal]
        public delegate void PlayerHealthChangedEventHandler(float amount);

    }
}
