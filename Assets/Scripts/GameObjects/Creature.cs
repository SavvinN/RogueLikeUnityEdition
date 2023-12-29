using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System.Numerics;

namespace rogueLike.GameObjects
{
    public class Creature : GameObject
    {
        protected int moveCooldown = 20;
        protected int attackCooldown = 30;

        public int MoveCooldown { get => moveCooldown; }
        public int AttackCooldown { get => attackCooldown; }
        public int lastActionFrame { get; set; }
        public Creature()
        {
            lastActionFrame = 0;
            SetSymbol('?');
            Walkable = false;
        }
        internal void Move(Direction direct, World myWorld)
        {
                Vector2 movedPos = Vector2.Zero;

                switch (direct)
                {
                    case Direction.Up:
                        movedPos = new Vector2(GetPos().X - 1, GetPos().Y);
                        break;
                    case Direction.Down:
                        movedPos = new Vector2(GetPos().X + 1, GetPos().Y);
                        break;
                    case Direction.Left:
                        movedPos = new Vector2(GetPos().X, GetPos().Y - 1);
                        break;
                    case Direction.Right:
                        movedPos = new Vector2(GetPos().X, GetPos().Y + 1);
                        break;
                }

                if (myWorld.IsPosWalkable(movedPos))
                {
                    Creature entity = myWorld.GetGameObjectGrid()[(int)Position.X, (int)Position.Y] as Creature;
                    myWorld.SetObject(GetPos(), myWorld.GetElementAt(GetPos()));
                    SetPos(movedPos);

                    if(entity != null)
                    myWorld.SetObject(GetPos(), entity);
                }     
        }

        internal virtual Vector2 Attack(Direction direct, GameObject[,] world)
        {
            Vector2 attackPos = Vector2.Zero;
            switch(direct)
            {
                case Direction.Left:
                    attackPos = GetPos() + -Vector2.UnitY;
                    break;
                case Direction.Right:
                    attackPos = GetPos() + Vector2.UnitY;
                    break;
                case Direction.Up:
                    attackPos = GetPos() + -Vector2.UnitX;
                    break;
                case Direction.Down:
                    attackPos = GetPos() + Vector2.UnitX;
                    break;
            }
            if (world[(int)attackPos.X, (int)attackPos.Y].GetType() != new Wall().GetType())
            return attackPos;
            else
            return Vector2.Zero;
        }

        public void Dead(World myWorld)
        {
            myWorld.SetObject(Position, myWorld.GetElementAt(Position));
            SetSymbol(new char());
            SetPos(Vector2.Zero);
        }
    }
}
