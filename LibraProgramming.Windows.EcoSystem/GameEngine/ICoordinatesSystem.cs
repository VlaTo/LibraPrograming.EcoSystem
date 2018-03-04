using System.Numerics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface ICoordinatesSystem
    {
        Vector2 GetPoint(Position position);

        Position GetPosition(Vector2 point);
    }
}