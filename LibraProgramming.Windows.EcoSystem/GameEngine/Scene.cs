using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using System.Numerics;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        private IEcoSystemController controller;

        private Vector2 size;

        /// <summary>
        /// 
        /// </summary>
        public IEcoSystemController Controller
        {
            get
            {
                return controller;
            }
            private set
            {
                if (null != controller)
                {
                    controller.Scene = null;
                }

                controller = value;

                if (null != controller)
                {
                    controller.Scene = this;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [PrefferedConstructor]
        public Scene(IEcoSystemController controller, Vector2 size)
        {
            this.size = size;
            Controller = controller;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}