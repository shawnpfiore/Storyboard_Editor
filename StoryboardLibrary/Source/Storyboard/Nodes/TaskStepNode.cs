#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	TaskStepNode.cs
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
using StoryboardLibary.EventsDefinitions;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class TaskStepNode : NodeBase
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the task text 
        /// </summary>
        public string TaskText { get; set; }

        /// <summary>
        /// Gets or Sets the step data text 
        /// </summary>
        public string StepDataText { get; set; }

        /// <summary>
        /// Gets or Sets the step data object name
        /// </summary>
        public string StepDataObjectName { get; set; }

        /// <summary>
        /// Gets or Sets the step data condition 
        /// </summary>
        public bool StepDataCondition { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStepNode"/> class
        /// </summary>
        /// <param name="id">The node id</param>
        public TaskStepNode(string id)
            : base(id)
        {
            this.nodeType = ENodeType.TASK_NODE_TYPE;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Call back for the on entered state
        /// </summary>
        public override void OnEnterState()
        {
            ActiveTaskStepCueEventArgs taskStepArgs = new ActiveTaskStepCueEventArgs()
            {
                TaskText = this.TaskText,
                StepDataText = this.StepDataText,
                StepDataObjectName = this.StepDataObjectName
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVE_TASK_STEP_EVENT.ToString(), taskStepArgs);

            base.OnEnterState();
        }
        #endregion
        #endregion
    }
}
