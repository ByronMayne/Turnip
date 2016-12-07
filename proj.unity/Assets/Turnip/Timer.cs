using UnityEngine;
using System.Collections;
using System;

namespace TurnipTimers
{
    public class Timer : ITimer
    {
        private double m_Length;
        private double m_TimeRemaning;
        private Action m_OnTimerExpired;
        private bool m_IsExpired;
        private int m_ID;
        private static int m_NextID;


        #region -= ITimer Interface =-
        /* Normally we don't want anyone editing the Length after a timer
        *  has been created. However Turnip reuses timers when they are done
        *  so we want to be able to change the state. This is hidden from the 
        *  user with our implicit interface.  
        */

        /// <summary>
        /// Gets or sets the length of our timer. 
        /// </summary>
        double ITimer.Length
        {
            get
            {
                return m_Length;
            }

            set
            {
                m_Length = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate for when this timer has completed.
        /// </summary>
        Action ITimer.OnTimerExpired
        {
            get
            {
                return m_OnTimerExpired;
            }

            set
            {
                m_OnTimerExpired = value;
            }
        }

        public int ID
        {
            get { return m_ID; }
        }

        public bool isExpired
        {
            get { return m_IsExpired; }
        }
        #endregion

        public Timer(double lengthInSeconds, Action onTimerExpired)
        {
            m_Length = lengthInSeconds;
            m_TimeRemaning = m_Length;
            m_OnTimerExpired = onTimerExpired;
            m_IsExpired = false;
            m_ID = m_NextID;
            m_NextID++;
        }

        public void OnDisabled()
        {
        }

        public void OnEnabled()
        {
        }

        public void Reset()
        {
            m_TimeRemaning = m_Length;
            m_IsExpired = false;
        }

        public void Tick(double delta)
        {
            if (!m_IsExpired)
            {
                if (m_TimeRemaning > 0)
                {
                    m_TimeRemaning -= delta;
                }
                else
                {
                    m_IsExpired = true;
                    if (m_OnTimerExpired != null)
                    {
                        m_OnTimerExpired();
                    }
                }
            }
        }

        public override int GetHashCode()
        {
            return m_ID;
        }
    }
}
