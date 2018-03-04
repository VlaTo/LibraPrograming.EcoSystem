using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public partial class SceneNode : ISceneNode
    {
        private ISceneNode parent;
        
        public ISceneNode Parent
        {
            get => parent;
            set
            {
                if (value == parent)
                {
                    return;
                }

                if (null != parent)
                {
                    DoParentRemoved();
                }

                parent = value;

                if (null != parent)
                {
                    DoParentAdded();
                }
            }
        }

        ICollection<ISceneNode> ISceneNode.Children => Children;

        protected SceneNodeCollection Children
        {
            get;
        }

        public virtual GameplayController Controller => parent?.Controller;

        protected SceneNode()
        {
            Children = new SceneNodeCollection(this);
        }

        public virtual Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            var tasks = new List<Task>();

            foreach (ISceneNode child in Children)
            {
                tasks.Add(child.CreateResourcesAsync(creator, reason));
            }

            return Task.WhenAll(tasks);
        }

        public virtual void Draw(CanvasDrawingSession session)
        {
            foreach (ISceneNode child in Children)
            {
                child.Draw(session);
            }
        }

        public virtual void Update(TimeSpan elapsed)
        {
            foreach (ISceneNode child in Children)
            {
                child.Update(elapsed);
            }
        }

        protected virtual void DoParentAdded()
        {
        }

        protected virtual void DoParentRemoved()
        {
        }

        protected virtual void DoChildAdded(ISceneNode child)
        {
        }

        protected virtual void DoChildRemoved(ISceneNode child)
        {
        }
    }
}