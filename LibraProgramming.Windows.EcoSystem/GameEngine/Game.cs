using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class Game: IDisposable
    {
        private bool disposed;
        private IGameState currentState;

        public IGameState CurrentState
        {
            get => currentState;
            set
            {
                currentState?.Leave(this);
                currentState = value;
                currentState?.Enter(this);
            }
        }

        public IScene Scene
        {
            get;
        }

        public MapSize MapSize
        {
            get;
            set;
        }

        public IList<Enemy> Enemies
        {
            get;
        }

        public Game(IScene scene)
        {
            Scene = scene;
            Enemies = new List<Enemy>();
        }

        public void Dispose()
        {
            Dispose(true);
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
                    Scene.Dispose();
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}