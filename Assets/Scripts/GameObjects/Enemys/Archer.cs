using System.Numerics;

namespace rogueLike.GameObjects.Enemys
{
    public class Archer : Enemy
    {
        public int LastFireFrame;

        public Archer(Vector2 spawnPos) : base()
        {
            LastActionFrame = 0;
            moveCooldown = 100;
            attackCooldown = 150;
            SetPos(spawnPos);
            SetSymbol('A');
            SetViewDistance(8);
        }
    }
}
