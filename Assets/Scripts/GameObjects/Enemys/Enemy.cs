using rogueLike.GameObjects.MazeObjects;
using System.Collections.Generic;
using System;
using System.Linq;

namespace rogueLike.GameObjects.Enemys
{
    public class Enemy : Creature
    {
        private int viewDistance = 6;
        private readonly int PatrolCooldown = 100;
        public bool seeThePlayer = true;
        private List<Vector2> path = new();

        public Enemy()
        {
            Walkable = false;
        }

        protected void SetViewDistance(int dist) => viewDistance = dist;

        public void Patrol(World myWorld, float frameCount)
        {
            Random rand = new();
            int randomDirection = rand.Next(0, 4);

            if(frameCount - LastActionFrame > PatrolCooldown)
            Move((Direction)randomDirection, myWorld, frameCount);
        }

        public void FindPlayer(Vector2 playerPosition, GameObject[,] grid)
        {
            if (Vector2.Distance(Position, playerPosition) < (float)viewDistance)
            {
                List<Vector2> sawPath = GetPathTo(playerPosition);

                foreach (var pos in sawPath)
                {
                    seeThePlayer = !World.CompareObjects(grid[pos.X, pos.Y], new Wall()); 
                    if(!seeThePlayer) break;
                }

                if (seeThePlayer)
                    path = GetPathTo(playerPosition);         
            }
        }

        private List<Vector2> GetPathTo(Vector2 playerPosition)
        {
            Vector2 viewPoint = Position;
            List<Vector2> path = new()
            {
                viewPoint
            };

            while (viewPoint != playerPosition)
            {
                float temp = viewPoint.X;
                    viewPoint.X += (viewPoint.X > playerPosition.X)
                                    ? -1
                                    : (viewPoint.X < playerPosition.X)
                                        ? 1
                                        : 0;

                if (temp == viewPoint.X)
                    viewPoint.Y += (viewPoint.Y > playerPosition.Y)
                                    ? -1
                                    : (viewPoint.Y < playerPosition.Y)
                                        ? 1
                                        : 0;
                path.Add(viewPoint);
            }
            return path;
        }

        public Vector2 GetNextStep()
        {
            if (path.Count > 1)
            {
                var nStep = path[1];
                path.Remove(path.First());
                return nStep;
            }
            else
                return Position;
        }

        public void EnemyMovement(World myWorld, Direction ObsedPlayerDirect, float frameCount) => Move(ObsedPlayerDirect, myWorld, frameCount);

        public Direction GetDirectionToPlayer() => Vector2.GetToDirection(GetNextStep() - Position);

        public virtual void EnemyAttackment(World myWorld, float frameCount)
        {
            throw new NotImplementedException();
        }

    }

}
