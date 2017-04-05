using System;

namespace ComicPresence.Common.LangUtils
{
    /// <summary>
    /// Thread-safe
    /// </summary>
    public class RunOnce
    {
        private object _syncRoot = new object();
        private bool _alreadyRun;
        private Action _action;

        public RunOnce(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Invokes action only if it hasn't already been invoked
        /// </summary>
        public void Run()
        {
            if (!_alreadyRun)
            {
                lock (_syncRoot)
                {
                    if (!_alreadyRun)
                    {
                        _action();
                        _alreadyRun = true;
                    }
                }
            }
        }
    }
}
