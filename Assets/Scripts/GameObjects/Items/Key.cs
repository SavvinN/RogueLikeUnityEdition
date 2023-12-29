using System;

using System.Numerics;


namespace rogueLike.GameObjects.Items
{
    internal class Key : Item
    {
        public Key(Vector2 SpawnPos) : base(SpawnPos)
        {
            SpawnChance = 5;
            SetPos(SpawnPos);
            SetSymbol('T');
        }

        public int Action() => IsPickUp ? 1 : 0;
    }
}
