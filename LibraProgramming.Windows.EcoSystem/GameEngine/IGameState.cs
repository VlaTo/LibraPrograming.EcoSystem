namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public interface IGameState
    {
        void Leave(Game game);

        void Enter(Game game);
    }
}