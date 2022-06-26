#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	DelayCueModel.cs
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
using System.Windows;
using static StoryboardEditor.IStoryboardEditorLib;
namespace StoryboardCues.Model
{
    public class DelayCueModel : CueBaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the delay text
        /// </summary>
        private string  delayValue;
        public string DelayValue
        {
            get { return this.delayValue; }
            set
            {
                this.delayValue = value;
                IsComplete = (this.delayValue != "");
                CueContents[0] = this.delayValue;
                this.IsTextAllowed(this.delayValue);
                this.NotifyChange();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DelayCueModel"/> class.
        /// </summary>
        public DelayCueModel()
        {
            this.CueName = "DelayCue";
            this.NodeType = ENodeType.GAMECUE_NODE_TYPE;
            this.IsComplete = false;
            this.CueContents = new System.Collections.Generic.List<string>();
            this.CueContents.Add(CueName);
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Check if the text filed is numeric
        /// </summary>
        /// <param name="text">The delay text text</param>
        private void IsTextAllowed(string text)
        {
            if (regex.IsMatch(text))
            {
                this.delayValue = string.Empty;
                MessageBox.Show(@"Delay value needs to be a numeric value!", "ERROR");
            }
        }

        #endregion
        #endregion
    }
}
