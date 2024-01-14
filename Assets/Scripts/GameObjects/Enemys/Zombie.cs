
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



    }
}
