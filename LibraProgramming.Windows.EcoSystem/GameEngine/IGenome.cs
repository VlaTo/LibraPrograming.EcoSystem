namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGenome
    {
        /// <summary>
        /// 
        /// </summary>
        int Length
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        byte this[int index]
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IGenome Clone();
    }
}
