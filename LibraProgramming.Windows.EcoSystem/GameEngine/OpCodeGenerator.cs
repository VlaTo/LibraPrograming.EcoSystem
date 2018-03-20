using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class OpCodeGenerator : IOpCodeGenerator
    {
        /*private static byte[] opcodes = new byte[]
        {
                2,0,4,0,0,0,6,0,0,0,0,0,0,2,0,0
        };*/

        private readonly Random random;
        //private int count;

        public OpCodeGenerator()
        {
            random = new Random();
        }

        public byte NextOpCode()
        {
            //return Convert.ToByte(random.Next(64));
            return Convert.ToByte(random.Next(8));

            //var index = count++ % opcodes.Length;
            //return opcodes[index];
        }
    }
}
