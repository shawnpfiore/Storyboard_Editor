#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStoryboardEditorLib.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 20, 2019
//  Last Update:    	January 21, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryboardEditor
{
    public static class IStoryboardEditorLib
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the CueIdDictinary
        /// </summary>
        public static Dictionary<string, ENodeType> CueIdDictinary = new Dictionary<string, ENodeType>();
        #endregion

        #region Enumerations

        public enum EFileType
        {
            SAVE,
            SAVE_AS,
            OPEN
        }

        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        public enum ENodeType
        {
            GAMECUE_NODE_TYPE = 0,
            TASK_NODE_TYPE,
            UNKNOWN_NODE_TYPE
        }

        /// <summary>
        /// Gets or Sets the storyboard events
        /// </summary>
        public enum EStoryboardEvents
        {
            //stroybaord events 
            ADD_CUES_TO_CANVAS = 0,
            DELETE_CUE_CLICKED,
            REPLACE_CUE_CLICKED,
            ADD_CUE_ABOVE_CLICKED,
            ADD_CUE_BELOW_CLICKED,
            UPDATE_CANVAS_CLICKED,
            CANVAS_UPDATED,

            // file IO events 
            FILE_IO_NEW_CLICKED,
            FILE_IO_SAVE_CLICKED,
            FILE_IO_OPEN_CLICKED,
        }

        /// <summary>
        /// Cue button type
        /// </summary>
        public enum ECueButtonType
        {
            REPLACE = 0,
            ADD_ABOVE,
            ADD_BELOW
        }
        #endregion

        #region EventArgs
        public class StoryboardAddCueEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or Sets the cue event name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or Sets the button enabled 
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or Sets the opacity
            /// </summary>
            public float Opacity { get; set; }
        }

        public class StoryboardCueEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or Sets the cue event id;
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or Sets the cue event name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or Sets the cue node type
            /// </summary>
            public ENodeType NodeType { get; set; }
        }

        public class StoryboardCanvasCueEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or Sets the cue event id;
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or Sets the cue event name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or Sets the cue node type
            /// </summary>
            public ECueButtonType ButtonType { get; set; }
        }

        public class SaveStoryboardEventArgs : EventArgs
        {
            /// <summary>
            /// Force a new save file
            /// </summary>
            public bool ForceNewSaveFile { get; set; }

            public EFileType FileType { get; set; }
        }
        #endregion
    }
}
