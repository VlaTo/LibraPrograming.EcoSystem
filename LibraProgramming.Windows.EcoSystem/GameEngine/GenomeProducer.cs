namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class GenomeProducer : IGenomeProducer
    {
        private readonly IOpCodeGenerator opCodeGenerator;
        private readonly int cellCount;

        public GenomeProducer(IOpCodeGenerator opCodeGenerator, int cellCount)
        {
            this.opCodeGenerator = opCodeGenerator;
            this.cellCount = cellCount;
        }

        public IGenome CreateGenome()
        {
            var genome = new Genome(cellCount);

            for(var index = 0; index < cellCount; index++)
            {
                genome[index] = opCodeGenerator.NextOpCode();
            }

            return genome;
        }
    }
}
