using UnityEngine;
using System.Collections;
using TurnipTimers;
using System;

public class ExampleRunner : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Timer levelTimer = Turnip.CreateTimer();
        levelTimer.SetLength(4.0f);
        levelTimer.OnTimerExpired += OnExpired;
        levelTimer.OnTimerTicked += OnTicked;
        levelTimer.Start();
    }

    private void OnTicked(double delta)
    {
        Debug.Log("Tick: " + delta);
    }

    private void OnExpired()
    {
        Debug.Log("Expired");
    }
}


