using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public static class NodeState
    {
        public static ISceneNodeState Empty<TNode>()
            where TNode : class, ISceneNode
        {
            return new EmptySceneState<TNode>();
        }

        /// <summary>
        /// 
        /// </summary>
        private class EmptySceneState<TNode> : SceneNodeState<TNode>
            where TNode : class, ISceneNode
        {
            public override void Update(TimeSpan elapsed)
            {
            }
        }
    }
}