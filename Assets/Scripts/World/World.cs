using System.Numerics;
using System;
using System.Collections.Generic;
using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using rogueLike.MazeGenerator;
namespace rogueLike
{
    public class World
    {
        private Wall wall = new();
        private Ground ground = new();
        private Room room = new();
        private ExitDoor exitDoor = new();
        private GameObject[,] _gameObjectsGrid;
        private GameObject[,] _gameObjGridCopy;
        private Player player;
        private Zombie[] zombie;
        private Archer[] archer;
        private List<Arrow> arrow = new List<Arrow>();
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
        Random rnd = new Random();
        public World(int level)
        {
            GenerateMaze(level);
        }

        public void GenerateMaze(int level)
        {
            _level = level;
            enemyCount = (_level / 2) + _level;
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
            constructObjectsGrid();
            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Cols; x++)
                {
                    _gameObjGridCopy[y,x] = _gameObjectsGrid[y, x];
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

        private void constructObjectsGrid()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    if (_grid[y, x] == wall.GetSymbol())
                    {
                        SetObject(y, x, new Wall());
                    }
                    if (_grid[y, x] == ground.GetSymbol())
                    {
                        SetObject(y, x, new Ground());
                    }
                    if (_grid[y, x] == room.GetSymbol())
                    {
                        SetObject(y, x, new Room());
                    }
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

        internal GameObject GetObjectAt(Vector2 pos) => _gameObjGridCopy[pos.X, pos.Y];
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
            if(_enemySpawnMap.Count != 0)
            {
                zombie = new Zombie[enemyCount];
                archer = new Archer[enemyCount];
                for (int i = 0; i < zombie.Length; i++)
                {
                    zombie[i] = new Zombie(_enemySpawnMap[rnd.Next(0, _enemySpawnMap.Count - 1)]);
                    _gameObjectsGrid[zombie[i].GetPos().X, zombie[i].GetPos().Y] = zombie[i];
                }
                for (int i = 0; i < archer.Length; i++)
                {
                    archer[i] = new Archer(_enemySpawnMap[rnd.Next(0, _enemySpawnMap.Count - 1)]);
                    _gameObjectsGrid[archer[i].GetPos().X, archer[i].GetPos().Y] = archer[i];
                }
            }
        }

        private void SpawnPlayer()
        {
            if(exitDoor.GetPos().Y > Cols/2)
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
            List<Vector2> spawnMap = new List<Vector2>();
            int wallCount = 0;
            for (int y = Cols/2; y < Cols - 2; y++)
            {
                for (int x = 2; y < Rows - 2; y++)
                {
                    if (CompareObjects(_gameObjectsGrid[y - 1, x], wall))
                        wallCount++;
                    if (CompareObjects(_gameObjectsGrid[y + 1, x], wall))
                        wallCount++;
                    if (CompareObjects(_gameObjectsGrid[y , x - 1], wall))
                        wallCount++;
                    if (CompareObjects(_gameObjectsGrid[y, x + 1], wall))
                        wallCount++;
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
            List<Vector2> spawnMap = new List<Vector2>();
            for (int y = 2; y < Rows - 1; y++)
            {
                for (int x = 2; x < Cols - 1; x++)
                {
                    if (_gameObjectsGrid[y, x].GetSymbol() == room.GetSymbol())
                    {
                        spawnMap.Add(new Vector2(y, x));
                    }
                }
            }
            return spawnMap;
        }

        public char[,] GetCharGrid() => _grid;

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
        public bool IsPosWalkable(Vector2 pos) => _gameObjectsGrid[pos.X, pos.Y].IsWalkable;
        internal GameObject GetElementAt(Vector2 Pos) => _gameObjGridCopy[Pos.X, Pos.Y];
        internal GameObject GetObject(Vector2 pos) => _gameObjectsGrid[pos.X, pos.Y];
        internal List<Creature> GetAllCreatures()
        {
            var list = new List<Creature>();
            list.Add(player);
            foreach(var z in zombie)
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
