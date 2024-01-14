using rogueLike.GameObjects.MazeObjects;
using System.Numerics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace rogueLike.GameObjects.Enemys
{
    public class Enemy : Creature
    {
        private int viewDistance = 6;
        private Vector2 lastPlayerPos;
        private bool seeThePlayer = true;
        public List<Vector2> path = new();

        public Enemy()
        {
            Walkable = false;
        }

        protected void SetViewDistance(int dist) => viewDistance = dist;

        public void Patrol(World myWorld, float frameCount)
        {
            Random rand = new();
            int randomDirection = rand.Next(0, 4);
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

                    if(seeThePlayer) break;
                }
                path = seeThePlayer ? GetPathTo(playerPosition) : path;
            }
        }

        private List<Vector2> GetPathTo(Vector2 playerPosition)
        {
            List<Vector2> path = new();
            Vector2 viewPoint = Position;
            path.Add(viewPoint);
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

    }

}
