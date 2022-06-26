#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	HighlightObjectsCueModel.cs
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
using StoryboardUtils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Model
{
    /// <summary>
    /// Object for the Listview GridViewColumn items
    /// </summary>
    public class ListViewHighlightObjects
    {
        /// <summary>
        /// Gets or Sets the name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the highlight
        /// </summary>
        public string Highlight { set; get; }
    }

    public class HighlightObjectsCueModel : CueBaseModel
    {
        #region Fields/Variables
        /// <summary>
        /// Gets or Sets the ListViewItems 
        /// </summary>
        public ObservableCollection<ListViewHighlightObjects> ListViewItems { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the gameObeject name
        /// </summary>
        private object gameObjectName;
        public object GameObjectName
        {
            get { return this.gameObjectName; }
            set
            {
                this.gameObjectName = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the selected highlight
        /// </summary>
        private object selectedHighlightItem;
        public object SelectedHightlightItem
        {
            get { return this.selectedHighlightItem; }
            set
            {
                this.selectedHighlightItem = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the selected item in the listview
        /// </summary>
        private object selectedListViewItem;
        public object SelectedListViewItem
        {
            get { return this.selectedListViewItem; }
            set
            {
                this.selectedListViewItem = value;
                this.NotifyChange();
            }
        }
        #endregion


        #region Events
        /// <summary>
        /// Gets or Sets the add gameobejct command 
        /// </summary>
        public ICommand AddButtonCommand { get; private set; }

        public ICommand RemoveButtonCommand { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightObjectsCueModel"/> class.
        /// </summary>
        public HighlightObjectsCueModel()
        {
            this.CueName = "HighlightObjectsCue";
            this.NodeType = ENodeType.GAMECUE_NODE_TYPE;

            this.AddButtonCommand = new RelayCommand(new Action<object>((param) =>
            { this.AddButtonClicked(); }), () => { return true; });

            this.RemoveButtonCommand = new RelayCommand(new Action<object>((param) =>
            { this.RemoveButtonClicked(); }), () => { return true; });

            this.ListViewItems = new ObservableCollection<ListViewHighlightObjects>();
            this.CueContents = new System.Collections.Generic.List<string>();
            this.IsComplete = false;
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Adds the gameobject and hightlight to the listview
        /// </summary>
        private void AddButtonClicked()
        {
            string errorMessage = string.Empty;
            ListViewHighlightObjects listViewObjects = null;

            if (!string.IsNullOrEmpty(this.gameObjectName?.ToString()))
            {
                listViewObjects = new ListViewHighlightObjects()
                {
                    Name = this.gameObjectName?.ToString(),
                    Highlight = ((ComboBoxItem)this.selectedHighlightItem)?.Content?.ToString()
                };

                var found = this.ListViewItems.FirstOrDefault(l => l.Name == listViewObjects.Name);

                if (found == null)
                    ListViewItems.Add(new ListViewHighlightObjects()
                    {
                        Name = this.gameObjectName?.ToString(),
                        Highlight = ((ComboBoxItem)this.selectedHighlightItem)?.Content?.ToString()
                    });
                else
                    errorMessage = $"Game object ( {listViewObjects?.Name} ) has already been defined!";

                this.GameObjectName = string.Empty;
            }
            else
                errorMessage = "Game Object name can not be empty";

            if (listViewObjects == null || !string.IsNullOrEmpty(errorMessage))
                MessageBox.Show(errorMessage, "ERROR");

            this.IsComplete = (ListViewItems.Count > 0);
        }

        /// <summary>
        /// Remove button clicked event 
        /// </summary>
        private void RemoveButtonClicked()
        {
            this.ListViewItems?.Remove(this.ListViewItems?.FirstOrDefault(i =>
            i.Name == ((ListViewHighlightObjects)this.selectedListViewItem)?.Name));
            this.IsComplete = (ListViewItems.Count > 0);
        }

        /// <summary>
        /// Updates the contents of the CueContents List
        /// </summary>
        private void UpdateContentsList()
        {
            CueContents.Clear();
            foreach (ListViewHighlightObjects item in ListViewItems)
            {
                CueContents.Add(item.Name);
                CueContents.Add(item.Highlight);
            }
        }
        #endregion
        #endregion
    }
}
