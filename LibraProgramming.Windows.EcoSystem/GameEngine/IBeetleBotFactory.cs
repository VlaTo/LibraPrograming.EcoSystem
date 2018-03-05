namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public interface IBeetleBotFactory
    {
        BeetleBot CreateBeetleBot();

        BeetleBot CreateBeetleBot(IGenome genome);
    }
}
