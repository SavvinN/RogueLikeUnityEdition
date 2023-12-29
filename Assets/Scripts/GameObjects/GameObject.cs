using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace rogueLike.GameObjects
{
    public abstract class GameObject
    {
        private char _symbol;

        private Vector2 _position;
        protected bool Walkable { get; set; }
        public bool IsWalkable { get => Walkable; }
        public Vector2 Position { get => _position; }
        public Vector2 GetPos() => _position;
        public void SetPos(Vector2 pos) => _position = pos;
        public char GetSymbol() => _symbol;
        protected void SetSymbol(char chr) => _symbol = chr;
    }
}
