using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public interface IScene : ISceneNode, IDisposable
    {
        IEcoSystemController Controller
        {
            get;
        }
    }
}