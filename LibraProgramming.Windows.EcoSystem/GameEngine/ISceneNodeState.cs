using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISceneNodeState
    {
        /// <summary>
        /// 
        /// </summary>
        void Leave();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void Enter(ISceneNode node);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        void Update(TimeSpan elapsed);
    }
}