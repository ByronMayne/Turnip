using UnityEngine;
using System.Collections;
using System;

namespace TurnipTimers
{
    public class Timer : ITimer
    {
        private static int m_NextID;
        private double m_Length;
        private double m_TimeRemaining;
        private TimerExpiredDelegate m_OnTimerExpired;
        private TickDelegate m_OnTicked;
        private bool m_IsExpired;
        private bool m_IsPaused;
        private int m_HashID;
        private bool m_UseScaledTime = true;
        private bool m_AutoRecycle = true;


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
        /// If true we update this timer with <see cref="Time.unscaledDeltaTime"/> otherwise
        /// we update with <see cref="Time.deltaTime"/>
        /// </summary>
        public bool useUnscaledTime
        {
            get { return m_UseScaledTime; }
            set { m_UseScaledTime = value; }

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
        public event TimerExpiredDelegate OnTimerExpired
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
        public event TickDelegate OnTimerTicked
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
            get { return m_HashID; }
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
            get { return (m_Length - m_TimeRemaining) / m_Length; }
        }

        /// <summary>
        /// Returns the number of seconds remaining for our timer. This takes into account of it has to loop. 
        /// </summary>
        public double timeRemaining
        {
            get { return m_TimeRemaining; }
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
            m_TimeRemaining = m_Length;
            m_IsExpired = false;
            m_IsPaused = true;
            m_HashID = m_NextID;
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
            m_TimeRemaining = m_Length;
            m_IsExpired = false;
        }

        void ITimer.ReassignTimer()
        {
            Reset();
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

        void ITickable.Tick(double delta, double unscaledDelta)
        {
            if (!m_IsExpired && !m_IsPaused)
            {
                double trueDelta = m_UseScaledTime ? delta : unscaledDelta;
                if (m_TimeRemaining > 0)
                {
                    m_TimeRemaining -= trueDelta;
                    if (m_OnTicked != null)
                    {
                        m_OnTicked(trueDelta);
                    }
                }
                else
                {
                    if (m_OnTimerExpired != null)
                    {
                        m_OnTimerExpired();
                    }
                    m_IsExpired = true;
                }
            }
        }

        public override int GetHashCode()
        {
            return m_HashID;
        }
    }
}
