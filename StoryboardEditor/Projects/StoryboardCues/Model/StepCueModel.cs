#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StepCueModel.cs
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
    public class StepCueModel : CueBaseModel
    {
        #region Properties

        private bool enableButtons;
        public bool EnableButtons
        {
            get { return this.enableButtons; }
            set
            {
                this.enableButtons = value;
                this.NotifyChange();
            }
        }
        /// <summary>
        /// Gets or Sets the object name Audioclip, avatar, etc... 
        /// </summary>
        private string objectName;
        public string ObjectName
        {
            get { return this.objectName; }
            set
            {
                this.objectName = value;
                IsComplete = (this.objectName != "" && this.stepCaption != "");
                CueContents[1] = objectName;
                this.NotifyChange();
            }
        }
        /// <summary>
        /// Gets or Sets the step caption
        /// </summary>
        private string stepCaption;
        public string StepCaption
        {
            get { return this.stepCaption; }
            set
            {
                this.stepCaption = value;
                IsComplete = (this.objectName != "" && this.stepCaption != "");
                CueContents[0] = stepCaption;
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StepCueModel"/> class.
        /// </summary>
        public StepCueModel()
        {
            this.CueName = "StepCue";
            this.NodeType = ENodeType.TASK_NODE_TYPE;
            this.enableButtons = true;
            this.IsComplete = false;
            this.stepCaption = "";
            this.objectName = "";
            this.CueContents = new System.Collections.Generic.List<string>();
            CueContents.Add(stepCaption);
            CueContents.Add(objectName);
        }

        public void ToggleButtons(bool enable)
        {
            this.enableButtons = enable;
            this.NotifyChange("EnableButtons");
        }
        #endregion
    }
}
