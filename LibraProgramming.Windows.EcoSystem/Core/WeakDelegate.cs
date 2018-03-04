using System;

namespace LibraProgramming.Windows.Games.Towers.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    public sealed class WeakDelegate<TDelegate> : WeakDelegateBase, IEquatable<TDelegate>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate"></param>
        public WeakDelegate(Delegate @delegate)
            : base(@delegate)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TDelegate CreateDelegate()
        {
            return (TDelegate) (object) CreateDelegate<TDelegate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TDelegate other)
        {
            return Equals((Delegate) (object) other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other is WeakDelegate<TDelegate> && Equals((WeakDelegate<TDelegate>) other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(WeakDelegate<TDelegate> left, WeakDelegate<TDelegate> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(WeakDelegate<TDelegate> left, WeakDelegate<TDelegate> right)
        {
            return false == Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Invoke(params object[] args)
        {
            CreateDelegate<TDelegate>().DynamicInvoke(args);
        }

        private bool Equals(WeakDelegate<TDelegate> other)
        {
            if (null != Instance)
            {
                return Instance.Equals(other.Instance) && Method.Equals(other.Method);
            }

            if (null != other.Instance)
            {
                return false;
            }

            return Method.Equals(other.Method);
        }
    }
}