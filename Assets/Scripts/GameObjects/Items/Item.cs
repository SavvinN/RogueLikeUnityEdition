
using System.Numerics;

namespace rogueLike.GameObjects.Items
{
    internal abstract class Item : GameObject
    {
        protected int SpawnChance { get; set; }
        public bool IsPickUp { get; set; }

        public int GetSpawnChance() => SpawnChance;
        public Item(Vector2 SpawnPos)
        {
            Walkable = true;
            IsPickUp = false;
        }
    }
}
