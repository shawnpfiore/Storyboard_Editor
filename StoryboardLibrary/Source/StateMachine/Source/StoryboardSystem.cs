#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardSystem.cs
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
using ECS.Utilites.Logging;
using StoryboardLibary.EventsDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    ///
    ///	Represents a loaded storyboard file.
    /// Owns the factory, and as a result all child state objects. Contains
    /// accessors for all information the game will need.
    ///
    /// Also owns a map of properties, which are accessed like storyboard-wide
    /// variables. These are used to control the flow of the lesson during runtime
    ///
    //////////////////////////////////////////////////////////////////////////
    public class StoryboardSystem
    {
        #region Fields / Variables
        /// <summary>
        /// Gets or Sets the Taskinfo event args
        /// </summary>
        public List<TaskInfoEventArgs> TaskList { get; set; }
        #endregion

        #region Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Event Listener for the event system to trigger storybaord updates
        public static Action<EventArgs> StopLessonEventListener { get; private set; }
        public static Action<EventArgs> LoadStroyboardEventListener { get; private set; }
        public static Action<EventArgs> StartLessonEventListener { get; private set; }
        public static Action<EventArgs> AutomateStoryboardEventListener { get; private set; }
        public static Action<EventArgs> DebugStepForwardsEventListener { get; private set; }
        public static Action<EventArgs> DebugStepBackwardsEventListener { get; private set; }
        ////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Properties
        /// <summary>
        /// Stores the refences to the loaded storyboard
        /// </summary>
        private List<IStoryboard> loadedStoryboards = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardSystem"/> class
        /// </summary>
        public StoryboardSystem()
        {
            this.loadedStoryboards = new List<IStoryboard>();

            StopLessonEventListener = new Action<EventArgs>(this.StopLessonEvent);
            LoadStroyboardEventListener = new Action<EventArgs>(this.LoadStroyboardEvent);
            StartLessonEventListener = new Action<EventArgs>(this.StartLessonEvent);
            AutomateStoryboardEventListener = new Action<EventArgs>(this.AutomateStoryboardEvent);
            DebugStepForwardsEventListener = new Action<EventArgs>(this.DebugStepForwardsEvent);
            DebugStepBackwardsEventListener = new Action<EventArgs>(this.DebugStepBackwardsEvent);

            EventManager.Instance.StartListening(EventTriggers.STOP_LESSON_EVENT.ToString(), StopLessonEventListener);
            EventManager.Instance.StartListening(EventTriggers.LOAD_STORYBOARD_EVENT.ToString(), LoadStroyboardEventListener);
            EventManager.Instance.StartListening(EventTriggers.START_LESSON_EVENT.ToString(), StartLessonEventListener);
            EventManager.Instance.StartListening(EventTriggers.AUTOMATE_STORYBOARD_EVENT.ToString(), AutomateStoryboardEventListener);
            EventManager.Instance.StartListening(EventTriggers.DEBUG_STEP_FORWARDS_EVENT.ToString(), DebugStepForwardsEventListener);
            EventManager.Instance.StartListening(EventTriggers.DEBUG_STEP_BACKWARDS_EVENT.ToString(), DebugStepBackwardsEventListener);
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Updates the active storyboard
        /// </summary>
        public virtual void Update()
        {
            var active = this.GetActiveStoryboard();
            active?.Update();
        }

        /// <summary>
        /// Clears the loaded storyboard and is ready for shutdown
        /// </summary>
        public virtual void Shutdown()
        {
            this.ClearLoadedStoryboards();
        }

        /// <summary>
        /// Loads a stroyboard
        /// </summary>
        /// <param name="sbName">The storyboard name</param>
        /// <param name="type">The Storyboard type - right now we only support one type</param>
        /// <returns>True if loaded</returns>
        public bool LoadStoryboard(string sbName, EStoryboardType type)
        {
            bool success = false;

            var created = new StoryboardInstance();

            if (created?.Load(sbName, type) == true)
            {
                var activeSb = this.GetActiveStoryboard();

                if (activeSb != null)
                {
                    var state = activeSb?.GetState();

                    if (state == EStoryboardState.STORYBOARD_ACTIVE)
                        created?.StartStoryboard();
                }

                this.loadedStoryboards?.Add(created);
            }
            else
                ECSLogger.Instance.LogInfo("file not found");

            return success;
        }

        /// <summary>
        /// Gets the active storyboard
        /// </summary>
        /// <returns>The active storyboard</returns>
        public IStoryboard GetActiveStoryboard()
        {
            IStoryboard ret = null;

            if (this.loadedStoryboards.Any())
                ret = this.loadedStoryboards?.FirstOrDefault();

            return ret;
        }

        /// <summary>
        /// Gets the root storyboard - if there are more than one on the stack
        /// </summary>
        /// <returns>The root storyboard</returns>
        public IStoryboard GetRootStoryboard()
        {
            IStoryboard ret = null;

            if (this.loadedStoryboards.Any())
            {
                while (this.loadedStoryboards?.Count() > 1)
                {
                    this.PopStoryboard();
                }
                ret = this.loadedStoryboards?.FirstOrDefault();
            }

            return ret;
        }

        /// <summary>
        /// Clears the list of stroyboards
        /// </summary>
        public void ClearLoadedStoryboards()
        {
            if (this.loadedStoryboards.Any())
                this.loadedStoryboards?.Clear();
        }

        /// <summary>
        /// Gets the storyboard state
        /// </summary>
        /// <returns>The state</returns>
        public EStoryboardState GetStoryboardState()
        {
            var active = this.GetActiveStoryboard();
            var state = active?.GetState();

            return (EStoryboardState)state;

        }

        /// <summary>
        /// Gets the TaskInfo event args
        /// </summary>
        /// <param name="taskName">The task name</param>
        /// <returns>The TaskInfo event params</returns>
        public TaskInfoEventArgs GetTaskInfo(string taskName)
        {
            TaskInfoEventArgs taskInfo = null;

            if (!string.IsNullOrWhiteSpace(taskName))
            {
                var active = this.GetActiveStoryboard();

                taskInfo = active?.GetTaskInfo(taskName);
            }

            return taskInfo;
        }

        /// <summary>
        /// Gets the current task 
        /// </summary>
        /// <returns>The TaskInfo event args</returns>
        public TaskInfoEventArgs GetCurrentTask()
        {
            TaskInfoEventArgs taskInfo = null;

            var active = this.GetActiveStoryboard();

            taskInfo = active?.GetCurrentTask();

            return taskInfo;
        }

        /// <summary>
        /// Gets a count of all tasks
        /// </summary>
        /// <returns>The count</returns>
        public Int32 GetAllTasksCount()
        {
            Int32 taskCount = 0;

            var active = this.GetActiveStoryboard();

            if (active != null)
                taskCount = active.GetAllTasks(this.TaskList);

            return taskCount;
        }

        /// <summary>
        /// Gets the current target node 
        /// </summary>
        /// <returns>The name of the node</returns>
        public string GetCurrentTargetObjectName()
        {
            string targetObject = null;

            var active = this.GetActiveStoryboard();

            if (active != null)
                targetObject = active.GetCurrentTargetObject();

            return targetObject;
        }

        /// <summary>
        /// Checks if the storyboard is finished
        /// </summary>
        /// <returns>Returns true or false</returns>
        public bool IsStoryboardFinished()
        {
            bool finished = false;

            if (this.GetActiveStoryboard() == null)
                finished = true;

            return finished;
        }

        /// <summary>
        ///  Event listener to stop the storyboard
        /// </summary>
        /// <param name="evnt">The incoming event params - non needed for this method</param>
        public virtual void StopLessonEvent(EventArgs evnt = null)
        {
            var active = this.GetActiveStoryboard();
            active?.StopStoryboard();
        }

        /// <summary>
        ///  Event listener to load a new stroyboard through a event message
        /// </summary>
        /// <param name="evnt">The incoming event params</param>
        public virtual void LoadStroyboardEvent(EventArgs evnt)
        {
            if (evnt != null)
            {
                var loadStoryboardEventArgs = evnt as LoadStoryboardEventArgs;
                this.LoadStoryboard(loadStoryboardEventArgs?.StoryboardName, loadStoryboardEventArgs.storyboardType);
            }
        }

        /// <summary>
        /// Event listener to start a new lesson
        /// </summary>
        /// <param name="evnt">The incoming event params - non needed for this method</param>
        public virtual void StartLessonEvent(EventArgs evnt = null)
        {
            var sb = this.GetActiveStoryboard();
            sb?.StartStoryboard();
        }

        /// <summary>
        ///  Event listener to automates the storyboard 
        /// </summary>
        /// <param name="evnt">The incoming event params</param>
        public virtual void AutomateStoryboardEvent(EventArgs evnt)
        {
            if (evnt != null)
            {
                var automateStoryboardEventArgs = evnt as AutomateStoryboardEventArgs;

                if (string.IsNullOrEmpty(automateStoryboardEventArgs?.storyboardPath))
                {
                    ECSLogger.Instance.LogInfo("No storyboard specified, using current storyboard");

                    var sb = this.GetActiveStoryboard();
                    sb?.AutomateStoryboard();
                }
                else
                {
                    ECSLogger.Instance.LogInfo($"Loading storyboard {automateStoryboardEventArgs?.storyboardPath}");

                    // Eventually we may want to link stroyboards 
                    if (this.LoadStoryboard(automateStoryboardEventArgs?.storyboardPath, EStoryboardType.ROOT_STORYBOARD))
                    {
                        var sb = this.GetActiveStoryboard();

                        sb?.AutomateStoryboard();
                    }
                    else
                        ECSLogger.Instance.LogError($"Failed to load storyboard for automate {automateStoryboardEventArgs?.storyboardPath}");
                }
            }
        }

        /// <summary>
        ///  Event listener to step forwards throught the storyboard
        /// </summary>
        /// <param name="evnt">The incoming event params - non needed for this method</param>
        public virtual void DebugStepForwardsEvent(EventArgs evnt = null)
        {
            var active = this.GetActiveStoryboard();
            active?.SkipForwards();
        }

        /// <summary>
        ///  Event listener to Step backwards through the storyboard
        /// </summary>
        /// <param name="evnt">The incoming event params - non needed for this method</param>
        public virtual void DebugStepBackwardsEvent(EventArgs evnt = null)
        {
            var active = this.GetActiveStoryboard();
            active?.SkipBack();
        }

        /// <summary>
        /// Kill all event listeners
        /// </summary>
        public virtual void KillAllEventListeners()
        {
            EventManager.Instance.StopListening(EventTriggers.STOP_LESSON_EVENT.ToString(), StopLessonEventListener);
            EventManager.Instance.StopListening(EventTriggers.LOAD_STORYBOARD_EVENT.ToString(), LoadStroyboardEventListener);
            EventManager.Instance.StopListening(EventTriggers.START_LESSON_EVENT.ToString(), StartLessonEventListener);
            EventManager.Instance.StopListening(EventTriggers.AUTOMATE_STORYBOARD_EVENT.ToString(), AutomateStoryboardEventListener);
            EventManager.Instance.StopListening(EventTriggers.DEBUG_STEP_FORWARDS_EVENT.ToString(), DebugStepForwardsEventListener);
            EventManager.Instance.StopListening(EventTriggers.DEBUG_STEP_BACKWARDS_EVENT.ToString(), DebugStepBackwardsEventListener);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Pops the current storyboard loaded of the stack
        /// </summary>
        private void PopStoryboard()
        {
            ECSLogger.Instance.LogInfo("Popping storybaord");
            // There can only be one storyboard active at a time, so the event was sent from
            // the top storyboard on the stack. We need to remove it and kill the instance
            if (this.loadedStoryboards.Any())
            {
                // Get the storyboard so we can remove it
                var found = this.loadedStoryboards?.FirstOrDefault();
                // Remove it!
                this.loadedStoryboards?.Remove(found);

                // find the next storyboard and resume it
                var nextActive = this.GetActiveStoryboard();

                if (nextActive != null)
                {
                    ECSLogger.Instance.LogInfo("Resuming previous storyboard");
                    nextActive?.ResumeStoryboard();
                }
                else
                {
                    // Lesson is complete
                    ECSLogger.Instance.LogInfo("No remaining storyboards, lesson finished");
                    EventManager.Instance.TriggerEvent(EventListeners.LESSON_FINISHED_EVENT.ToString());
                }
            }
        }
        #endregion
        #endregion
    }
}
