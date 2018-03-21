namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Mutation
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Original
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte OpCode
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="original"></param>
        /// <param name="opCode"></param>
        public Mutation(int index, byte original, byte opCode)
        {
            Index = index;
            Original = original;
            OpCode = opCode;
        }
    }
}
