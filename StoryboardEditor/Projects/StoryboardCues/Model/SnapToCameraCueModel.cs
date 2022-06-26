#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SnapToCameraCueModel.cs
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
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Model
{
    /// <summary>
    /// An object for the Listview Items 
    /// </summary>
    public class ListViewSnapToCameraItems
    {
        /// <summary>
        /// Gets or Sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the delay time
        /// </summary>
        public float Delay { get; set; }
    }

    public class SnapToCameraCueModel : CueBaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the list view item
        /// </summary>
        public ObservableCollection<ListViewSnapToCameraItems> ListViewItems { get; set; }

        /// <summary>
        /// Gets or Sets the camera name for the ListView items 
        /// </summary>
        private string cameraName;
        public string CameraName
        {
            get { return this.cameraName; }
            set
            {
                this.cameraName = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Selected camera item 
        /// </summary>
        private object selectedCameraItem;
        public object SelectedCameraItem
        {
            get { return this.selectedCameraItem; }
            set
            {
                this.selectedCameraItem = value;
                this.NotifyChange();
            }
        }

        /// <summary>
        /// Gets or Sets the camera delay 
        /// </summary>
        private string cameraDelay;
        public string CameraDelay
        {
            get { return this.cameraDelay; }
            set
            {
                this.cameraDelay = value;
                this.IsTextAllowed(this.cameraDelay.ToString());
                this.NotifyChange();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Gets or Sets the add camera button cliked 
        /// </summary>
        public ICommand AddCameraCommand { get; set; }

        /// <summary>
        /// Gets or Sets the remove camera button clicked 
        /// </summary>
        public ICommand RemoveCameraCommand { get; set; }
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObjectCueModel"/> class.
        /// </summary>
        public SnapToCameraCueModel()
        {
            this.CueName = "SnapToCameraCue";
            this.NodeType = ENodeType.GAMECUE_NODE_TYPE;

            this.AddCameraCommand = new RelayCommand(new Action<object>((param) =>
            { this.AddCameraClicked(); }), () => { return true; });

            this.RemoveCameraCommand = new RelayCommand(new Action<object>((param) =>
            { this.RemoveCameraClicked(); }), () => { return true; });

            this.ListViewItems = new ObservableCollection<ListViewSnapToCameraItems>();
            this.CueContents = new System.Collections.Generic.List<string>();
            this.IsComplete = false;
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Adds a camera to the listview items
        /// </summary>
        private void AddCameraClicked()
        {
            string errorMessage = string.Empty;
            int value;
            int.TryParse(this.cameraDelay, out value);

            ListViewSnapToCameraItems listViewObjects = new ListViewSnapToCameraItems() { Name = this.cameraName, Delay = value };

            var found = this.ListViewItems.FirstOrDefault(l => l.Name == listViewObjects.Name);

            if (string.IsNullOrEmpty(this.cameraName))
                errorMessage = $"Camera name can not be empty";

            if (string.IsNullOrEmpty(errorMessage))
            {
                if (found == null)
                {
                    this.ListViewItems?.Add(listViewObjects);
                    this.CameraDelay = "0";
                    this.CameraName = string.Empty;
                }
                else
                    errorMessage = $"Camera ( {listViewObjects?.Name} ) has already been defined!";
            }

            if (!string.IsNullOrEmpty(errorMessage))
                MessageBox.Show(errorMessage, "ERROR");

            this.IsComplete = (this.ListViewItems.Count > 0);
            UpdateContentsList();
        }

        /// <summary>
        /// Removes a selected camera item
        /// </summary>
        private void RemoveCameraClicked()
        {
            this.ListViewItems?.Remove(this.ListViewItems?.FirstOrDefault(i =>
            i.Name == ((ListViewSnapToCameraItems)this.selectedCameraItem)?.Name));
            this.IsComplete = (this.ListViewItems.Count > 0);
            UpdateContentsList();
        }

        /// <summary>
        /// Check if the text filed is numeric
        /// </summary>
        /// <param name="text">The delay text text</param>
        private void IsTextAllowed(string text)
        {
            if (regex.IsMatch(text))
            {
                this.cameraDelay = string.Empty;
                MessageBox.Show("Camera delay input needs to be a numeric value!", "ERROR");
            }
        }

        /// <summary>
        /// Updates the contents of the CueContents List
        /// </summary>
        private void UpdateContentsList()
        {
            CueContents.Clear();
            foreach (ListViewSnapToCameraItems item in ListViewItems)
            {
                CueContents.Add(item.Name);
                CueContents.Add(item.Delay.ToString());
            }
        }
        #endregion
        #endregion
    }
}
