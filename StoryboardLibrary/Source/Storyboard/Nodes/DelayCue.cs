#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	DelayCue.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 16, 2018
//  Last Update:    	January 16, 2019
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
    /// Represents a Delay Cue.
    /// This cue was designed to ass a delay to the storyboard
    //////////////////////////////////////////////////////////////////////////
    public class DelayCue : NodeBase
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the delay time
        /// </summary>
        public float Delay { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DelayCue"/> class
        /// </summary>
        /// <param name="id">The id</param>
        public DelayCue(string id)
            : base(id)
        {
            this.nodeType = IStoryboardLibrary.ENodeType.GAMECUE_NODE_TYPE;
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

            ActiveDelayCueEventArgs activePayAudioEventArgs = new ActiveDelayCueEventArgs()
            {
                Delay = this.Delay
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVE_DELAY_CUE_EVENT.ToString(), activePayAudioEventArgs);
        }
        #endregion
        #endregion
    }
}
