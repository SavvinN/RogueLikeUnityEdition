using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using static System.Console;

namespace rogueLike
{
    public class Game
    {
        System.Numerics.Vector2 position;
        
        public int  level = 1,
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

        public void Start()
        {
            
        }

        public void HandleEntityAction()
        {
            HandleArrowsAction();
            HandlePlayerInput(myWorld.GetPlayer());
            HandleEnemyAction(myWorld.GetZombies(), myWorld.GetArchers());
        }

        public void HandleArrowsAction()
        {
            var arrows = myWorld.GetAllArrows();

            foreach (var arrow in arrows)
            {
                if (arrow.Position != Vector2.Zero && frameCount - arrow.LastMovedFrame > arrow.velocity)
                {
                    arrow.Move();
                    arrow.LastMovedFrame = (int)frameCount;
                }
                TryToHit(arrow.Position, myWorld ,myWorld.GetPlayer());
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

            if(life <= 0)
            {
                GameStatus = false;
            }
                
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
                HandleMoveInput(key);
                HandleAttackInput(key);
            }
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

                currentPlayer.LastAttackFrame = (int)frameCount;
                foreach (var enemy in myWorld.GetAllCreatures())
                {
                    Game.TryToHit(attackPos, myWorld, enemy);
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

            if (frameCount - enemy.LastMovedFrame > enemy.MoveCooldown && direct != Direction.None)
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
                    case Direction.None:
                        enemy.Patrol(myWorld); 
                        break;
                }

                if (temp != enemy.Position)
                    enemy.LastMovedFrame = (int)frameCount;
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

            if (frameCount - archer.LastAttackFrame > archer.AttackCooldown && direct != Direction.None)
            {
                var Arrow = new Arrow(archer.GetPos(), direct, myWorld.GetGameObjectGrid());
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

                enemy.LastMovedFrame = (int)frameCount;
                if (attackPos != Vector2.Zero)
                    TryToHit(attackPos, myWorld, myWorld.GetPlayer());
            }
        }

        public static void TryToHit(Vector2 attackPos, World myWorld, Creature creature)
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


