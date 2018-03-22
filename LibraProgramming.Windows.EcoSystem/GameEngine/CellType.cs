using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum CellType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Free = 0,

        /// <summary>
        /// 
        /// </summary>
        Occupied = 1,

        /// <summary>
        /// 
        /// </summary>
        Wall = 2,

        /// <summary>
        /// 
        /// </summary>
        Food = 4,

        /// <summary>
        /// 
        /// </summary>
        Poison = 8,

        /// <summary>
        /// The occupation mask for the type <see cref="CellType" />.
        /// </summary>
        OccupationMask = Free | Occupied,

        /// <summary>
        /// The attribute mask for the type <see cref="CellType" />.
        /// </summary>
        AttributeMask = Food | Poison | Wall
    }
}
