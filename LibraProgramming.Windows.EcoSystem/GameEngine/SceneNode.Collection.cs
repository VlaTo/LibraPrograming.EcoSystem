using System;
using System.Collections;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public partial class SceneNode
    {
        /// <summary>
        /// 
        /// </summary>
        public sealed class SceneNodeCollection : CollectionBase, ICollection<ISceneNode>
        {
            private readonly SceneNode owner;

            public bool IsReadOnly => InnerList.IsReadOnly;

            public SceneNodeCollection(SceneNode owner)
            {
                this.owner = owner;
            }

            public new IEnumerator GetEnumerator()
            {
                return new ImmutableEnumerator(InnerList);
            }

            IEnumerator<ISceneNode> IEnumerable<ISceneNode>.GetEnumerator()
            {
                return new ImmutableEnumerator(InnerList);
            }

            public void Add(ISceneNode node)
            {
                if (null == node)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                InnerList.Add(node);
                node.Parent = owner;

                owner.DoChildAdded(node);
            }

            public bool Contains(ISceneNode node)
            {
                if (null == node)
                {
                    return false;
                }

                return InnerList.Contains(node);
            }

            public void CopyTo(ISceneNode[] array, int arrayIndex)
            {
                if (null == array)
                {
                    throw new ArgumentNullException(nameof(array));
                }

                InnerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(ISceneNode node)
            {
                if (null == node)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                var index = InnerList.IndexOf(node);

                if (0 > index)
                {
                    return false;
                }

                InnerList.RemoveAt(index);
                node.Parent = null;

                owner.DoChildRemoved(node);

                return true;
            }

            /// <summary>
            /// 
            /// </summary>
            private class ImmutableEnumerator : IEnumerator<ISceneNode>
            {
                private const int NoIndex = -1;

                private readonly ISceneNode[] collection;
                private bool disposed;
                private int index;

                public ISceneNode Current
                {
                    get
                    {
                        EnsureCurrentState();

                        if (NoIndex == index || index >= collection.Length)
                        {
                            throw new InvalidOperationException();
                        }

                        return collection[index];
                    }
                }

                object IEnumerator.Current => Current;

                public ImmutableEnumerator(ArrayList source)
                {
                    this.collection = CreateArray(source);
                    index = NoIndex;
                }

                public bool MoveNext()
                {
                    EnsureCurrentState();

                    if (NoIndex == index)
                    {
                        if (0 >= collection.Length)
                        {
                            return false;
                        }

                        index = 0;
                    }
                    else
                    {
                        var newIndex = index + 1;

                        if (newIndex >= collection.Length)
                        {
                            return false;
                        }

                        index = newIndex;
                    }

                    return true;
                }

                public void Reset()
                {
                    EnsureCurrentState();
                    index = NoIndex;
                }

                public void Dispose()
                {
                    Dispose(true);
                }

                private static ISceneNode[] CreateArray(ArrayList source)
                {
                    var array = new ISceneNode[source.Count];
                    source.CopyTo(array, 0);
                    return array;
                }

                private void Dispose(bool dispose)
                {
                    if (disposed)
                    {
                        return;
                    }

                    try
                    {
                        if (dispose)
                        {

                        }
                    }
                    finally
                    {
                        disposed = true;
                    }
                }

                private void EnsureCurrentState()
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(String.Empty);
                    }
                }
            }
        }
    }
}