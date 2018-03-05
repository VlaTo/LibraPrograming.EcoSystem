﻿using System.Numerics;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPositioningSystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        Vector2 GetPosition(Coordinates coordinates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Coordinates GetCoordinates(Vector2 position);
    }
}