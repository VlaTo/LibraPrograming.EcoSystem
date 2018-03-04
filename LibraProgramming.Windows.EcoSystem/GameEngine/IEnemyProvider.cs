using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface IEnemyProvider
    {
        ICollection<Enemy> GetEnemies();
    }
}