using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System.Numerics;

namespace rogueLike.GameObjects
{
    public class Creature : GameObject
    {
        protected int moveCooldown = 30;
        protected int attackCooldown = 30;
        public int LastActionFrame = 0;
        public int MoveCooldown { get => moveCooldown; }
        public int AttackCooldown { get => attackCooldown; }

        public Creature()
        {
            SetSymbol('?');
            Walkable = false;
        }

        internal void Move(Direction direct, World myWorld, float frameCount)
        {
            if (frameCount - LastActionFrame > MoveCooldown)
            {
                var movedPos = Position + Vector2.GetFromDirection(direct);
                TryToWalk(movedPos, myWorld, frameCount);
            }
        }

        public void TryToWalk(Vector2 movedPos, World myWorld, float frameCount)
        {
            if (myWorld.IsPosWalkable(movedPos) && myWorld.GetGameObjectGrid()[Position.X, Position.Y] is Creature entity)
            {
                myWorld.SetObject(Position, myWorld.GetElementAt(Position));
                SetPos(movedPos);
                LastActionFrame = (int)frameCount;
                myWorld.SetObject(Position, entity);
            }
        }

        internal virtual Vector2 Attack(Direction direct, World myWorld, float frameCount)
        {
            Vector2 attackPos;

            attackPos = Position + Vector2.FromDirection[direct];

            if (!World.CompareObjects(myWorld.GetElementAt(attackPos), new Wall())
                && frameCount - LastActionFrame > AttackCooldown)
            {
                LastActionFrame = (int)frameCount;
                TryToHit(attackPos, myWorld);
                return attackPos;
            }
            else
                return Vector2.Zero;
        }

        public void TryToHit(Vector2 attackPos, World myWorld)
        {
            GameObject objectAt = myWorld.GetGameObjectGrid()[attackPos.X, attackPos.Y];

            if (objectAt is Creature attackedObj)
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
