namespace rogueLike.GameObjects.MazeObjects
{
    internal class Wall : GameObject
    {
        private const char symbol = '█';
        public Wall()
        {
            Walkable = false;
            SetSymbol(symbol);
        }
    }
}
