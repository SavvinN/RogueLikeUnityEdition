using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using System.Collections.Generic;
using static System.Console;

namespace rogueLike
{
    public class Game
    {
        System.Numerics.Vector2 position;

        public int level = 1,
                    life = 3;
        public float frameCount = 0;
        private World myWorld;
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
            GameObject elementAtPlayerPos = myWorld.GetElementAt(myWorld.GetPlayer().GetPos());
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

            if (frameCount - currentPlayer.LastMovedFrame > currentPlayer.MoveCooldown)
            {
                var temp = currentPlayer.Position;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        currentPlayer.Move(Direction.Up, myWorld, frameCount);
                        break;
                    case ConsoleKey.DownArrow:
                        currentPlayer.Move(Direction.Down, myWorld, frameCount);
                        break;
                    case ConsoleKey.RightArrow:
                        currentPlayer.Move(Direction.Right, myWorld, frameCount);
                        break;
                    case ConsoleKey.LeftArrow:
                        currentPlayer.Move(Direction.Left, myWorld, frameCount);
                        break;
                }

                if (temp != currentPlayer.Position)
                    currentPlayer.LastMovedFrame = (int)frameCount;
            }
        }

        public Vector2 HandleAttackInput(ConsoleKey key)
        {
            var currentPlayer = myWorld.GetPlayer();
            Vector2 attackPos = Vector2.Zero;

            if (frameCount - currentPlayer.LastAttackFrame > currentPlayer.AttackCooldown)
            {
                switch (key)
                {
                    case ConsoleKey.W:
                        attackPos = currentPlayer.Attack(Direction.Up, myWorld);
                        break;
                    case ConsoleKey.S:
                        attackPos = currentPlayer.Attack(Direction.Down, myWorld);
                        break;
                    case ConsoleKey.D:
                        attackPos = currentPlayer.Attack(Direction.Right, myWorld);
                        break;
                    case ConsoleKey.A:
                        attackPos = currentPlayer.Attack(Direction.Left, myWorld);
                        break;
                }
                currentPlayer.LastAttackFrame = (int)frameCount;
            }
            return attackPos;
        }

        public void HandleEnemyAction(Zombie[] zombs, Archer[] archs)
        {
            foreach (var zombie in zombs)
            {
                zombie.FindPlayer(myWorld.GetPlayer().Position, myWorld.GetGameObjectGrid());
                EnemyMovement(zombie);
                MeleeEnemyAttackment(zombie);
            }

            foreach (var archer in archs)
            {
                archer.FindPlayer(myWorld.GetPlayer().Position, myWorld.GetGameObjectGrid());
                if(!(myWorld.GetPlayer().Position == archer.Position))
                EnemyMovement(archer);

                if(EnemyObsPlayer(archer) != Direction.None)
                RangeEnemyAttackment(archer);
            }
        }

        public void HandleArrowsAction()
        {
            var arrows = myWorld.GetAllArrows();

            foreach (var arrow in arrows)
            {
                if (arrow.Position != Vector2.Zero && frameCount - arrow.LastMovedFrame > arrow.velocity)
                {
                    arrow.Move(myWorld);
                    arrow.LastMovedFrame = (int)frameCount;
                }
            }

            if (arrows is not null)
                for (int i = 0; i < arrows.Count; i++)
                {
                    if (arrows[i].Position == Vector2.Zero)
                        myWorld.RemoveArrow(arrows[i]);
                }
        }

        public Direction EnemyObsPlayer(Enemy enemy)
        {
            var nextStep = enemy.GetNextStep(myWorld.GetPlayer().Position) - enemy.Position;

            if(Vector2.ToDirection.TryGetValue(nextStep, out Direction direction))
            {
                return direction;
            }
            else
            {
                enemy.Patrol(myWorld, frameCount);
                return Direction.None;
            }
        }

        public void EnemyMovement(Enemy enemy)
        {
            var direct = EnemyObsPlayer(enemy);

            if (frameCount - enemy.LastMovedFrame > enemy.MoveCooldown)
            {
                enemy.Move(direct, myWorld, frameCount);  
            }
        }


        private void RangeEnemyAttackment(Archer archer)
        {
            var direct = Direction.None;
            var playerPos = myWorld.GetPlayer().Position;
            
            if(archer.Position.X > playerPos.X && archer.Position.Y == playerPos.Y)
                direct = Direction.Up;
            else
            if (archer.Position.X < playerPos.X && archer.Position.Y == playerPos.Y)
                direct = Direction.Down;
            else
            if (archer.Position.Y < playerPos.Y && archer.Position.X == playerPos.X)
                direct = Direction.Right;
            else
            if (archer.Position.Y > playerPos.Y && archer.Position.X == playerPos.X)
                direct = Direction.Left;

            if (frameCount - archer.LastAttackFrame > archer.AttackCooldown)
            {
                var Arrow = new Arrow(archer.GetPos(), direct);
                myWorld.AddArrow(Arrow);
                archer.LastAttackFrame = (int)frameCount;
            }
        }

        public void MeleeEnemyAttackment(Enemy enemy)
        {
            var playerPos = myWorld.GetPlayer().Position;
            var attackPos = Vector2.Zero;

            if (frameCount - enemy.LastMovedFrame > enemy.AttackCooldown)
            {
                foreach (var direct in Vector2.ToDirection)
                {
                    if (enemy.Position + direct.Key == playerPos)
                        attackPos = enemy.Attack(direct.Value, myWorld);
                }

                enemy.LastMovedFrame = (int)frameCount;
            }
        }
    }
}


