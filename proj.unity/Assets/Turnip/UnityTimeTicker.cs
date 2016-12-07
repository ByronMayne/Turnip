using UnityEngine;
using System.Collections;
using System;
using Turnip;

public class UnityTimeTicker : MonoBehaviour, ITickRunner
{
    /// <summary>
    /// The instance that we are going to tick. 
    /// </summary>
    private ITickable m_Tickable;

    /// <summary>
    /// Does out tick runner have a <see cref="ITickable"/> to run?
    /// </summary>
    private bool m_HasTickable;

    /// <summary>
    /// Gets or sets the Tickable that we run with this runner.
    /// </summary>
    ITickable ITickRunner.Tickable
    {
        get
        {
            return m_Tickable;
        }

        set
        {
            if(typeof(UnityEngine.Object).IsAssignableFrom(value.GetType()))
            {
                // Do to Unity and it's C++ background we have to cast it as an object. 
                m_HasTickable = ((UnityEngine.Object)value) != null; 
            }
            else
            {
                m_HasTickable = m_Tickable != null;
            }
   
            m_Tickable = value;
        }
    }

    /// <summary>
    /// Created only in Awake function where we hide it.
    /// </summary>
    private void Awake()
    {
        // Stop the ticker from being seen.
        hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        // Stop it from being destroyed. 
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Here we tick in our update function. 
    /// </summary>
    private void Update()
    {
        if(m_HasTickable)
        {
            // Only update if we have a tickable.
            m_Tickable.Tick(Time.deltaTime);
        }
    }
}
