using UnityEngine;
using System.Collections;
using TurnipTimers;
using System;

public class ExampleRunner : MonoBehaviour
{
    TimerPtr timer;

    // Use this for initialization
    void Start()
    {
        timer = Turnip.CreateTimerPtr(); 
    }
}

public class KitchenOven
{
    // We need to bake for 2 hours. 
    private const float COOK_TIME_IN_SECONDS = 2f * 60f * 60f;

    // Keep a reference to our timer
    private Timer m_CookingTimer;

    public void StartCooking()
    {
        // Create a new timer
        Timer m_CookingTimer = Turnip.CreateTimer();
        // Set the time in seconds that we want to cook
        m_CookingTimer.SetLength(COOK_TIME_IN_SECONDS);
        // The function to call when we are done
        m_CookingTimer.OnTimerExpired += OnItemCooked;
        // This starts it ticking by default it will not start counting until you call Start(); 
        m_CookingTimer.Start();
    }

    // Someone opened the over
    private void OnOvenOpened()
    {
        m_CookingTimer.Pause();
    }

    // Someone closed the over
    private void OnOvenClosed()
    {
        m_CookingTimer.Start();
    }

    // Everything is ready!
    public void OnItemCooked()
    {
        Debug.Log("Item is cooked");
    }
}

