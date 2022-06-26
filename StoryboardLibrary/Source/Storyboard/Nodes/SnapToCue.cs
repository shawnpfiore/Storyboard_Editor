#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SnapToCameraCue.cs
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
using System.Collections.Generic;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    /// Represents a Snap to camera cue.
    /// This cue was designed to drive the camera transitions throughout 
    /// the lesson. Multiple camera can be defined for one transition. 
    //////////////////////////////////////////////////////////////////////////
    public class SnapToCameraCue : NodeBase
    {

        #region Properties
        /// <summary>
        /// Gets ans Sets the snap to cameras 
        /// </summary>
        public List<string> cameras { get; set; }

        /// <summary>
        /// Gets or Sets the delay
        /// </summary>
        public List<float> Delay { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapToCameraCue"/> class
        /// </summary>
        /// <param name="id">The id</param>
        public SnapToCameraCue(string id) 
            : base(id)
        {
            this.cameras = new List<string>();
            this.Delay = new List<float>();
            this.nodeType = ENodeType.GAMECUE_NODE_TYPE;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Call back for the on entered state
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

            ActiveSnapToCueEventArgs activateSnapToEventArgs = new ActiveSnapToCueEventArgs()
            {
                Objects = this.cameras,
                Delay = this.Delay
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVE_SNAP_TO_CAMERA_CUE_EVENT.ToString(), activateSnapToEventArgs);
       }
        #endregion
        #endregion
    }
}
