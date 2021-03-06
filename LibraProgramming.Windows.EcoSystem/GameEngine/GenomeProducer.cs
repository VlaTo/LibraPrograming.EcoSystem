﻿using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <inheritdoc />
    internal sealed class GenomeProducer : IGenomeProducer
    {
        private readonly IOpCodeGenerator opCodeGenerator;
        private readonly int genomeLength;
        private readonly int mutationsCount;
        private readonly Random random;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCodeGenerator"></param>
        /// <param name="genomeLength"></param>
        public GenomeProducer(IOpCodeGenerator opCodeGenerator, int genomeLength, int mutationsCount)
        {
            this.opCodeGenerator = opCodeGenerator;
            this.genomeLength = genomeLength;
            this.mutationsCount = mutationsCount;

            random = new Random();
        }

        /// <inheritdoc />
        public IGenome CreateGenome()
        {
            var genome = new Genome(genomeLength);

            for(var index = 0; index < genomeLength; index++)
            {
                genome[index] = opCodeGenerator.NextOpCode();
            }

            return genome;
        }

        /// <inheritdoc />
        public IGenome MutateGenome(IGenome genome)
        {
            if (null == genome)
            {
                throw new ArgumentNullException(nameof(genome));
            }

            var clone = genome.Clone();

            for (var count = 0; count < mutationsCount; count++)
            {
                var index = random.Next(clone.Length);
                clone.Replace(index, opCodeGenerator.NextOpCode());
            }

            return clone;
        }
    }
}
