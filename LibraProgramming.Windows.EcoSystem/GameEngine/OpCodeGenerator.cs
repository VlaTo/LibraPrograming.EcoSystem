using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class OpCodeGenerator : IOpCodeGenerator
    {
        private readonly Random random;

        public OpCodeGenerator()
        {
            random = new Random();
        }

        public byte NextOpCode()
        {
            return Convert.ToByte(random.Next(64));
        }
    }
}
