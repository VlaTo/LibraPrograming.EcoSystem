using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class Genome : IGenome
    {
        private readonly byte[] opcodes;
        private IImmutableList<Mutation> mutations;

        public int Length => opcodes.Length;

        public IReadOnlyList<Mutation> Mutations => mutations;

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
            mutations = ImmutableList<Mutation>.Empty;
        }

        public Genome(Genome genome)
            : this(genome.Length)
        {
            mutations = genome.mutations;
            
            for (var index = 0; index < genome.Length; index++)
            {
                opcodes[index] = genome[index];
            }
        }

        public void Replace(int index, byte opcode)
        {
            var original = this[index];

            this[index] = opcode;
            mutations = mutations.Add(new Mutation(index, original, opcode));
        }

        public IGenome Clone()
        {
            return new Genome(this);
        }
    }
}
