using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface IPathFinder
    {
        IReadOnlyCollection<Position> GetWaypoints(Position position);
    }
}