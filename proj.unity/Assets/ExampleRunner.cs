using UnityEngine;
using System.Collections;
using TurnipTimers;

public class ExampleRunner : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Turnip.CreateTimer(5, CallMeIn5);
    }

    public void CallMeIn5()
    {
        Debug.Log("Delay for 1 second " + Time.timeSinceLevelLoad);
        Turnip.CreateTimer(1, CallMeIn1);
    }

    public void CallMeIn1()
    {
        Debug.Log("Delay for 5 seconds " + Time.timeSinceLevelLoad);
        Turnip.CreateTimer(5, CallMeIn5);
    }
}
