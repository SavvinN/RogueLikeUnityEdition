using System;
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

        public override void EnemyAttackment(World myWorld, float frameCount)
        {
            var playerPos = myWorld.GetPlayer().Position;
            var attackPos = Position - playerPos;

            attackPos.X = attackPos.X != 0
                ? -1 * Math.Sign(attackPos.X)
                : 0;

            attackPos.Y = attackPos.Y != 0
                ? -1 * Math.Sign(attackPos.Y)
                : 0;

            var direct = Vector2.GetToDirection(attackPos);

            if (frameCount - LastFireFrame > AttackCooldown && direct != Direction.None)
            {
                var Arrow = new Arrow(Position, direct);
                myWorld.AddArrow(Arrow);
                LastFireFrame = (int)frameCount;
            }
        }
    }
}
