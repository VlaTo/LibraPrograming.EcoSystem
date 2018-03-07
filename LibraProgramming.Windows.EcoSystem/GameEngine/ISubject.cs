using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ISubject<TSource, TResult> : IObservable<TResult>, IObserver<TSource>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public interface ISubject<TSubject> : ISubject<TSubject, TSubject>
    {
    }
}
