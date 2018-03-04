using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Towers.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class WeakEventHandler
    {
        private readonly IList<WeakDelegate<EventHandler>> handlers;

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
        public WeakEventHandler()
        {
            handlers = new List<WeakDelegate<EventHandler>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void AddHandler(EventHandler eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            var @delegate = new WeakDelegate<EventHandler>(handler);

            if (false == handlers.Contains(@delegate))
            {
                handlers.Add(@delegate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RemoveHandler(EventHandler eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            handlers.Remove(new WeakDelegate<EventHandler>(handler));
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class WeakEventHandler<TEventArgs>
        where TEventArgs : EventArgs
    {
        private readonly IList<WeakDelegate<EventHandler<TEventArgs>>> handlers;

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
        public WeakEventHandler()
        {
            handlers = new List<WeakDelegate<EventHandler<TEventArgs>>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void AddHandler(EventHandler<TEventArgs> eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            //handlers.Add(new WeakDelegate<EventHandler<TEventArgs>>(handler));
            var @delegate = new WeakDelegate<EventHandler<TEventArgs>>(handler);

            if (false == handlers.Contains(@delegate))
            {
                handlers.Add(@delegate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RemoveHandler(EventHandler<TEventArgs> eventHandler)
        {
            var handler = (Delegate)((object)eventHandler);
            handlers.Remove(new WeakDelegate<EventHandler<TEventArgs>>(handler));
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