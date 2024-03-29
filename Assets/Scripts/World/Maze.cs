﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rogueLike.GameObjects;

namespace rogueLike.MazeGenerator
{
    internal class Maze
    {
        private readonly char _wall;
        private readonly char _ground;
        private readonly char _room;
        private readonly char[,] _grid;

        // Размеры комнат
        private int rheight = 7;
        private int rwidth = 5;
        private readonly int roomsPercentage = 2;
        private readonly Random rnd = new();
        private readonly int _sizeX = 20;
        private readonly int _sizeY = 20;

        public Maze(int sizeX, int sizeY, char wall, char ground, char room)
        {
            _wall = wall;
            _ground = ground;
            _room = room;
            _sizeX = ToOdd(sizeX);
            _sizeY = ToOdd(sizeY);
            _grid = new char[_sizeX, _sizeY];
            GenerateMaze(_sizeY, _sizeX, RoomsNumberSelection());
        }

        public (int, int) GetSize() => (_sizeX, _sizeY);

        public char[,] GetGrid() => _grid;

        public int RoomsNumberSelection() => _sizeX * _sizeY / (rheight * rwidth) / roomsPercentage;

        public int ToOdd(int number) => number % 2 == 0
                                        ? number - 1
                                        : number;

        public bool DeadEnd(int x, int y, int height, int width)
        {
            int a = 0;
            if (x != 1 && x - 2 >= 0)
            {
                if (_grid[y, x - 2] == _ground)
                    a += 1;
            }
            else a += 1;

            if (y != 1 && y - 2 >= 0)
            {
                if (_grid[y - 2, x] == _ground)
                    a += 1;
            }
            else a += 1;

            if (x != width - 2 && x + 2 < width)
            {
                if (_grid[y, x + 2] == _ground)
                    a += 1;
            }
            else a += 1;

            if (y != height - 2 && y + 2 < height)
            {
                if (_grid[y + 2, x] == _ground)
                    a += 1;
            }
            else a += 1;

            if (a == 4)
                return true;
            else
                return false;
        }

        public void GenerateBase(int height, int width)
        {
            for (int i = 0; i < height; i++) 
                for (int j = 0; j < width; j++)
                {
                    _grid[i, j] = _wall;
                }

        }

        public void DigTunnels(int height, int width)
        {
            int x = 3, y = 3, a = 0; 
            while (a < 10000)
            { 
                _grid[y, x] = _ground; a++;
                while (true)
                { 
                    int c = rnd.Next() % 4; 
                    switch (c)
                    {  
                        case 0:
                            if (y != 1 && y - 2 >= 0)
                                if (_grid[y - 2, x] == _wall)
                                { 
                                    _grid[y - 1, x] = _ground;
                                    _grid[y - 2, x] = _ground;
                                    y -= 2;
                                }
                            break;
                        case 1:
                            if (y != height - 2 && y + 2 < height)
                                if (_grid[y + 2, x] == _wall)
                                { 
                                    _grid[y + 1, x] = _ground;
                                    _grid[y + 2, x] = _ground;
                                    y += 2;
                                }
                            break;
                        case 2:
                            if (x != 1 && x - 2 >= 0)
                                if (_grid[y, x - 2] == _wall)
                                { 
                                    _grid[y, x - 1] = _ground;
                                    _grid[y, x - 2] = _ground;
                                    x -= 2;
                                }
                            break;
                        case 3:
                            if (x != width - 2 && x + 2 < width)
                                if (_grid[y, x + 2] == _wall)
                                { 
                                    _grid[y, x + 1] = _ground;
                                    _grid[y, x + 2] = _ground;
                                    x += 2;
                                }
                            break;
                    }
                    if (DeadEnd(x, y, height, width))
                        break;
                }
                if (DeadEnd(x, y, height, width))
                { 
                    do
                    {
                        x = 2 * (rnd.Next() % ((width - 1) / 2)) + 1;
                        y = 2 * (rnd.Next() % ((height - 1) / 2)) + 1;
                    }
                    while (_grid[y, x] != _ground);
                }
            }
        }

        public void GenerateRooms(int height, int width, int k)
        {
            int x, y;
            bool b, swap = true;
            for (int l = 0; l < k; l++)
            {
                b = true;
                while (b)
                {
                    do
                    { 
                        if (rwidth % 4 == 0)
                            x = 2 * (rnd.Next() % (width / 2)) + 1; 
                        else
                            x = 2 * (rnd.Next() % (width / 2)) + 2;
                        if (rheight % 4 == 0)
                            y = 2 * (rnd.Next() % (height / 2)) + 1;
                        else
                            y = 2 * (rnd.Next() % (height / 2)) + 2;
                    }
                    while (x < rwidth + 2 || x > width - rwidth - 2 ||
                                y < rheight + 2 || y > height - rheight - 2);
                    b = false;
                    for (int i = y - rheight - 2; i < y + rheight + 2; i++)
                        for (int j = x - rwidth - 2; j < x + rwidth + 2; j++)
                            if (_grid[i, j] == _room)
                                b = false;

                    if (b)
                        continue;

                    DigRoom(y, x, swap);
                }
            }
        }

        public void DigRoom(int y, int x, bool swap)
        {
            for (int i = y - rheight / 2; i < y + rheight / 2 + 1; i++) 
                for (int j = x - rwidth / 2; j < x + rwidth / 2 + 1; j++)
                    _grid[i, j] = _room;
            if (rnd.Next() % 4 == 0) _grid[y + rheight / 2 + 1, x - rwidth / 2 + 2 * (rnd.Next() % (rwidth / 2 + 1))] = _room;
            if (rnd.Next() % 4 == 1) _grid[y - rheight / 2 - 1, x - rwidth / 2 + 2 * (rnd.Next() % (rwidth / 2 + 1))] = _room;
            if (rnd.Next() % 4 == 2) _grid[y - rheight / 2 + 2 * (rnd.Next() % (rheight / 2 + 1)), x + rwidth / 2 + 1] = _room;
            if (rnd.Next() % 4 == 3) _grid[y - rheight / 2 + 2 * (rnd.Next() % (rheight / 2 + 1)), x - rwidth / 2 - 1] = _room;
            RotateRoom(swap);
        }

        public void RotateRoom(bool swap)
        {
            if (swap)
            {
                rheight += rwidth;
                rwidth = rheight - rwidth;
                rheight -= rwidth;
            } 
        }

        public void GenerateMaze(int height, int width, int k)
        {
            rheight--;

            rwidth--;

            GenerateBase(width, height);

            DigTunnels(width, height);

            GenerateRooms(width, height, k);

        }

    }
}
