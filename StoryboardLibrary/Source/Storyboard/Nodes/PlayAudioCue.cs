#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	PlayAudioCue.cs
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
    /// Represents a Play Audio Cue.
    /// This cue was designed to play an audio clip along with a caption
    /// should be tied in with the hud for displaying a popup message
    //////////////////////////////////////////////////////////////////////////
    public class PlayAudioCue : NodeBase
    {
        #region Properties
        /// <summary>
        /// Gets and Sets the audio clip
        /// </summary>
        public string AudioClip { get; set; }

        /// <summary>
        /// Gets and Sets the caption
        /// </summary>
        public string Caption { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayAudioCue"/> class
        /// </summary>
        /// <param name="id">The id</param>
        public PlayAudioCue(string id)
            : base(id)
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

            ActivePayAudioCueEventArgs activePayAudioEventArgs = new ActivePayAudioCueEventArgs()
            {
                AudioClip = this.AudioClip,
                Caption = this.Caption
            };

            EventManager.Instance.TriggerEvent(EventListeners.ACTIVE_PAY_AUDIO_CUE_EVENT.ToString(), activePayAudioEventArgs);
        }
        #endregion
        #endregion
    }
}
