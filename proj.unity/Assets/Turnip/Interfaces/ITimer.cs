using System.Collections;
using System;
using UnityEngine;

namespace TurnipTimers
{
    public interface ITimer : ITickable
    {
        // Events
        event TimerExpiredDelegate OnTimerExpired;
        event TickDelegate OnTimerTicked;

        // Properties 
        int ID { get; }
        bool isAvaiableForRecycle { get; }
        bool isExpired { get; }
        double Length { get; set; }
        bool useUnscaledTime { get; set; }
        bool autoRecycle { get; set; }

        void ReassignTimer();

        // Functions
        void Reset();
    }
}
