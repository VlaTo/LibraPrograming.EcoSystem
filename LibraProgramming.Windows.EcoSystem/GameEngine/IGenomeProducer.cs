namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGenomeProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IGenome CreateGenome();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        IGenome MutateGenome(IGenome genome);
    }
}
