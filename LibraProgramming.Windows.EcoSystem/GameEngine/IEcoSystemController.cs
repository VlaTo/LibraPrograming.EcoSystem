using Microsoft.Graphics.Canvas.UI;
using System;
using System.Threading.Tasks;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public interface IEcoSystemController
    {
        IScene Scene
        {
            get;
            set;
        }

        Task InitializeAsync(CanvasCreateResourcesReason reason);

        void Update(TimeSpan elapsed);

        void Shutdown();
    }
}
