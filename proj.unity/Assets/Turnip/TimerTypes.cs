namespace TurnipTimers
{
    public enum TimerTypes
    {
        /// <summary>
        /// This timer is only ever fired once and then is recycled
        /// </summary>
        OneShot,

        /// <summary>
        /// This timer repeats X amount of times until then is recycled.
        /// </summary>
        Repeating,
    }
}

