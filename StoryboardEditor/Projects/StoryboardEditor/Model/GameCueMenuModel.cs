#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardEditor.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 17, 2019
//  Last Update:    	January 17, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using StoryboardUtils;
using StoryboardUtils.BaseClasses;
using System;
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardEditor.Model
{
    public class GameCueMenuModel : NotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the tile 
        /// </summary>
        private string name;
        public string Name
        {
            get { return this.name; }

            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyChange();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        private ENodeType nodeType;
        public ENodeType NodeType
        {
            get { return nodeType; }

            set { this.nodeType = value; }
        }

        /// <summary>
        /// Gets or Sets the delete button disabled property
        /// </summary>
        private bool enableAddButton;
        public bool EnableAddButton
        {
            get { return this.enableAddButton; }
            set
            {
                this.enableAddButton = value;
                this.NotifyChange();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// The command that is executed when the File Button Save menu items is clicked 
        /// </summary>
        public ICommand AddButtonCommand{ get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCueMenuModel"/> class.
        /// </summary>
        public GameCueMenuModel()
        {
            this.AddButtonCommand = new RelayCommand(new Action<object>((param) => { this.AddGameCueToMenu(); }), () => { return true; });
        }
        #endregion

        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Adds a came cue to the menu
        /// </summary>
        private void AddGameCueToMenu()
        {
            StoryboardAddCueEventArgs args = new StoryboardAddCueEventArgs()
            {
                Name = this.name,
                Enable = true,
                Opacity = 1.0f
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.ADD_CUES_TO_CANVAS.ToString(), args);
        }

        #endregion
        #endregion
    }
}
