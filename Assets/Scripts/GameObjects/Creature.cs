using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System.Numerics;

namespace rogueLike.GameObjects
{
    public class Creature : GameObject
    {
        protected int moveCooldown = 30;
        protected int attackCooldown = 30;
        public int LastMovedFrame = 0;
        public int LastAttackFrame = 0;

        public int MoveCooldown { get => moveCooldown; }
        public int AttackCooldown { get => attackCooldown; }

        public Creature()
        {
            SetSymbol('?');
            Walkable = false;
        }

        internal void Move(Direction direct, World myWorld, float frameCount)
        {
            Vector2 movedPos;

            if (Vector2.FromDirection.TryGetValue(direct, out movedPos))
            {
                movedPos = GetPos() + Vector2.FromDirection[direct];
            }
            else
            {
                movedPos = Vector2.Zero;
            }

            TryToWalk(movedPos, myWorld);

            LastMovedFrame = (int)frameCount;
        }

        public void TryToWalk(Vector2 movedPos, World myWorld)
        {
            if (myWorld.IsPosWalkable(movedPos))
            {
                Creature entity = myWorld.GetGameObjectGrid()[Position.X, Position.Y] as Creature;
                myWorld.SetObject(GetPos(), myWorld.GetElementAt(GetPos()));
                SetPos(movedPos);

                if (entity != null)
                myWorld.SetObject(GetPos(), entity);
            }
        }

        internal virtual Vector2 Attack(Direction direct, World myWorld)
        {
            Vector2 attackPos;

            if(Vector2.FromDirection.TryGetValue(direct, out attackPos))
            {
                attackPos = GetPos() + Vector2.FromDirection[direct];
            }
            else
            {
                attackPos = Vector2.Zero;
            }

            TryToHit(attackPos, myWorld);

            return attackPos;
        }

        public void TryToHit(Vector2 attackPos, World myWorld)
        {
            GameObject objectAt = myWorld.GetGameObjectGrid()[attackPos.X, attackPos.Y];
            Creature attackedObj = objectAt as Creature;

            if (attackedObj != null)
            {
                attackedObj.Dead(myWorld);
            }
        }

        public void Dead(World myWorld)
        {
            myWorld.SetObject(Position, myWorld.GetElementAt(Position));
            SetSymbol(new char());
            SetPos(Vector2.Zero);
        }
    }
}
