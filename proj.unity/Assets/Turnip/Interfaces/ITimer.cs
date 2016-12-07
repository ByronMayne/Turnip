using UnityEngine;
using System.Collections;
using System;

namespace TurnipTimers
{
    public interface ITimer : ITickable
    {
        double Length { get; set; }

        Action OnTimerExpired { get; set; }

        int ID { get; }

        bool isExpired { get; }

        void Reset();
    }
}
