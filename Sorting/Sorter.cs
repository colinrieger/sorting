﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sorting
{
    public abstract class Sorter
    {
        private Stopwatch m_StopWatch = new Stopwatch();

        public string TimeElapsed
        {
            get
            {
                TimeSpan ts = m_StopWatch.Elapsed;
                return String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
            }
        }

        public void Sort(List<int> list, int version = 1)
        {
            m_StopWatch.Restart();
            HandleSort(list, version);
            m_StopWatch.Stop();
        }
        
        protected abstract void HandleSort(List<int> list, int version = 1);
    }
}
