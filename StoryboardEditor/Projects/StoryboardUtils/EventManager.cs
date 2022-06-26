#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	EventManager.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	December 20, 2018
//  Last Update:    	January 08, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;

public class EventManager
{
    #region Fields/Variables
    /// <summary>
    /// Singleton instance of this class
    /// </summary>
    private static EventManager instance = null;

    /// <summary>
    /// Sync object to facilitate locking so class is multi-thread safe.
    /// </summary>
    private static object sync = new object();

    /// <summary>
    /// Map for the events
    /// </summary>
    private Dictionary<string, Action<EventArgs>> eventDictionary;

    /// <summary>
    /// Instance for the event manager
    /// </summary>
    private EventManager() { }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the StoryboardParser singleton.
    /// </summary>
    public static EventManager Instance
    {
        get
        {
            lock (sync)
            {
                if (instance == null)
                {
                    instance = new EventManager();
                    instance.Init();
                }
            }

            return instance;
        }
    }
    #endregion

    #region Methods
    #region Public Methods
    /// <summary>
    /// Init the map events
    /// </summary>
    public void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<EventArgs>>();
        }
    }
   
    /// <summary>
    /// Start listening for events
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="listener">The event listener</param>
    public void StartListening(string eventName, Action<EventArgs> listener)
    {
        Action<EventArgs> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// Stop listening to events
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="listener">The event listener</param>
    public void StopListening(string eventName, Action<EventArgs> listener)
    {
        if (instance == null) return;
        Action<EventArgs> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    /// <summary>
    /// Triggers the event
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="eventArgs"></param>
    public void TriggerEvent(string eventName, EventArgs eventArgs = null)
    {
        Action<EventArgs> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {

            thisEvent.Invoke(eventArgs);
            // OR USE instance.eventDictionary[eventName]();
        }
    }
    #endregion
    #endregion
}