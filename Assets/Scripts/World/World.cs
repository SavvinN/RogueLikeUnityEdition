using System.Numerics;
using System;
using System.Collections.Generic;
using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using rogueLike.MazeGenerator;
using Unity.VisualScripting;
namespace rogueLike
{
    public class World
    {
        private readonly Wall wall = new();
        private readonly Ground ground = new();
        private readonly Room room = new();
        private readonly ExitDoor exitDoor = new();
        private GameObject[,] _gameObjectsGrid;
        private GameObject[,] _gameObjGridCopy;
        private Player player;
        private Zombie[] zombie;
        private Archer[] archer;
        private readonly List<Arrow> arrow = new();
        private List<Vector2> _enemySpawnMap;
        private List<Vector2> _itemSpawnMap;
        private char[,] _grid;
        private String worldFrame = String.Empty;
        private int Rows = 0,
                    Cols = 0;
        private int sizeX = 20,
                    sizeY = 18;
        private int _level = 0,
                    enemyCount = 0;
        private Maze maze;
        readonly Random rnd = new();
        public World(int level)
        {
            GenerateMaze(level);
        }

        public void GenerateMaze(int level)
        {
            _level = level;
            enemyCount = _level;
            SetWorldSize();
            maze = new Maze(sizeY,
                            sizeX,
                            wall.GetSymbol(),
                            ground.GetSymbol(),
                            room.GetSymbol());

            _grid = maze.GetGrid();
            (Rows, Cols) = maze.GetSize();
            _gameObjectsGrid = new GameObject[Rows, Cols];
            _gameObjGridCopy = new GameObject[Rows, Cols];
            CreateExitDoor();
            worldFrame = CreateWorldFrame();
            ConstructObjectsGrid();
            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Cols; x++)
                {
                    _gameObjGridCopy[y, x] = _gameObjectsGrid[y, x];
                }
            _enemySpawnMap = GenerateEnemySpawnMap();
            _itemSpawnMap = GenerateItemSpawnMap();
            SpawnEnemy();
            SpawnPlayer();
        }

        internal void AddArrow(Arrow a) => arrow.Add(a);
        internal void RemoveArrow(Arrow a)
        {
            arrow.Remove(a);
        }

        internal List<Arrow> GetAllArrows() => arrow;

        private void ConstructObjectsGrid()
        {
            Dictionary<char, GameObject> symbolObjectDictionary = new()
            {
                { wall.GetSymbol(), wall},
                { ground.GetSymbol(), ground},
                { room.GetSymbol(), room},
                { exitDoor.GetSymbol(), exitDoor}
            };

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    SetObject(y, x, symbolObjectDictionary[_grid[y, x]].Clone());
                }
            }
        }

        internal void SetObject(int y, int x, GameObject obj)
        {
            _gameObjectsGrid[y, x] = obj;
            _gameObjectsGrid[y, x].SetPos(new Vector2(y, x));
        }

        internal void SetObject(Vector2 pos, GameObject obj)
        {
            int x = pos.Y;
            int y = pos.X;
            _gameObjectsGrid[y, x] = obj;
            _gameObjectsGrid[y, x].SetPos(new Vector2(y, x));
        }

        private void CreateExitDoor()
        {
            int random = rnd.Next(2, Cols - 2);
            random += (_grid[Rows - 2, random] == wall.GetSymbol()) ? -1 : 0;
            _grid[Rows - 1, random] = exitDoor.GetSymbol();
            SetObject(Rows - 1, random, exitDoor);
            exitDoor.SetPos(new Vector2(Rows - 1, random));
        }

        private void SpawnEnemy()
        {
            zombie = new Zombie[enemyCount];
            archer = new Archer[zombie.Length - enemyCount / 2];
            for (int i = 0; i < zombie.Length; i++)
            {
                zombie[i] = new Zombie(_enemySpawnMap[rnd.Next(0, _enemySpawnMap.Count - 1)]);
                _gameObjectsGrid[zombie[i].Position.X, zombie[i].Position.Y] = zombie[i];
            }
            for (int i = 0; i < archer.Length; i++)
            {
                archer[i] = new Archer(_enemySpawnMap[rnd.Next(0, _enemySpawnMap.Count - 1)]);
                _gameObjectsGrid[archer[i].Position.X, archer[i].Position.Y] = archer[i];
            }
        }

        private void SpawnPlayer()
        {
            if (exitDoor.Position.Y > Cols / 2)
            {
                player = _gameObjectsGrid[1, 1] == wall
                       ? new Player(new Vector2(2, 1))
                       : new Player(new Vector2(1, 1));
            }
            else
            {
                player = _gameObjectsGrid[1, Cols - 2] == wall
                       ? new Player(new Vector2(1, Cols - 3))
                       : new Player(new Vector2(1, Cols - 2));
            }

            SetObject(player.Position.X, player.Position.Y, player);
        }

        public List<Vector2> GenerateItemSpawnMap()
        {
            List<Vector2> spawnMap = new();
            Vector2[] Dir = new Vector2[4]
            {
                -Vector2.UnitX,
                Vector2.UnitX,
                -Vector2.UnitY,
                Vector2.UnitY
            };

            int wallCount = 0;
            for (int y = Cols / 2; y < Cols - 2; y++)
            {
                for (int x = 2; y < Rows - 2; y++)
                {
                    foreach(var d in Dir)
                    {
                        if (CompareObjects(_gameObjectsGrid[y + d.X, x + d.Y], wall))
                            wallCount++;
                    }

                    if (wallCount == 3 && CompareObjects(_gameObjectsGrid[y, x], wall))
                    {
                        spawnMap.Add(new Vector2(y, x));
                    }
                    wallCount = 0;
                }
            }
            return spawnMap;
        }

        private List<Vector2> GenerateEnemySpawnMap()
        {
            List<Vector2> spawnMap = new();
            for (int y = 2; y < Rows - 1; y++)
            {
                for (int x = 2; x < Cols - 1; x++)
                {
                    if (CompareObjects(_gameObjectsGrid[y, x], room))
                    {
                        spawnMap.Add(new Vector2(y, x));
                    }
                }
            }
            return spawnMap;
        }

        internal GameObject[,] GetGameObjectGrid() => _gameObjectsGrid;

        public String GetFrame() => worldFrame;

        private void SetWorldSize()
        {
            int defaultSizeX = 20;
            int defaultSizeY = 18;
            sizeX = _level * 2 + defaultSizeX;
            sizeY = defaultSizeY + _level;
        }

        private string CreateWorldFrame()
        {
            string frame = "";
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    frame += _grid[y, x];
                }
                frame += "\n";
            }
            return frame;
        }

        internal static bool CompareObjects(GameObject obj1, GameObject obj2) => obj1.GetType().Equals(obj2.GetType());
        public bool IsPosWalkable(Vector2 pos) =>
            (pos.Y >= 0 && pos.X >= 0) 
            && (pos.Y < sizeX && pos.X < sizeY)
            && _gameObjectsGrid[pos.X, pos.Y].IsWalkable;

        internal GameObject GetElementAt(Vector2 Pos) => _gameObjGridCopy[Pos.X, Pos.Y];
        internal List<Creature> GetAllCreatures()
        {
            var list = new List<Creature>
            {
                player
            };

            foreach (var z in zombie)
            {
                list.Add(z);
            }

            foreach (var a in archer)
            {
                list.Add(a);
            }

            return list;
        }
        internal Player GetPlayer() => player;
        internal Zombie[] GetZombies() => zombie;
        internal Archer[] GetArchers() => archer;
    }

}
