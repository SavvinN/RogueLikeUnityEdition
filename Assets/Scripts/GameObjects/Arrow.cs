using rogueLike.GameObjects.MazeObjects;
using System.Numerics;


namespace rogueLike.GameObjects
{
    public class Arrow : GameObject
    {
        private readonly Direction _direction = Direction.None;
        private int _distance = 5;
        public int LastMovedFrame = 0;
        public int velocity = 30;

        internal Arrow(Vector2 pos, Direction direction)
        {
            Walkable = true;
            SetSymbol('+');
            SetPos(pos);
            _direction = direction;
        }

        public void Move(World myWorld, float frameCount)
        {
            if (frameCount - LastMovedFrame > velocity)
            {
                var movingPos = Vector2.GetFromDirection(_direction);
                movingPos = Position + movingPos;
                LastMovedFrame = (int)frameCount;

                if (!World.CompareObjects(myWorld.GetElementAt(movingPos), new Wall())
                && !(_distance < 0))
                {
                    SetPos(movingPos);
                    TryToHit(Position, myWorld);
                    _distance--;
                }
                else
                    RemoveArrow();
            }
        }

        public void TryToHit(Vector2 attackPos, World myWorld)
        {
            GameObject objectAt = myWorld.GetGameObjectGrid()[attackPos.X, attackPos.Y];

            if (objectAt is Creature attackedObj && World.CompareObjects(attackedObj, myWorld.GetPlayer()))
            {
                attackedObj.Dead(myWorld);
            }
        }

        public void RemoveArrow()
        {
            SetSymbol(new char());
            SetPos(Vector2.Zero);
        }
    }
}
