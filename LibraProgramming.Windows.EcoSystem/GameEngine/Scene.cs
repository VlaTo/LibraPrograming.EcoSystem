using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        /// <summary>
        /// 
        /// </summary>
        public override IEcoSystemController Controller
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        [PrefferedConstructor]
        public Scene(IEcoSystemController controller)
        {
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