using System.Numerics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface ICoordinatesSystem
    {
        Vector2 GetPoint(Coordinates position);

        Coordinates GetPosition(Vector2 point);
    }
}