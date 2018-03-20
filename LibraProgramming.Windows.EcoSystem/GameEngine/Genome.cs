using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class Genome : IGenome
    {
        private readonly byte[] opcodes;

        public int Length => opcodes.Length;

        public byte this[int index]
        {
            get
            {
                if (0 > index)
                {
                    throw new IndexOutOfRangeException();
                }

                var idx = index % opcodes.Length;

                return opcodes[idx];
            }
            internal set
            {
                if (0 > index)
                {
                    throw new IndexOutOfRangeException();
                }

                if (opcodes.Length <= index)
                {
                    throw new IndexOutOfRangeException();
                }

                opcodes[index] = value;
            }
        }

        public Genome(int length)
        {
            opcodes = new byte[length];
        }

        public Genome(IGenome genome)
            : this(genome.Length)
        {
            for(var index = 0; index < genome.Length; index++)
            {
                opcodes[index] = genome[index];
            }
        }

        public IGenome Clone()
        {
            return new Genome(this);
        }
    }
}
