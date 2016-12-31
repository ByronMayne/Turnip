using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace TurnipTimers
{
    public class Turnip : ITickable
    {
        // Our current instance
        private static Turnip m_Instance;
        // A reference to our runner
        private ITickRunner m_Runner;
        // The current time scale that we are using
        private float m_TimeScale = TimeConstants.DEFAULT_TIME_SCALE;
        // Timers
        private HashSet<ITimer> m_ActiveTimers = new HashSet<ITimer>();
        private HashSet<ITimer> m_NewTimers = new HashSet<ITimer>();
        private Queue<WeakReference> m_UserManagedTimers = new Queue<WeakReference>(); 


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

        private Turnip()
        {
            m_ActiveTimers        = new HashSet<ITimer>();
            m_NewTimers = new HashSet<ITimer>();
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
        public static Timer CreateTimer()
        {
            ITimer timer = null;

            if(m_Instance.TryToRecycleTimer(ref timer))
            {
                // This resets it's expired state and it's time remaining.
                timer.ReassignTimer();
                // And resets it's callbacks. 
            }
            else
            {
                // create a new one
                timer = new Timer();
                // Add it to our pending queue.
                m_Instance.m_NewTimers.Add(timer);

            }

            return timer as Timer;
        }

        private bool TryToRecycleTimer(ref ITimer timerToRecycle)
        {
            foreach(ITimer timer in m_ActiveTimers)
            {
                if(timer.isAvaiableForRecycle)
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
        void ITickable.Tick(double delta, double unscaledDelta)
        {
            delta *= m_TimeScale;
            ITimer unmanagedTimerPendingRemoval = null;

            foreach (ITimer timer in m_ActiveTimers)
            {
                timer.Tick(delta, unscaledDelta);

                if(timer.isExpired && timer.autoRecycle)
                {
                    // We only remove one each frame. 
                    unmanagedTimerPendingRemoval = timer;
                }
            }


            // Since users can add new timers on the callbacks from other timers
            // we have to delay adding or our hashset will throw an error.
            if(m_NewTimers.Count > 0)
            {
                m_ActiveTimers.UnionWith(m_NewTimers);
                m_NewTimers.Clear();
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
