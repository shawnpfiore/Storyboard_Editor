#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	PlayAudioCueModel.cs
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
    public class PlayAudioCueModel : CueBaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the Audio file name
        /// </summary>
        private string audioFileName;
        public string AudioFileName
        {
            get { return this.audioFileName; }
            set
            {
                this.audioFileName = value;
                IsComplete = (this.audioFileName != "" && this.audioDescription != "");
                CueContents[0] = audioFileName;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the audio description
        /// </summary>
        private string audioDescription;
        public string AudioDescription
        {
            get { return this.audioDescription; }
            set
            {
                this.audioDescription = value;
                IsComplete = (this.audioFileName != "" && this.audioDescription != "");
                CueContents[1] = audioDescription;
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayAudioCueModel"/> class.
        /// </summary>
        public PlayAudioCueModel()
        {
            this.CueName = "PlayAudioCue";
            this.NodeType = ENodeType.GAMECUE_NODE_TYPE;
            this.IsComplete = false;
            this.audioFileName = "";
            this.audioDescription = "";
            this.CueContents = new System.Collections.Generic.List<string>();
            this.CueContents.Add(audioFileName);
            this.CueContents.Add(audioDescription);
        }
        #endregion
    }
}
