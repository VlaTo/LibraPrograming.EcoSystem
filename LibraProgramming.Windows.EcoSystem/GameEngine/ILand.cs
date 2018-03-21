namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public interface ILand
    {
        BeetleBot this[Coordinates coordinates]
        {
            get;
            set;
        }
    }
}
