namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public enum BeetleBotMessageType
    {
        /// <summary>
        /// 
        /// </summary>
        Move,

        /// <summary>
        /// 
        /// </summary>
        Dies
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BeetleBotMessage
    {
        public BeetleBotMessageType MessageType
        {
            get;
        }

        public BeetleBot BeetleBot
        {
            get;
        }

        protected BeetleBotMessage(BeetleBotMessageType messageType, BeetleBot beetleBot)
        {
            MessageType = messageType;
            BeetleBot = beetleBot;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class BeetleBotDiesMessage : BeetleBotMessage
    {
        public BeetleBotDiesMessage(BeetleBot beetleBot)
            : base(BeetleBotMessageType.Dies, beetleBot)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class BeetleBotMoveMessage : BeetleBotMessage
    {
        public Coordinates Origin
        {
            get;
        }

        public Coordinates Destination
        {
            get;
        }

        public BeetleBotMoveMessage(BeetleBot beetleBot, Coordinates origin, Coordinates destination)
            : base(BeetleBotMessageType.Move, beetleBot)
        {
            Origin = origin;
            Destination = destination;
        }
    }
}
