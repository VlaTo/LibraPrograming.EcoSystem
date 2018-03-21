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
        ISubject<BeetleBotMessage> BeetleBotMessage
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ILand Land
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
    }
}
