using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace TurnipTimers
{
    public class ITimerEqualityComparer : IEqualityComparer<ITimer>
    {
        public bool Equals(ITimer x, ITimer y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(ITimer obj)
        {
            return obj.ID;
        }
    }

    public class Turnip : ITickable
    {
        // Our current instance
        private static Turnip m_Instance;
        // A reference to our runner
        private ITickRunner m_Runner;
        // The current time scale that we are using
        private float m_TimeScale = TimeConstants.DEFAULT_TIME_SCALE;
        // Timers
        private HashSet<ITimer> m_Timers = new HashSet<ITimer>(new ITimerEqualityComparer());
        private HashSet<ITimer> m_PendingTimers = new HashSet<ITimer>();

        /// <summary>
        /// Gets the current instance of the Timepiece or creates a new one. 
        /// </summary>
        private static Turnip instance
        {
            get
            {
                // Is it null?>
                if (m_Instance == null)
                {
                    // Create a new one
                    m_Instance = new Turnip();
                }

                return m_Instance;
            }
        }

        /// <summary>
        /// Gets or sets the time scale that we use for all timers. 
        /// </summary>
        public static float timeScale
        {
            get
            {
                return m_Instance.m_TimeScale;
            }

            set
            {
                m_Instance.m_TimeScale = value;
            }
        }

        /// <summary>
        /// Initializes the timepiece with the runner who does all it's update functions.
        /// </summary>
        /// <param name="runner">The runner that does all the updating</param>
        public static void Initialize(ITickRunner runner)
        {
            // Set our runners ITickable to this
            runner.Tickable = instance;
        }

        /// <summary>
        /// Resets the time scale back to the default defined at <see cref="TimeConstants.DEFAULT_TIME_SCALE"/>.
        /// </summary>
        public static void ResetTimeScale()
        {
            m_Instance.m_TimeScale = TimeConstants.DEFAULT_TIME_SCALE;
        }

        /// <summary>
        /// Creates a new timer that will count down and call a function when it's complete. Returns
        /// the newly created timer.
        /// </summary>
        /// <param name="lengthInSeconds">How many seconds should the timer count down for?</param>
        /// <param name="onTimerComplete">When the timer is complete this will be invoked.</param>
        /// <returns>The newly created timer or a reused expired one.</returns>
        public static Timer CreateTimer(float lengthInSeconds, Action onTimerComplete)
        {
            ITimer timer = null;

            if(m_Instance.TryToRecycleTimer(ref timer))
            {
                // We are reusing so we just reset the length
                timer.Length = lengthInSeconds;
                // This resets it's expired state and it's time remaining.
                timer.Reset();
                // And resets it's callbacks. 
                timer.OnTimerExpired = onTimerComplete;
            }
            else
            {
                // create a new one
                timer = new Timer(lengthInSeconds, onTimerComplete);
                // Add it to our pending queue.
                m_Instance.m_PendingTimers.Add(timer);
            }

            return timer as Timer;
        }

        private bool TryToRecycleTimer(ref ITimer timerToRecycle)
        {
            foreach(ITimer timer in m_Timers)
            {
                if(timer.isExpired)
                {
                    timerToRecycle = timer;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Invoked when our Tickable interface is activated.
        /// </summary>
        void ITickable.OnEnabled()
        {

        }

        /// <summary>
        /// Invoked every frame by our ITickRunner.
        /// </summary>
        /// <param name="delta">The delta time for our tick</param>
        void ITickable.Tick(double delta)
        {
            foreach (ITimer timer in m_Timers)
            {
                timer.Tick(delta);
            }

            // Since users can add new timers on the callbacks from other timers
            // we have to delay adding or our hashset will throw an error.
            if(m_PendingTimers.Count > 0)
            {
                m_Timers.UnionWith(m_PendingTimers);
                m_PendingTimers.Clear();
            }
        }

        /// <summary>
        /// Invoked when our Tickable interface is deactivated.
        /// </summary>
        void ITickable.OnDisabled()
        {

        }
    }
}
