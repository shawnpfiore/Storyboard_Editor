#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardTaskModel.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 29, 2019
//  Last Update:    	January 29, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using StoryboardUtils.BaseClasses;
using System;
using System.Configuration;
using System.Globalization;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardTask.Model
{
    public class StoryboardTaskModel : NotifyUserControl
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        public ENodeType NodeType { get; set; }

        /// <summary>
        /// Gets or Sets the task Title
        /// </summary>
        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the task short name description 
        /// </summary>
        private string shortName;
        public string ShortName
        {
            get { return this.shortName; }
            set
            {
                this.shortName = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the storyboard date and time 
        /// </summary>
        private string taskCreationDate;
        public string TaskCreationDate
        {
            get { return this.taskCreationDate; }
            set
            {
                this.taskCreationDate = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the storyboard version 
        /// </summary>
        private string versionNumber;
        public string VersionNumber
        {
            get { return this.versionNumber; }
            set
            {
                this.versionNumber = value;
                this.NotifyChange();
            }
        }
        #endregion

        #region Event
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTaskModel"/> class.
        /// </summary>
        public StoryboardTaskModel()
        {
            this.NodeType = ENodeType.TASK_NODE_TYPE;

            this.taskCreationDate = DateTime.Now.ToString("MM/dd/yyyy h:mm tt").Replace(@"/", "-");

            // Add the assembly version to the title string
            string appVersion = ConfigurationManager.AppSettings["VersionNumber"].ToString(CultureInfo.CurrentCulture);
            this.versionNumber += $" Version {appVersion}";
        }
        #endregion
    }
}
