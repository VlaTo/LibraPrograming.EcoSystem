using System;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnemyWaveFactory
    {
        [PrefferedConstructor]
        public EnemyWaveFactory()
        {
        }

        public Enemy CreateEnemy(EnemyWaveEmitter emitter, int waveNumber)
        {
            throw new NotImplementedException();

            /*var origin = emitter.GetSpawnPoint();
            var enemy = waveNumber > 3
                ? new FastMovementEnemy(origin, 80.0f, emitter.Waypoints, 0.88f, 17.0d)
                : new Enemy(origin, 140.0f, emitter.Waypoints, 0.33f, 24.0d);

            enemy.CreateResources(emitter.Creator, CanvasCreateResourcesReason.FirstTime);

            return enemy;*/
        }
    }
}