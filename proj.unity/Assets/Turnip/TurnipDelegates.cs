
namespace TurnipTimers
{
    /// <summary>
    /// A delegate that is invoked whenever a timer has expired
    /// </summary>
    public delegate void TimerExpiredDelegate();

    /// <summary>
    /// Invoked by a timer whenever it's ticked. The tickDelta is the either <see cref="UnityEngine.Time.deltaTime"/>
    /// or <see cref="UnityEngine.Time.unscaledDeltaTime"/> based on the timers <see cref="ITimer.useUnscaledTime"/>
    /// setting.
    /// </summary>
    /// <param name="tickDelta"></param>
    public delegate void TickDelegate(double tickDelta);
}