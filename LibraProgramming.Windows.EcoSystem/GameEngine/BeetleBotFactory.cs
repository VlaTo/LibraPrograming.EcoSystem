using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class BeetleBotFactory : IBeetleBotFactory
    {
        private readonly IGenomeProducer genomeProducer;
        private readonly ICreatePositionProvider positionProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genomeProducer"></param>
        /// <param name="positioningSystem"></param>
        public BeetleBotFactory(ICreatePositionProvider positionProvider, IGenomeProducer genomeProducer)
        {
            this.positionProvider = positionProvider;
            this.genomeProducer = genomeProducer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BeetleBot CreateBeetleBot()
        {
            var genome = genomeProducer.CreateGenome();
            return CreateBeetleBot(genome);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        public BeetleBot CreateBeetleBot(IGenome genome)
        {
            var origin = positionProvider.CreatePosition();
            var beetleBot = new BeetleBot(origin, genome, TimeSpan.FromSeconds(10.0d));

            return beetleBot;
        }
    }
}
