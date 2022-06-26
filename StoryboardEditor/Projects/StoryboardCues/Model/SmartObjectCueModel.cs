#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SmartObjectCueModel.cs
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
    public class SmartObjectCueModel : CueBaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the smart object name 
        /// </summary>
        private string smartObjectName;
        public string SmartObjectName
        {
            get { return this.smartObjectName; }
            set
            {
                this.smartObjectName = value;
                IsComplete = (this.smartObjectName != "" && this.selectedHighlightItem != null);
                CueContents[0] = (smartObjectName);
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the condition check box
        /// </summary>
        private bool isConditionSelected;
        public bool IsConditionSelected
        {
            get { return this.isConditionSelected; }
            set
            {
                this.isConditionSelected = value;
                IsComplete = (this.smartObjectName != "" && this.selectedHighlightItem != null);
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the selected highlight
        /// </summary>
        private object selectedHighlightItem;
        public object SelectedHighlightItem
        {
            get { return this.selectedHighlightItem; }
            set
            {
                this.selectedHighlightItem = value;
                IsComplete = (this.smartObjectName != "" && this.selectedHighlightItem != null);
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObjectCueModel"/> class.
        /// </summary>
        public SmartObjectCueModel()
        {
            this.CueName = "SmartObjectCue";
            this.NodeType = ENodeType.GAMECUE_NODE_TYPE;
            this.IsComplete = false;
            this.smartObjectName = "";
            this.IsConditionSelected = false;
            CueContents = new System.Collections.Generic.List<string>();
            CueContents.Add(smartObjectName);
        }
        #endregion
    }
}
