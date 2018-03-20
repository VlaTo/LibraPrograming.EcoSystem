using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IGenome CreateGenome()
        {
            var genome = new Genome(genomeLength);

            for(var index = 0; index < genomeLength; index++)
            {
                genome[index] = opCodeGenerator.NextOpCode();
            }

            return genome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        public IGenome MutateGenome(IGenome genome)
        {
            if (null == genome)
            {
                throw new ArgumentNullException(nameof(genome));
            }

            var clone = new Genome(genome);

            for (var count = 0; count < mutationsCount; count++)
            {
                var index = random.Next(clone.Length);
                clone[index] = opCodeGenerator.NextOpCode();
            }

            return clone;
        }
    }
}
