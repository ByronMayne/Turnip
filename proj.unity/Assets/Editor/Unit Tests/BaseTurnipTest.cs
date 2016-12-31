using UnityEngine;
using UnityEditor;
using System.Collections;
using NUnit.Framework;
using TurnipTimers;
using System;

[TestFixture]
public class BaseTurnipTest : ITickRunner
{
    private ITickable m_Tickable;

    public ITickable Tickable
    {
        get
        {
            return m_Tickable;
        }

        set
        {
            m_Tickable = value;
        }
    }

    [SetUp]
    public void Setup()
    {
        Turnip.Initialize(this);
        EditorApplication.update += Update;
    }

    public virtual void Update()
    {
        m_Tickable.Tick(Time.deltaTime, Time.unscaledDeltaTime);
    }

    [TearDown]
    public void TearDown()
    {
        EditorApplication.update -= Update;
    }

}
