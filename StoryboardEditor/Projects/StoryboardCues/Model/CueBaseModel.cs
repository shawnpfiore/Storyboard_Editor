#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	CueBaseModel.cs
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
using StoryboardCues.Interfaces;
using StoryboardUtils;
using StoryboardUtils.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Model
{
    public class CueBaseModel : NotifyUserControl, ICueNode
    {
        #region Properties
        /// <summary>
        /// Stores the regex for the numeric check 
        /// </summary>
        public static readonly Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

        /// <summary>
        /// Gets or Sets the CueId
        /// </summary>
        public string CueId { get; set; }

        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        public ENodeType NodeType { get; set; }
          
        /// <summary>
        /// Gets or Sets if node was completely filled out
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or Sets the name
        /// </summary>
        public string CueName { get; set; }

        /// <summary>
        /// Gets or Sets the text content
        /// </summary>
        public List<string> CueContents { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// Gets or Sets the delete command
        /// </summary>
        public ICommand DeleteCueCommand { get; private set; }

        /// <summary>
        /// Gets or Sets the replace cue command
        /// </summary>
        public ICommand ReplaceCueCommand { get; private set; }

        /// <summary>
        /// Gets or Sets the add cue command
        /// </summary>
        public ICommand AddCueAboveCommand { get; private set; }

        /// <summary>
        /// Gets or Sets the add cue command
        /// </summary>
        public ICommand AddCueBelowCommand { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CueBaseModel"/> class.
        /// </summary>
        public CueBaseModel()
        {
            this.DeleteCueCommand = new RelayCommand(new Action<object>((param) =>
            { this.DeleteCueClicked(); }), () => { return true; });
            this.ReplaceCueCommand = new RelayCommand(new Action<object>((param) =>
            { this.ReplaceCueClicked(); }), () => { return true; });
            this.AddCueAboveCommand = new RelayCommand(new Action<object>((param) =>
            { this.AddCueAboveClicked(); }), () => { return true; });
            this.AddCueBelowCommand = new RelayCommand(new Action<object>((param) =>
            { this.AddCueBelowClicked(); }), () => { return true; });
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Generates a new cue id
        /// </summary>
        public void GenerateNewCueId()
        {
            if (this.NodeType == ENodeType.GAMECUE_NODE_TYPE)
            {
                var id = this.GetCurrentCueId(ENodeType.GAMECUE_NODE_TYPE, "GC");
                id++;
                this.CueId = "GC" + id.ToString();

                if (!CueIdDictinary.ContainsKey(this.CueId))
                    CueIdDictinary?.Add(this.CueId, this.NodeType);
            }
            else if (this.NodeType == ENodeType.TASK_NODE_TYPE)
            {
                var id = this.GetCurrentCueId(ENodeType.TASK_NODE_TYPE, "ST");
                id++;
                this.CueId = "ST" + id.ToString();

                if (!CueIdDictinary.ContainsKey(this.CueId))
                    CueIdDictinary?.Add(this.CueId, this.NodeType);
            }
            else if (this.NodeType == ENodeType.UNKNOWN_NODE_TYPE)
            {
                var id = this.GetCurrentCueId(ENodeType.UNKNOWN_NODE_TYPE, "UK");
                id++;
                this.CueId = "UK" + id.ToString();

                if (!CueIdDictinary.ContainsKey(this.CueId))
                    CueIdDictinary?.Add(this.CueId, this.NodeType);
            }

            this.NotifyChange("CueId");
        }
        #endregion

        #region Non Public Methods
        /// <summary>
        /// Delete cue button clicked
        /// </summary>
        private void DeleteCueClicked()
        {
            StoryboardCueEventArgs args = new StoryboardCueEventArgs()
            {
                Id = this.CueId,
                Name = this.CueName,
                NodeType = this.NodeType
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.DELETE_CUE_CLICKED.ToString(), args);
        }

        /// <summary>
        /// Replace cue button clicked 
        /// </summary>
        private void ReplaceCueClicked()
        {
            StoryboardCueEventArgs args = new StoryboardCueEventArgs()
            {
                Id = this.CueId,
                Name = this.CueName,
                NodeType = this.NodeType
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.REPLACE_CUE_CLICKED.ToString(), args);
        }

        /// <summary>
        /// Add cue above button clicked
        /// </summary>
        private void AddCueAboveClicked()
        {
            StoryboardCueEventArgs args = new StoryboardCueEventArgs()
            {
                Id = this.CueId,
                Name = this.CueName,
                NodeType = this.NodeType
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.ADD_CUE_ABOVE_CLICKED.ToString(), args);
        }

        /// <summary>
        /// Add cue below clicked
        /// </summary>
        private void AddCueBelowClicked()
        {
            StoryboardCueEventArgs args = new StoryboardCueEventArgs()
            {
                Id = this.CueId,
                Name = this.CueName,
                NodeType = this.NodeType
            };
            EventManager.Instance.TriggerEvent(EStoryboardEvents.ADD_CUE_BELOW_CLICKED.ToString(), args);
        }

        /// <summary>
        /// Gets the current cue id 
        /// </summary>
        /// <param name="nodeType">The cue node type</param>
        /// <param name="tag">The string tag</param>
        /// <returns>Returns the new cue id</returns>
        private int GetCurrentCueId(ENodeType nodeType, string tag)
        {
            List<int> cueIds = new List<int>();

            if (CueIdDictinary.Values.ToList().Any())
            {
                var cueKey = from entry in CueIdDictinary
                             where entry.Value == nodeType
                             select entry.Key;

                int value;
                cueKey?.ToList()?.ForEach(c =>
                {
                    int.TryParse(c?.Replace(tag, ""), out value);
                    cueIds?.Add(value);
                });
            }
            cueIds.Sort((pair1, pair2) => pair1.CompareTo(pair2));

            return cueIds.LastOrDefault();
        }
        #endregion
        #endregion
    }
}
