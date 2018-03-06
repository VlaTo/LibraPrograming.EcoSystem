using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class BeetleBotFactory : IBeetleBotFactory
    {
        private readonly IGenomeProducer genomeProducer;
        private readonly IPositioningSystem positioningSystem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genomeProducer"></param>
        /// <param name="positioningSystem"></param>
        public BeetleBotFactory(IGenomeProducer genomeProducer, IPositioningSystem positioningSystem)
        {
            this.genomeProducer = genomeProducer;
            this.positioningSystem = positioningSystem;
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
            var origin = positioningSystem.GetRandomCoordinates();
            var beetleBot = new BeetleBot(origin, genome, TimeSpan.FromSeconds(10.0d), positioningSystem);

            return beetleBot;
        }
    }
}
