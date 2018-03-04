using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class StateAwareSceneNode<TNode> : SceneNode
        where TNode : class, ISceneNode
    {
        private ISceneNodeState state;

        public ISceneNodeState State
        {
            get => state;
            set
            {
                DoLeaveState();
                state = value;
                DoEnterState();
            }
        }

        protected StateAwareSceneNode()
        {
            State = NodeState.Empty<TNode>();
        }

        public override void Update(TimeSpan elapsed)
        {
            state?.Update(elapsed);
            base.Update(elapsed);
        }

        protected virtual void DoEnterState()
        {
            state?.Enter(this);
        }

        protected virtual void DoLeaveState()
        {
            state?.Leave();
        }
    }
}