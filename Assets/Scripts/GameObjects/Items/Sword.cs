using System.Numerics;

namespace rogueLike.GameObjects.Items
{
    internal class Sword : Item
    {
        public Sword(Vector2 SpawnPos) : base(SpawnPos)
        {
            SpawnChance = 10;
            SetPos(SpawnPos);
            SetSymbol('!');
        }

        public int Action() => IsPickUp ? 30 : 0;
    }
}
