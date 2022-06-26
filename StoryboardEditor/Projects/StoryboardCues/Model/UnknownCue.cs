#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	UnknownCue.cs
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
using StoryboardUtils;
using System;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Model
{
    public class UnknownCue : CueBaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the unknown cue description 
        /// </summary>
        private string cueDescription;
        public string CueDescription
        {
            get { return this.cueDescription; }
            set
            {
                this.cueDescription = value;
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownCue"/> class.
        /// </summary>
        public UnknownCue()
        {
            this.CueName = "UnknownCue";
            this.NodeType = ENodeType.UNKNOWN_NODE_TYPE;

            this.GenerateNewCueId();
        }
        #endregion
    }
}
