# Turnip Timers     
 Turnip is an easy to use timer library to use inside of the [UnityEngine](www.unity3d.com). The goal is to make a dead simple API that everyone could use. This includes timers that pool behind the scenes when they have expired, tick themselves with delta time or unscaled delta time. Calling of functions when they are complete, and chaining of timers. That being said Turnip is still in active developement and big changes might accrue. If you have any suggestions please feel free to contact me or make a pull request.


 ## The Basics
 To use Turnip Timers there is only really one class that you need to remember, Trunip.cs. Here is where we create our timers. 
 ```csharp
public class KitchenOven
{
    // We need to bake for 2 hours. 
    private const float COOK_TIME_IN_SECONDS = 2f * 60f * 60f;

    public void StartCooking()
    {
        // Create a new timer
        Timer cookingTimer = Turnip.CreateTimer();
        // Set the time in seconds that we want to cook
        cookingTimer.SetLength(COOK_TIME_IN_SECONDS);
        // The function to call when we are done
        cookingTimer.OnTimerExpired += OnItemCooked;
        // This starts it ticking by default it will not start counting until you call Start(); 
        cookingTimer.Start();
    }

    public void OnItemCooked()
    {
        Debug.Log("Item is cooked");
    }
}

 ```
 In the example above we are doing a very simple task. When someone invokes ```StartCooking()``` we will create a new timer*, set it's length to our cook time, set it's on complete callback, and then start it. Now this timer just starts ticking and never stops. In the case of someone opening the over we might want to pause. 

```csharp
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
```
For this bit of code we created a reference to our timer since we want to be able to stop and start it. We also have two new functions for the when the oven is opened and when it's closed. To pause our timer you call ```Timer.Pause()``` and to resume/start our timer you call ```Timer.Start()```. As soon as the timer is paused it never gets update events and is frozen in time. 

>  * Turnip uses a pool of timers internally so it does not always create new ones it might just recycle and old one. 
 
 ### META

Developed by Byron Mayne [[twitter](https://twitter.com/byMayne) &bull; [github](https://github.com/ByronMayne)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
