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

            if(AttackDirections.TryGetValue(key, out Direction value))
            {
                currentPlayer.Attack(value, myWorld, frameCount);
                attackPos = currentPlayer.Position + Vector2.FromDirection[value];
            }
            else
            {
                attackPos = Vector2.Zero;
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
                if (!(myWorld.GetPlayer().Position == archer.Position))
                    EnemyMovement(archer);

                if (EnemyObsPlayer(archer) != Direction.None)
                    RangeEnemyAttackment(archer);
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

        public Direction EnemyObsPlayer(Enemy enemy) => Vector2.GetToDirection(enemy.GetNextStep() - enemy.Position);

        public void EnemyMovement(Enemy enemy) => enemy.Move(EnemyObsPlayer(enemy), myWorld, frameCount);

        private void RangeEnemyAttackment(Archer archer)
        {
            var direct = Direction.None;
            var playerPos = myWorld.GetPlayer().Position;

            if (archer.Position.X > playerPos.X && archer.Position.Y == playerPos.Y)
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

            if (frameCount - archer.LastFireFrame > archer.AttackCooldown)
            {
                var Arrow = new Arrow(archer.Position, direct);
                myWorld.AddArrow(Arrow);
                archer.LastFireFrame = (int)frameCount;
            }
        }

        public void MeleeEnemyAttackment(Enemy enemy)
        {
            var playerPos = myWorld.GetPlayer().Position;

            foreach (var direct in Vector2.ToDirection)
            {
                if (enemy.Position + direct.Key == playerPos)
                    enemy.Attack(direct.Value, myWorld, frameCount);
            }
        }
    }
}


