using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface IScene : ISceneNode, IDisposable
    {
        void SetController(GameplayController value);
    }
}