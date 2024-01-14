using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using System.Collections.Generic;

namespace rogueLike
{
    public class Game
    {
        public int level = 1,
                    life = 3;
        public float frameCount = 0;
        private readonly World myWorld;
        public bool GameStatus { get; set; }
        public World MyWorld { get => myWorld; }

        public Game()
        {
            myWorld = new World(level);
            GameStatus = true;
        }

        public void RegenerateLevel()
        {
            life--;
            myWorld.GenerateMaze(level);
            myWorld.GetAllArrows().Clear();
        }

        public bool IsGoal()
        {
            return IsPlayerDead() || PlayerOnExit();
        }

        public bool IsGameOver => GameStatus = life <= 0;

        public bool IsPlayerDead()
        {
            if (myWorld.GetPlayer().Position == Vector2.Zero)
            {
                RegenerateLevel();
                return true;
            }
            else
                return false;
        }

        public bool PlayerOnExit()
        {
            GameObject elementAtPlayerPos = myWorld.GetElementAt(myWorld.GetPlayer().Position);
            if (World.CompareObjects(elementAtPlayerPos, new ExitDoor()))
            {
                LevelUp();
                return true;
            }
            else
                return false;
        }

        public void LevelUp()
        {
            level++;
            myWorld.GenerateMaze(level);
        }

        public void HandleMoveInput(ConsoleKey key)
        {
            var currentPlayer = myWorld.GetPlayer();
            Dictionary<ConsoleKey, Direction> MoveDirections = new()
            {
                {ConsoleKey.UpArrow , Direction.Up},
                {ConsoleKey.DownArrow , Direction.Down},
                {ConsoleKey.RightArrow , Direction.Right},
                {ConsoleKey.LeftArrow , Direction.Left}
            };

            if (MoveDirections.TryGetValue(key, out _))
            {
                currentPlayer.Move(MoveDirections[key], myWorld, frameCount);
            }
        }

        public Vector2 HandleAttackInput(ConsoleKey key)
        {
            var currentPlayer = myWorld.GetPlayer();
            Vector2 attackPos;

            Dictionary<ConsoleKey, Direction> AttackDirections = new()
            {
                {ConsoleKey.W , Direction.Up},
                {ConsoleKey.S , Direction.Down},
                {ConsoleKey.D , Direction.Right},
                {ConsoleKey.A , Direction.Left}
            };

            if (AttackDirections.TryGetValue(key, out Direction value))
            {
                attackPos = currentPlayer.Attack(value, myWorld, frameCount);
            }
            else
            {
                attackPos = Vector2.Zero;
            }

            return attackPos;
        }

        public void HandleEnemyBehavior(Zombie[] zombs, Archer[] archs)
        {
            EnemyBehavior(zombs);
            EnemyBehavior(archs);
        }

        public void EnemyBehavior(Enemy[] enemys)
        {
            foreach (var enemy in enemys)
            {
                enemy.FindPlayer(myWorld.GetPlayer().Position, myWorld.GetGameObjectGrid());

                var PlayerDirect = enemy.GetDirectionToPlayer();

                if (PlayerDirect != Direction.None)
                {
                    enemy.EnemyMovement(myWorld, PlayerDirect, frameCount);
                    enemy.EnemyAttackment(myWorld, frameCount);
                }
                else
                    enemy.Patrol(myWorld, frameCount);
            }
        }

        public void HandleArrowsAction()
        {
            var arrows = myWorld.GetAllArrows();

            foreach (var arrow in arrows)
            {
                arrow.Move(myWorld, frameCount);
            }

            if (arrows is not null)
                for (int i = 0; i < arrows.Count; i++)
                {
                    if (arrows[i].Position == Vector2.Zero)
                        myWorld.RemoveArrow(arrows[i]);
                }
        }
    }
}


