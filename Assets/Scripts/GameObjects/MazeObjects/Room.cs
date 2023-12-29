

namespace rogueLike.GameObjects.MazeObjects
{
    internal class Room : GameObject
    {
        private const char symbol = '.';
        public Room()
        {
            Walkable = true;
            SetSymbol(symbol);
        }
    }
}