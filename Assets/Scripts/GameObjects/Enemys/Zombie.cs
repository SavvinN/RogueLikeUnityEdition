
using System.Numerics;

namespace rogueLike.GameObjects.Enemys
{
    public class Zombie : Enemy
    {
        public Zombie(Vector2 spawnPos) : base()
        {
            SetSymbol('Z');
            SetPos(spawnPos);
            SetViewDistance(6);
            attackCooldown = 40;
        }

        public override void EnemyAttackment(World myWorld, float frameCount)
        {
            var playerPos = myWorld.GetPlayer().Position;

            foreach (var direct in Vector2.ToDirection)
            {
                if (Position + direct.Key == playerPos)
                    Attack(direct.Value, myWorld, frameCount);
            }
        }

    }
}
