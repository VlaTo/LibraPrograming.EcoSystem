using System.Collections.Generic;

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
        IReadOnlyList<Mutation> Mutations
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="opcode"></param>
        void Replace(int index, byte opcode);
    }
}
