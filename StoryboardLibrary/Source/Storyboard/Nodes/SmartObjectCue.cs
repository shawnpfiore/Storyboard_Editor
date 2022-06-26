#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SmartObjectCue.cs
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
    //////////////////////////////////////////////////////////////////////////
    /// Represents a Smart object cue.
    /// This cue was designed for a unity smart object to trigger 
    /// the storyboard update within unity. 
    //////////////////////////////////////////////////////////////////////////
    public class SmartObjectCue : NodeBase
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the object name
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Gets or Sets the highlight
        /// </summary>
        public string Highlight { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObjectCue"/> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="highlight"></param>
        public SmartObjectCue(string id, ICondition condition) 
            : base(id, condition)
        {
            this.nodeType = ENodeType.GAMECUE_NODE_TYPE;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// On entered state call back 
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

            ActivateSmartObjectCueEventArgs activateSmartObjectArgs = new ActivateSmartObjectCueEventArgs()
            {
                ObjectName = this.ObjectName,
                Highlight = this.Highlight
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVATE_SMART_OBJECT_CUE_EVENT.ToString(), activateSmartObjectArgs);
        }
        #endregion
        #endregion
    }
}
