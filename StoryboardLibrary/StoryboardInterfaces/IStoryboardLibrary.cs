#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStoryboardLibrary.cs
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
using System.Collections.Generic;

namespace StoryboardLibary
{
    public static class IStoryboardLibrary
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the transitions array for gobal use
        /// </summary>
        public static List<ITransition> TransitionArray = new List<ITransition>();

        /// <summary>
        /// Gets or Sets the current state for golbal use
        /// </summary>
        public static IState CurrentState { get; set; }
        #endregion

        #region Enumerations
        /// <summary>
        /// Event Listeners thoughout the stroyboard
        /// </summary>
        public enum EventListeners
        {
            NODE_STARTED_EVENT = 0,
            NODE_FINISHED_EVENT,
            STATE_CHANGED_EVENT,
            ACTIVE_PAY_AUDIO_CUE_EVENT,
            ACTIVATE_SMART_OBJECT_CUE_EVENT,
            ACTIVE_SNAP_TO_CAMERA_CUE_EVENT,
            ACTIVE_DELAY_CUE_EVENT,
            ACTIVE_HIGHLIGHT_OBJECTS_CUE_EVENT,
            STORYBOARD_STARTED_EVENT,
            STORYBOARD_FINISHED_EVENT,
            ACTIVE_TASK_STEP_EVENT,
            STORYBOARD_LOADED_EVENT,
            STORYBOARD_FAILED_EVENT,
            AUTOMATE_STORYBOARD_FINISHED_EVENT,
            STORYBOARD_STOPPED_EVENT,
            LESSON_FINISHED_EVENT,
            UNKNOWN_EVENT
        };

        /// <summary>
        /// Event triggers for the storyboard to execute 
        /// </summary>
        public enum EventTriggers
        {
            STOP_LESSON_EVENT = 0,
            LOAD_STORYBOARD_EVENT,
            START_LESSON_EVENT,  
            AUTOMATE_STORYBOARD_EVENT,
            DEBUG_STEP_FORWARDS_EVENT,
            DEBUG_STEP_BACKWARDS_EVENT
        }

        /// <summary>
        /// All storyboard states 
        /// </summary>
        public enum EStoryboardState
        {
            STORYBOARD_UNINITIALIZED = 0,   //< Storyboard created but not loaded
            STORYBOARD_LOADED,              //< Storyboard loaded successfully, but not started
            STORYBOARD_ACTIVE,              //< Storyboard running & currently updating
            STORYBOARD_PAUSED,              //< Storyboard is paused
            STORYBOARD_AUTOMATING,          //< Storyboard is being automated
            STORYBOARD_FINISHED,            //< Storyboard finished successfully
            STORYBOARD_LOAD_FAILED          //< Failed to load the storyboard file, error state
        };

        /// <summary>
        /// storyboard states 
        /// right now we only support one 
        /// </summary>
        public enum EStoryboardType
        {
            ROOT_STORYBOARD
        };

        /// <summary>
        /// Storyboard node types 
        /// </summary>
        public enum ENodeType
        {
            GAMECUE_NODE_TYPE = 0,
            TASK_NODE_TYPE,
            UNKNOWN_NODE_TYPE
        }
        /// <summary>
        /// Storyboard cue types
        /// </summary>
        public enum ECueType
        {
            // Game object cues 
            PLAY_AUDIO_CUE = 0,
            SMART_OBJECT_CUE,
            SNAP_TO_CAMERA_CUE,
            DELAY_CUE,
            HIGHLIGHT_OBJECTS_CUE,

            // Special cues
            START_CUE,
            END_CUE,
            UNKNOWN_CUE
        }

        /// <summary>
        /// Storyboard task types
        /// </summary>
        public enum ETaskType
        {
            STEP_DATA,
            UNKNOWN_TASK
        }
        #endregion
    }
}
