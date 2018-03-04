using System;
using System.Reflection;

namespace LibraProgramming.Windows.Games.Towers.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class WeakDelegateBase
    {
        protected readonly WeakReference Instance;
        protected readonly MethodInfo Method;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAlive => null != Instance && Instance.IsAlive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate"></param>
        public WeakDelegateBase(Delegate @delegate)
        {
            if (null == @delegate)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            if (null != @delegate.Target)
            {
                Instance = new WeakReference(@delegate.Target);
            }

            Method = @delegate.GetMethodInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Delegate other)
        {
            return null != other && Instance.Target == other.Target && Method.Equals(other.GetMethodInfo());
        }

        protected Delegate CreateDelegate<TDelegate>()
        {
            if (Method.IsStatic)
            {
                return Method.CreateDelegate(typeof(TDelegate));
            }

            if (null == Instance)
            {
                throw new InvalidOperationException();
            }

            return Method.CreateDelegate(typeof(TDelegate), Instance.Target);
        }
    }
}