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
        private Action<double> m_OnTicked;
        private bool m_IsExpired;
        private bool m_IsPaused;
        private int m_ID;
        private bool m_AutoRecycle = true;
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
        /// Returns if this timer is available for recycle.
        /// </summary>
        bool ITimer.isAvaiableForRecycle
        {
            get { return m_IsExpired && m_AutoRecycle; }
        }

        /// <summary>
        /// Gets or sets the delegate for when this timer has completed.
        /// </summary>
        public event Action OnTimerExpired
        {
            add
            {
                m_OnTimerExpired += value;
            }

            remove
            {
                m_OnTimerExpired -= value;
            }
        }

        /// <summary>
        /// Invoked whenever this timer has ticked in it's update loop
        /// </summary>
        public event Action<double> OnTimerTicked
        {
            add
            {
                m_OnTicked += value;
            }
            remove
            {
                m_OnTicked -= value;
            }
        }

        int ITimer.ID
        {
            get { return m_ID; }
        }

        public bool isExpired
        {
            get { return m_IsExpired; }
        }

        /// <summary>
        /// By default the timer will be reused after it has expired. If this
        /// is set to false the timer will never be recycled by Turnip. This is useful
        /// if you just want to keep using the same timer over and over again.
        /// </summary>
        public bool autoRecycle
        {
            get
            {
                return m_AutoRecycle;
            }

            set
            {
                m_AutoRecycle = value;
            }
        }

        /// <summary>
        /// Get's the progress of the timer. 
        /// <value>(m_Length - m_TimeRemaning) % m_Length</value>
        /// </summary>
        public double progress
        {
            get { return (m_Length - m_TimeRemaning) % m_Length; }
        }

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Start()
        {
            m_IsPaused = false;
        }
        #endregion

        public Timer()
        {
            m_TimeRemaning = m_Length;
            m_IsExpired = false;
            m_IsPaused = true;
            m_ID = m_NextID;
            m_NextID++;
        }

        void ITickable.OnDisabled()
        {
        }

        void ITickable.OnEnabled()
        {
        }

        /// <summary>
        /// Resets the timer to the default length in seconds. Does not effect
        /// the paused state.
        /// </summary>
        public void Reset()
        {
            m_TimeRemaning = m_Length;
            m_IsExpired = false;
        }

        /// <summary>
        /// Resets our timer and changes it's length to a new value. Reset does not
        /// change the paused state. 
        /// </summary>
        /// <param name="lengthInSeconds"></param>
        public void SetLength(float lengthInSeconds)
        {
            m_Length = lengthInSeconds;
            Reset();
        }

        void ITickable.Tick(double delta)
        {
            if (!m_IsExpired && !m_IsPaused)
            {
                if (m_TimeRemaning > 0)
                {
                    m_TimeRemaning -= delta;
                    if(m_OnTicked != null)
                    {
                        m_OnTicked(delta);
                    }
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
