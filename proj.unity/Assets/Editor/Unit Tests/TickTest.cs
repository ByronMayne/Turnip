using UnityEngine;
using UnityEditor;
using System.Collections;
using NUnit.Framework;
using TurnipTimers;
using System;

[TestFixture]
public class TickTest : BaseTurnipTest
{
    [Test, Timeout(1000), RequiresThread]
    public void CreateTimer()
    {
        var timer = Turnip.CreateTimer();
        timer.OnTimerTicked += Timer_OnTimerTicked;
        timer.Start();
        Debug.Log("Started");
    }

    private void Timer_OnTimerTicked(double tickDelta)
    {
        Debug.Log("Passed: " + tickDelta);
        Assert.Pass("Timer was ticked: " + tickDelta);
    }
}
