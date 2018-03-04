using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface ISceneNode
    {
        ISceneNode Parent
        {
            get;
            set;
        }

        ICollection<ISceneNode> Children
        {
            get;
        }

        GameplayController Controller
        {
            get;
        }

        Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason);

        void Draw(CanvasDrawingSession session);

        void Update(TimeSpan elapsed);
    }
}