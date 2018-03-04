using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Towers.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEventHandler"></typeparam>
    public class WeakEvent<TEventHandler>
    {
        private readonly IList<WeakDelegate<TEventHandler>> handlers;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAlive
        {
            get
            {
                var count = 0;
                var delegates = handlers.ToArray();

                foreach (var handler in delegates)
                {
                    if (handler.IsAlive)
                    {
                        count++;
                    }
                    else
                    {
                        handlers.Remove(handler);
                    }
                }

                return count > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WeakEvent()
        {
            handlers = new List<WeakDelegate<TEventHandler>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void AddHandler(TEventHandler eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            handlers.Add(new WeakDelegate<TEventHandler>(handler));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RemoveHandler(TEventHandler eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            handlers.Remove(new WeakDelegate<TEventHandler>(handler));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Invoke(object sender, EventArgs args)
        {
            var delegates = handlers.ToArray();

            foreach (var handler in delegates)
            {
                if (handler.IsAlive)
                {
                    handler.Invoke(sender, args);
                }
                else
                {
                    handlers.Remove(handler);
                }
            }
        }
    }
}