#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	HighlightObjectsCue.cs
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
using System.Collections.Generic;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class HighlightObjectsCue : NodeBase
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the obejct name for highlighting 
        /// </summary>
        public List<string> ObjectNames { get; set; }

        /// <summary>
        /// Gets or Sets the objects highlight
        /// </summary>
        public List<string> Highlights { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightObjectsCue"/> class
        /// </summary>
        /// <param name="id">The id</param>
        public HighlightObjectsCue(string id) 
            : base(id)
        {
            this.nodeType = IStoryboardLibrary.ENodeType.GAMECUE_NODE_TYPE;
            this.ObjectNames = new List<string>();
            this.Highlights = new List<string>(); 
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

            ActiveHighlightObjectsCueEventArgs activePayAudioEventArgs = new ActiveHighlightObjectsCueEventArgs()
            {
                ObjectNames = this.ObjectNames,
                Highlights = this.Highlights
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVE_HIGHLIGHT_OBJECTS_CUE_EVENT.ToString(), activePayAudioEventArgs);
        }
        #endregion
        #endregion
    }
}
