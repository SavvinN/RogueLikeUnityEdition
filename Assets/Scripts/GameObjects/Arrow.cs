using rogueLike.GameObjects.MazeObjects;
using System.Numerics;


namespace rogueLike.GameObjects
{
    public class Arrow : GameObject
    {
        private Direction _direction;
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

        public void Move(World myWorld)
        {

            var movingPos = Vector2.Zero;

            switch (_direction)
            {
                case Direction.Up:
                    movingPos = (Position - Vector2.UnitX);
                    break;
                case Direction.Down:
                    movingPos = (Position + Vector2.UnitX);
                    break;
                case Direction.Right:
                    movingPos = (Position + Vector2.UnitY);
                    break;
                case Direction.Left:
                    movingPos = (Position - Vector2.UnitY);
                    break;
            }

            if (!World.CompareObjects(myWorld.GetElementAt(movingPos), new Wall())
                && !(_distance < 0))
            {
                SetPos(movingPos);
                TryToHit(GetPos(), myWorld);
                _distance--;
            }
            else
                RemoveArrow();
        }

        public void TryToHit(Vector2 attackPos, World myWorld)
        {
            GameObject objectAt = myWorld.GetGameObjectGrid()[attackPos.X, attackPos.Y];
            Creature attackedObj = objectAt as Creature;

            if (attackedObj != null && World.CompareObjects(attackedObj, myWorld.GetPlayer()))
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
