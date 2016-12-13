using UnityEngine;
using System.Collections;
using System;

namespace TurnipTimers
{
    public interface ITimer : ITickable
    {
        double Length { get; set; }

        event Action OnTimerExpired;

        event Action<double> OnTimerTicked;

        int ID { get; }

        bool isExpired { get; }

        bool isAvaiableForRecycle { get; }

        bool useUnscaledTime { get; set; }

        void Reset();
    }
}
