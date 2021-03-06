﻿using UnityEngine;
using System.Collections;

namespace TurnipTimers
{
    /// <summary>
    /// A class that can be ticked uses this interface. This allows us to swap up Unity ticking (with the update function)
    /// to other native C# versions. 
    /// </summary>
    public interface ITickable
    {
        void OnDisabled();

        void Tick(double delta, double unscaleDelta);

        void OnEnabled();
    }
}
