using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        private GameplayController controller;

        public override GameplayController Controller => controller;

        /// <summary>
        /// 
        /// </summary>
        [PrefferedConstructor]
        public Scene()
        {
        }

        public void SetController(GameplayController value)
        {
            controller = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}