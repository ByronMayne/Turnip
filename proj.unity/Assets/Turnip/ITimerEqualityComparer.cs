using System.Collections.Generic;

namespace TurnipTimers
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> is used in our <see cref="HashSet{T}"/> to make sure we use the static incrementing 
    /// <see cref="ITimer.ID"/> instead of the default hash code provided by <see cref="System.object"/>. 
    /// </summary>
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
}