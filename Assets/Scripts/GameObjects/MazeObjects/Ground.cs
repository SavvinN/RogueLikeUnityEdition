

namespace rogueLike.GameObjects.MazeObjects
{
    internal class Ground : GameObject
    {
        private const char symbol = ' ';
        public Ground()
        {
            Walkable = true;
            SetSymbol(symbol);
        }
    }
}