using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public abstract class SceneNodeState<TNode> : ISceneNodeState
        where TNode : class, ISceneNode 
    {
        protected TNode Node
        {
            get;
            private set;
        }

        public void Leave()
        {
            OnLeave();
            Node = null;
        }

        public void Enter(ISceneNode node)
        {
            Node = (TNode) node;
            OnEnter();
        }

        public abstract void Update(TimeSpan elapsed);

        protected virtual void OnLeave()
        {
        }

        protected virtual void OnEnter()
        {
        }
    }
}