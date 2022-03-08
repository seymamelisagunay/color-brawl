using System;
using System.Collections.Generic;

namespace Game.UI.Home
{
    public static class ActionQueue
    {
        private static Queue<Action> _progressQueue = new Queue<Action>();

        public static void Next()
        {
            if (_progressQueue.Count == 0) return;
            var progress = _progressQueue.Dequeue();
            progress.Invoke();
        }

        public static void Add(Action progress)
        {
            _progressQueue.Enqueue(progress);
        }

        public static void Clear()
        {
            _progressQueue.Clear();
        }
    }
}