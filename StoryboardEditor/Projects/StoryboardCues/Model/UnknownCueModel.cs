#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	UnknownCue.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 20, 2019
//  Last Update:    	March 14, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Model
{
    public class UnknownCueModel : CueBaseModel
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
                IsComplete = (this.cueDescription != "");
                CueContents[0] = cueDescription;
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownCueModel"/> class.
        /// </summary>
        public UnknownCueModel()
        {
            this.CueName = "UnknownCue";
            this.NodeType = ENodeType.UNKNOWN_NODE_TYPE;
            this.cueDescription = "";
            this.IsComplete = false;
            this.CueContents = new System.Collections.Generic.List<string>();
            this.CueContents.Add(cueDescription);
        }
        #endregion
    }
}
