using Microsoft.Graphics.Canvas.UI;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEcoSystemController
    {
        /// <summary>
        /// 
        /// </summary>
        IScene Scene
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task InitializeAsync(CanvasCreateResourcesReason reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        void Update(TimeSpan elapsed);

        /// <summary>
        /// 
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 
        /// </summary>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        Vector2 GetPosition(Coordinates coordinates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Coordinates GetCoordinates(Vector2 position);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        CellType GetAttribute(Coordinates coordinates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="occupy"></param>
        void Occupy(Coordinates coordinates, bool occupy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        bool IsOccupied(Coordinates coordinates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="poisoned"></param>
        /// <returns></returns>
        bool Eat(Coordinates coordinates, out bool poisoned);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bot"></param>
        void DoBeetleBotDies(BeetleBot bot);

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EpochStartedEventArgs> EpochStarted;
    }
}
