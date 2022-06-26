#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardInstance.cs
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
using System.Threading;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    /// Represents a loaded storyboard file.
    /// Owns the factory, and as a result all child state objects. Contains
    /// accessors for all information the game will need.
    ///
    /// Also owns a map of properties, which are accessed like storyboard-wide
    /// variables. These are used to control the flow of the lesson during runtime
    //////////////////////////////////////////////////////////////////////////
    public class StoryboardInstance : IStoryboard
    {
        #region Fields / Variables
        /// <summary>
        /// Gets or Sets the state machine factory
        /// </summary>
        public StateMachineFactory Factory { get; private set; }

        /// <summary>
        /// Gets or Sets the storybord factory
        /// </summary>
        public StoryboardFactory SbFactory { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Stores the loaded storyboard 
        /// </summary>
        private IStateMachine loadedStroyboard = null;

        /// <summary>
        /// Map for the dtoryboard nodes
        /// </summary>
        private Dictionary<string, IStoryboardNode> storyboardNodes;

        /// <summary>
        /// Stores the task short name
        /// </summary>
        private string shortName = string.Empty;

        /// <summary>
        /// Stores the task title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Stores the storyboard file path
        /// </summary>
        private string filePath = string.Empty;

        /// <summary>
        /// Stores the current state in the state machine
        /// </summary>
        private EStoryboardState currentState;

        /// <summary>
        /// Stores the current storyboard type
        /// </summary>
        private EStoryboardType type;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardInstance"/> class
        /// </summary>
        public StoryboardInstance()
        {
            this.currentState = EStoryboardState.STORYBOARD_UNINITIALIZED;
            this.type = EStoryboardType.ROOT_STORYBOARD;
            this.storyboardNodes = new Dictionary<string, IStoryboardNode>();
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Basic constructor, creates but does not load the storyboard
        /// </summary>
        /// <param name="storyboardFile">The file path to the storyboard file that will be loaded</param>
        /// <param name="sbType">The storyboard type</param>
        /// <returns>True or False if storyboard was created</returns>
        public virtual bool Load(string storyboardFile, EStoryboardType sbType)
        {
            if (this.currentState == EStoryboardState.STORYBOARD_UNINITIALIZED || this.currentState == EStoryboardState.STORYBOARD_LOADED)
            {
                this.type = sbType;
                this.Factory = new StateMachineFactory();
                this.SbFactory = new StoryboardFactory(this.Factory);
                StoryboardLoadedEventArgs loadedEventArgs = new StoryboardLoadedEventArgs();

                StoryboardParser parser = new StoryboardParser(this);
                this.loadedStroyboard = parser.Parse(storyboardFile, loadedEventArgs, this.type);

                if (this.loadedStroyboard != null)
                    this.currentState = EStoryboardState.STORYBOARD_LOADED;
                else
                {
                    this.currentState = EStoryboardState.STORYBOARD_LOAD_FAILED;
                    ECSLogger.Instance.LogInfo("Storyboard load failed!");
                }

                if (this.currentState == EStoryboardState.STORYBOARD_LOADED)
                {
                    this.filePath = storyboardFile;
                    title = loadedEventArgs?.StoryboardName;
                    this.storyboardNodes?.Clear();

                    // Store off all the created nodes locally, so we don't have to constantly
                    // ping the factory and constantly cast things up to IStoryboardNodes
                    var createdStates = this.Factory?.StateMap;

                    createdStates?.ForEach(s =>
                    {
                        dynamic asNode = (IStoryboardNode)s;

                        if (asNode != null)
                            this.storyboardNodes[asNode.GetId()] = (IStoryboardNode)asNode;
                    });

                    loadedEventArgs.StoryboardType = this.type;
                    EventManager.Instance.TriggerEvent(EventListeners.STORYBOARD_LOADED_EVENT.ToString(), loadedEventArgs);
                }

                if (this.currentState != EStoryboardState.STORYBOARD_LOADED)
                    EventManager.Instance.TriggerEvent(EventListeners.STORYBOARD_FAILED_EVENT.ToString());
            }

            return currentState == EStoryboardState.STORYBOARD_LOADED;
        }

        /// <summary>
        /// Resets the storyboard 
        /// </summary>
        /// <returns>True or False</returns>
        public virtual bool Reset()
        {
            if (currentState == EStoryboardState.STORYBOARD_LOADED ||
                currentState == EStoryboardState.STORYBOARD_ACTIVE ||
                currentState == EStoryboardState.STORYBOARD_FINISHED)
            {
                this.StopStoryboard();
                this.currentState = EStoryboardState.STORYBOARD_UNINITIALIZED;

                if (this.Load(this.filePath, this.type))
                    this.currentState = EStoryboardState.STORYBOARD_ACTIVE;
            }

            return currentState == EStoryboardState.STORYBOARD_LOADED;
        }

        /// <summary>
        /// Pause the storyboard
        /// </summary>
        public virtual void PauseStoryboard()
        {
            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE)
                this.currentState = EStoryboardState.STORYBOARD_PAUSED;
        }

        /// <summary>
        /// Resume the storyboard
        /// </summary>
        public virtual void ResumeStoryboard()
        {
            if (this.currentState == EStoryboardState.STORYBOARD_PAUSED)
                this.currentState = EStoryboardState.STORYBOARD_ACTIVE;
        }

        /// <summary>
        /// Automate the storyboard
        /// </summary>
        /// <param name="path">The storyboard file path</param>
        public virtual void AutomateStoryboard()
        {
            ECSLogger.Instance.LogInfo($"Automation started for storyboard {this.filePath}");

            if (this.loadedStroyboard != null)
            {
                this.currentState = EStoryboardState.STORYBOARD_AUTOMATING;

                var node = this.loadedStroyboard?.GetStateById("start");
                var endIt = this.storyboardNodes?.Values?.LastOrDefault();
                var transitions = IStoryboardLibrary.TransitionArray;
                Int32 transitionIdx = 0;
                while (node != null)
                {
                    // Lookup the node
                    var sbNode = this.storyboardNodes?[node?.GetStateName()];

                    if (sbNode != endIt)
                    {
                        sbNode?.AutomateNode();
                        // transition to the next node
                        var numTranstions = node.GetNumTransition();
                        
                        node = null;
                        while (transitionIdx < numTranstions)
                        {
                            if (transitions[transitionIdx].CanTransition())
                            {
                                node = transitions?[transitionIdx]?.GetDestinationState();
                                this.loadedStroyboard.Update();
                                break;
                            }
                        }

                        transitionIdx++;
                        // wait, so Unity can catch up
                        //TODO (SF) make this value data driven
                        Thread.Sleep(200);
                        //await Task.Delay(200);
                    }
                }

                // Hackkk Let Unity have a moment to process 
                //TODO (SF) make this value data driven
                Thread.Sleep(3000);
                ECSLogger.Instance.LogInfo($"Automation finished for storyboard {this.filePath}");
                EventManager.Instance.TriggerEvent(EventListeners.AUTOMATE_STORYBOARD_FINISHED_EVENT.ToString());
                this.currentState = EStoryboardState.STORYBOARD_FINISHED;
            }
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <param name="output">The taskinfo event args output</param>
        /// <returns>The number of transitions</returns>
        public virtual Int32 GetAllTasks(List<TaskInfoEventArgs> output)
        {
            output = new List<TaskInfoEventArgs>();
            var transitions = IStoryboardLibrary.TransitionArray;

            Int32 current = 0;
            if (this.loadedStroyboard != null)
            {
                // This is messy, but we are walking through the transitions in the 
                // start with the 'start' node
                var node = this.loadedStroyboard.GetStateById("start");
                var endIt = this.storyboardNodes.Values?.LastOrDefault();

                while (node != null)
                {
                    // Lookup the node
                    var sbNode = this.storyboardNodes?[node.GetStateName()];

                    if (sbNode != endIt)
                    {
                        // we only care about tasks in this function, so skip all cues
                        var nodeType = sbNode.GetNodeType();
                        if (nodeType == ENodeType.TASK_NODE_TYPE)
                        {
                            var asTask = sbNode;
                            output.Add((TaskInfoEventArgs)asTask);
                        }

                        // transition to the next node
                        var numTransitions = node.GetNumTransition();

                        if (numTransitions > 0)
                        {
                            node = null;
                            // Iterate through the transitions to see if there is a valid one for this storyboard path
                            while (current < numTransitions)
                            {
                                if (transitions[current].CanTransition())
                                {
                                    node = transitions?[current]?.GetDestinationState();
                                    break;
                                }
                                current++;
                            }
                        }
                        else
                        {
                            node = null;
                        }
                    }
                }
            }

            return current;
        }

        /// <summary>
        /// Gets the current node
        /// </summary>
        /// <returns>The storyboard node</returns>
        public virtual IStoryboardNode GetCurrentNode()
        {
            IStoryboardNode currentNode = null;

            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE || this.currentState == EStoryboardState.STORYBOARD_FINISHED)
            {
                var currentState = this.loadedStroyboard?.CurrentState;
                currentNode = (IStoryboardNode)currentState;
            }

            return currentNode;
        }

        /// <summary>
        /// Gets the current target object 
        /// </summary>
        /// <returns>The name of the current object</returns>
        public virtual string GetCurrentTargetObject()
        {
            string value = string.Empty;

            IStoryboardNode node = this.GetCurrentNode();

            if (node != null)
            {
                var casted = (NodeBase)node;

                if (casted != null)
                    value = casted.ToString();
            }

            return value;
        }

        /// <summary>
        /// Gets the current task
        /// </summary>
        /// <returns>The current task info event args</returns>
        public virtual TaskInfoEventArgs GetCurrentTask()
        {
            TaskInfoEventArgs found = null;

            IStoryboardNode node = this.GetCurrentNode();

            // Make sure this is actually a task node
            if (node?.GetNodeType() == ENodeType.TASK_NODE_TYPE)
            {
                var casted = (TaskInfoEventArgs)node;
                if (casted != null)
                    found = casted;
            }

            return found;
        }

        /// <summary>
        /// Get the stroyboard state
        /// </summary>
        /// <returns>The storyboard state</returns>
        public virtual EStoryboardState GetState()
        {
            return this.currentState;
        }

        /// <summary>
        /// Gets the storyboard file
        /// </summary>
        /// <returns>The name of the storyboard file path</returns>
        public virtual string GetStoryboardFile()
        {
            return this.filePath;
        }

        /// <summary>
        /// Gets the storyboard name
        /// </summary>
        /// <returns>The name of the storyboard</returns>
        public virtual string GetStoryboardName()
        {
            return this.shortName;
        }

        /// <summary>
        /// Gets the task info event args
        /// </summary>
        /// <param name="taskName">The task name</param>
        /// <returns>The task info event args</returns>
        public virtual TaskInfoEventArgs GetTaskInfo(string taskName)
        {
            TaskInfoEventArgs found = null;

            if (this.storyboardNodes.ContainsKey(taskName))
            {
                var it = this.storyboardNodes?[taskName];

                if (it != this.storyboardNodes?.Values?.Last())
                {
                    // Make sure this is actually a task node
                    if (it.GetNodeType() == ENodeType.TASK_NODE_TYPE)
                        found = (TaskInfoEventArgs)it;
                }
            }

            return found;
        }

       /// <summary>
       /// Skip the storyboard forwards debug only
       /// </summary>
        public virtual void SkipForwards()
        {
            ECSLogger.Instance.LogInfo("Skipping forward");

            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE)
            {
                //Find our current node if it exists
                var currentNode = this.GetCurrentNode();

                if (currentNode != null)
                {
                    var transitions = IStoryboardLibrary.TransitionArray;

                    var node = this.loadedStroyboard?.GetStateById(currentNode.GetId());
                    //Get the Transitions within our current Node
                    var numTransitions = node?.GetNumTransition();

                    // Loop over transitions, if can transition use it we must
                    if (numTransitions > 0)
                    {
                        //Temporary variables for iterating our listeses
                        IState tempTransition = null;
                        int transIndex = 0;

                        while (transIndex < numTransitions)
                        {
                            if (transitions[transIndex].CanTransition())
                            {
                                tempTransition = transitions?[transIndex]?.GetDestinationState();
                                ((NodeBase)currentNode)?.OnTransitionOut(tempTransition);
                                //Push our now current previous state onto the stack
                                this.loadedStroyboard?.PushPreviousState((IState)currentNode);
                                //Set the current state to the next state we found
                                this.loadedStroyboard?.SetCurrentState(tempTransition);

                                break;
                            }
                            transIndex++; // Increment our Transition index within this task
                        }
                    }

                }
                else
                    ECSLogger.Instance.LogError("Current state is null, set it using SetCurrentState");
            }
        }

        /// <summary>
        /// skip back through the storyboard debug only
        /// </summary>
        public virtual void SkipBack()
        {
            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE)
            {
                var currentNode = this.GetCurrentNode();

                if (currentNode != null)
                {
                    ((NodeBase)currentNode)?.OnTransitionOut(null);

                    this.loadedStroyboard?.PopPreviousState();

                    var previousState = this.loadedStroyboard?.PreviousState;

                    if (previousState != null)
                        this.loadedStroyboard?.SetCurrentState(previousState);
                }
            }
        }

        /// <summary>
        /// Starts the storyboard
        /// </summary>
        /// <returns>True or False</returns>
        public virtual bool StartStoryboard()
        {
            bool success = false;

            if (this.currentState == EStoryboardState.STORYBOARD_LOADED)
            {
                this.currentState = EStoryboardState.STORYBOARD_ACTIVE;
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Stop the storyboard
        /// </summary>
        /// <returns>True or False</returns>
        public virtual bool StopStoryboard()
        {
            bool success = false;

            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE)
            {
                this.currentState = EStoryboardState.STORYBOARD_UNINITIALIZED;
                success = true;

                EventManager.Instance.TriggerEvent(EventListeners.STORYBOARD_STOPPED_EVENT.ToString());
            }

            return success;
        }

        public virtual void Update()
        {
            if (this.currentState == EStoryboardState.STORYBOARD_ACTIVE)
                this.loadedStroyboard?.Update();
        }
        #endregion
        #endregion
    }
}
