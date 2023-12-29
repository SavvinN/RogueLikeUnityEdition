using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace rogueLike.GameObjects.Items
{
    internal class Heart : Item
    {
        public Heart(Vector2 SpawnPos) : base(SpawnPos)
        {
            SpawnChance = 20;
            SetPos(SpawnPos);
            SetSymbol('♥');
        }

        public int Action() => IsPickUp ? 1 : 0;
    }
}
