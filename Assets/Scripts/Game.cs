using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using static System.Console;

namespace rogueLike
{
    internal class Game
    {
        System.Numerics.Vector2 position;
        
        public int  level = 1,
                    life = 3;
        public float FrameCount = 0;
        private World myWorld;
        public bool GameStatus { get; set; }
        public World MyWorld { get { return myWorld; } }
        public Game()
        {
            myWorld = new World(level);
        }

        public void Start()
        {
            GameStatus = true;
        }

        public void HandleEntityAction()
        {
            HandlePlayerInput(myWorld.GetPlayer());
            HandleEnemyAction(myWorld.GetZombies(), myWorld.GetArchers());
            HandleArrowsAction();
        }

        public void HandleArrowsAction()
        {
            var arrows = myWorld.GetAllArrows();


            foreach (var arrow in arrows)
            {
                if (arrow.Position != Vector2.Zero && FrameCount - arrow.LastMovedFrame > arrow.velocity)
                {
                    arrow.Move();
                    arrow.LastMovedFrame = (int)FrameCount;
                }
                TryToHit(arrow.Position, myWorld.GetPlayer());
            }

            if (arrows is not null)
                for (int i = 0; i < arrows.Count; i++)
                {
                    if (arrows[i].Position == Vector2.Zero)
                        myWorld.RemoveArrow(arrows[i]);
                }
        }

        public void RegenerateLevel()
        {
            life--;
            myWorld.GenerateMaze(level);
            myWorld.GetAllArrows().Clear(); 
            Start();
        }

        public bool IsGoal()
        {
            GameObject elementAtPlayerPos = myWorld.GetElementAt(myWorld.GetPlayer().GetPos());

            if(life < 0)
                level = 1;

            if (myWorld.GetPlayer().Position == Vector2.Zero)
            {
                RegenerateLevel();
                return true;
            }

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
            Start();
        }

        public void HandlePlayerInput(Player currentPlayer)
        {
            while (KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                ConsoleKey key = keyInfo.Key;
                HandleMoveInput(currentPlayer, key);
                HandleAttackInput(currentPlayer, key);
            }
        }

        public void HandleMoveInput(Player currentPlayer, ConsoleKey key)
        {
            if (FrameCount - currentPlayer.LastMovedFrame > currentPlayer.MoveCooldown)
            {
                var temp = currentPlayer.Position;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        currentPlayer.Move(Direction.Up, myWorld);
                        break;
                    case ConsoleKey.DownArrow:
                        currentPlayer.Move(Direction.Down, myWorld);
                        break;
                    case ConsoleKey.RightArrow:
                        currentPlayer.Move(Direction.Right, myWorld);
                        break;
                    case ConsoleKey.LeftArrow:
                        currentPlayer.Move(Direction.Left, myWorld);
                        break;
                }

                if (temp != currentPlayer.Position)
                    currentPlayer.LastMovedFrame = (int)FrameCount;
            }
        }

        public Vector2 HandleAttackInput(Player currentPlayer, ConsoleKey key)
        {
            Vector2 attackPos = Vector2.Zero;
            if (FrameCount - currentPlayer.LastAttackFrame > currentPlayer.AttackCooldown)
            {
                switch (key)
                {
                    case ConsoleKey.W:
                        attackPos = currentPlayer.Attack(Direction.Up, myWorld.GetGameObjectGrid());
                        break;
                    case ConsoleKey.S:
                        attackPos = currentPlayer.Attack(Direction.Down, myWorld.GetGameObjectGrid());
                        break;
                    case ConsoleKey.D:
                        attackPos = currentPlayer.Attack(Direction.Right, myWorld.GetGameObjectGrid());
                        break;
                    case ConsoleKey.A:
                        attackPos = currentPlayer.Attack(Direction.Left, myWorld.GetGameObjectGrid());
                        break;
                }

                currentPlayer.LastAttackFrame = (int)FrameCount;
                foreach(var enemy in myWorld.GetAllCreatures())
                {
                    TryToHit(attackPos, enemy);
                }
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
                if(!(myWorld.GetPlayer().Position.X == archer.Position.X)
                    && !(myWorld.GetPlayer().Position.Y == archer.Position.Y))
                EnemyMovement(archer);

                if(EnemyObsPlayer(archer) != Direction.None)
                RangeEnemyAttackment(archer);
            }
        }

        public Direction EnemyObsPlayer(Enemy enemy)
        {
            var nextStep = enemy.GetNextStep(myWorld.GetPlayer().Position) - enemy.Position;

            if (nextStep == Vector2.UnitX)
                return Direction.Down;
            else
            if (nextStep == -Vector2.UnitX)
                return Direction.Up;
            else
            if (nextStep == Vector2.UnitY)
                return Direction.Right;
            else
            if (nextStep == -Vector2.UnitY)
                return Direction.Left;
            else
                return Direction.None;
        }

        public void EnemyMovement(Enemy enemy)
        {
            var direct = EnemyObsPlayer(enemy);

            if (FrameCount - enemy.LastMovedFrame > enemy.MoveCooldown && direct != Direction.None)
            {
                var temp = enemy.Position;
                switch (direct)
                {
                    case Direction.Left:
                        enemy.Move(direct, myWorld);
                        break;
                    case Direction.Right:
                        enemy.Move(direct, myWorld);
                        break;
                    case Direction.Down:
                        enemy.Move(direct, myWorld);
                        break;
                    case Direction.Up:
                        enemy.Move(direct, myWorld);
                        break;
                }

                if (temp != enemy.Position)
                    enemy.LastMovedFrame = (int)FrameCount;
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

            if (FrameCount - archer.LastAttackFrame > archer.AttackCooldown && direct != Direction.None)
            {
                var Arrow = new Arrow(archer.GetPos(), direct, myWorld.GetGameObjectGrid());
                myWorld.AddArrow(Arrow);
                archer.LastAttackFrame = (int)FrameCount;
            }
        }

        public void MeleeEnemyAttackment(Enemy enemy)
        {
            var playerPos = myWorld.GetPlayer().Position;
            var attackPos = Vector2.Zero;
            if (FrameCount - enemy.LastMovedFrame > enemy.AttackCooldown)
            {
                if (enemy.Position + Vector2.UnitX == playerPos)
                    attackPos = enemy.Attack(Direction.Down, myWorld.GetGameObjectGrid());
                else
                if (enemy.Position - Vector2.UnitX == playerPos)
                    attackPos = enemy.Attack(Direction.Up, myWorld.GetGameObjectGrid());
                else
                if (enemy.Position + Vector2.UnitY == playerPos)
                    attackPos = enemy.Attack(Direction.Right, myWorld.GetGameObjectGrid());
                else
                if (enemy.Position - Vector2.UnitY == playerPos)
                    attackPos = enemy.Attack(Direction.Left, myWorld.GetGameObjectGrid());

                enemy.LastMovedFrame = (int)FrameCount;
                if (attackPos != Vector2.Zero)
                    TryToHit(attackPos, myWorld.GetPlayer());
            }
        }

        public void TryToHit(Vector2 attackPos, Creature creature)
        {
            GameObject objectAt = myWorld.GetGameObjectGrid()[attackPos.X, attackPos.Y];
            Creature attackedObj = objectAt as Creature;

            if (attackedObj != null && attackedObj == creature)
            {
                attackedObj.Dead(myWorld);
            }
        }

    }
}


