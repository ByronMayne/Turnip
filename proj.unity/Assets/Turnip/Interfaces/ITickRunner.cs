using UnityEngine;
using System.Collections;

namespace TurnipTimers
{
    /// <summary>
    /// Used to allow Turnip to assign a ticker it's <see cref="ITickable"/> value that it can run.
    /// </summary>
    public interface ITickRunner
    {
        ITickable Tickable { get; set; }
    }
}
