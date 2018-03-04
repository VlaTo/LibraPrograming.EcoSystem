namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public struct MapSize
    {
        public int Columns
        {
            get;
        }

        public int Rows
        {
            get;
        }

        public MapSize(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }
    }
}