#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	GameCueListModel.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	February 08, 2019
//  Last Update:    	March 14, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using ECS.Utilites.Logging;
using StoryboardCues.Model;
using StoryboardCues.View;
using StoryboardUtils;
using StoryboardUtils.BaseClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardEditor.Model
{
    /// <summary>
    /// TODO (SF) cant add a new step
    /// error check if you are trying to relace the first step or adding above the first step
    /// Comment code and clean up
    /// Log more messages throughout 
    /// </summary>
    public class GameCueListModel : NotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the selected cue list item
        /// </summary>
        public string SelectedItem { get; set; }

        /// <summary>
        /// Gets or Sets the game cue list items
        /// </summary>
        public ObservableCollection<string> CueList { get; set; }

        /// <summary>
        /// Gets or Sets the Cue List Visibility 
        /// </summary>
        private Visibility cueListVisibility;
        public Visibility CueListVisibility
        {
            get { return this.cueListVisibility; }
            set
            {
                this.cueListVisibility = value;
                this.NotifyChange();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Gets or Sets the okay command
        /// </summary>
        public ICommand OkayCommand { get; set; }

        /// <summary>
        /// Gets or Sets the cancel command 
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Gets or Sets the double click command
        /// </summary>
        public ICommand DoubleClickCommand { get; set; }
        #endregion

        #region Fields/Variables
        /// <summary>
        /// Stores the cue base model object 
        /// </summary>
        private CueBaseModel cueBase;

        /// <summary>
        /// Stores the cue button type (replace/add above/ add below)
        /// </summary>
        private ECueButtonType cueButtontype;

        /// <summary>
        /// Add cue event listener
        /// </summary>
        private static Action<EventArgs> UpdateCanvasEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCueListModel"/> class.
        /// </summary>
        public GameCueListModel()
        {
            UpdateCanvasEvent = new Action<EventArgs>(this.ReplaceButtonEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.UPDATE_CANVAS_CLICKED.ToString(), UpdateCanvasEvent);

            this.OkayCommand = new RelayCommand(new Action<object>((param) =>
            { this.OkayButtonClicked(); }), () => { return true; });

            this.CancelCommand = new RelayCommand(new Action<object>((param) =>
            { this.CancelButtonClicked(); }), () => { return true; });

            this.DoubleClickCommand = new RelayCommand(new Action<object>((param) =>
            { this.OkayButtonClicked(); }), () => { return true; });

            this.CueListVisibility = Visibility.Hidden;

            this.CueList = new ObservableCollection<string>();

            var parsecameCues = ParseStoryboardSchema.GetAvailableCues();

            parsecameCues?.ToList()?.ForEach(c =>
            {
                this.CueList?.Add(c);
            });

            this.CueList?.Add("StepCue"); 
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Handles when the Okay button is clicked 
        /// </summary>
        private void OkayButtonClicked()
        {
            switch (this.cueButtontype)
            {
                case ECueButtonType.REPLACE:
                    this.ReplaceCue(this.SelectedItem);
                    break;
                case ECueButtonType.ADD_ABOVE:
                    this.AddCueAbove(this.SelectedItem);
                    break;
                case ECueButtonType.ADD_BELOW:
                    this.AddCueBelow(this.SelectedItem);
                    break;
            }

            this.cueListVisibility = Visibility.Hidden;
            this.NotifyChange("CueListVisibility");
        }

        /// <summary>
        /// Handles when the cancel button is clicked 
        /// </summary>
        private void CancelButtonClicked()
        {
            this.cueListVisibility = Visibility.Hidden;
            this.NotifyChange("CueListVisibility");
        }

        /// <summary>
        /// Handles when the replace button is clicked 
        /// </summary>
        /// <param name="evnt"></param>
        private void ReplaceButtonEvent(EventArgs evnt)
        {
            var canvasCueEvent = evnt as StoryboardCanvasCueEventArgs;

            if (canvasCueEvent != null)
            {
                this.cueBase = StoryboardCanvasUtils.GetCueBaseModel(canvasCueEvent?.Name, canvasCueEvent?.Id);
                this.cueButtontype = canvasCueEvent.ButtonType;
                this.cueListVisibility = Visibility.Visible;
                this.NotifyChange("CueListVisibility");
            }
        }

        /// <summary>
        /// Replaces the current cue with another 
        /// </summary>
        /// <param name="cueType">The cue type to change</param>
        private void ReplaceCue(string cueType)
        {
            try
            {
                var userControl = StoryboardCanvasUtils.GetCueUserControl(this.cueBase);
                var index = StoryboardCanvasUtils.CanvasGameCues.IndexOf(userControl);

                switch (cueType)
                {
                    case "PlayAudioCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new PlayAudioCueView());
                        break;
                    case "SnapToCameraCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new SnapToCameraCueView());
                        break;
                    case "SmartObjectCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new SmartObjectCueView());
                        break;
                    case "HighlightObjectsCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new HighlightObjectsCueView());
                        break;
                    case "DelayCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new DelayCueView());
                        break;
                    case "StepCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new StepCueView());
                        break;
                    case "UnknownCue":
                        StoryboardCanvasUtils.RemoveCueIdFromDictinary(this.cueBase);
                        StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(this.cueBase));
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new UnknownCueView());
                        break;
                    default:
                        ECSLogger.Instance.LogError($"Unable to replace cue with {cueType}");
                        break;
                }

                // Saves Cache data after replacing a cue
                StoryboardEditor.Managers.CacheDataManager.WriteCacheFile();
            }
            catch (Exception e)
            {
                ECSLogger.Instance.LogFatal($"Unable to replace cue with exception {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new cue above the current one selected 
        /// </summary>
        /// <param name="cueType">The cue type</param>
        private void AddCueAbove(string cueType)
        {
            try
            {
                var userControl = StoryboardCanvasUtils.GetCueUserControl(this.cueBase);
                var index = StoryboardCanvasUtils.CanvasGameCues.IndexOf(userControl);

                switch (cueType)
                {
                    case "PlayAudioCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new PlayAudioCueView());
                        break;
                    case "SnapToCameraCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new SnapToCameraCueView());
                        break;
                    case "SmartObjectCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new SmartObjectCueView());
                        break;
                    case "HighlightObjectsCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new HighlightObjectsCueView());
                        break;
                    case "DelayCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new DelayCueView());
                        break;
                    case "StepCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new StepCueView());
                        break;
                    case "UnknownCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index, new UnknownCueView());
                        break;
                    default:
                        ECSLogger.Instance.LogError($"Unable to add cue above with name {cueType}");
                        break;
                }

                // Saves Cache data after adding a cue above is used
                StoryboardEditor.Managers.CacheDataManager.WriteCacheFile();
            }
            catch (Exception e)
            {
                ECSLogger.Instance.LogFatal($"Unable to add cue above with exception {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new cue below the current selected cue
        /// </summary>
        /// <param name="cueType">The cue type</param>
        private void AddCueBelow(string cueType)
        {
            try
            {
                var userControl = StoryboardCanvasUtils.GetCueUserControl(this.cueBase);
                var index = StoryboardCanvasUtils.CanvasGameCues.IndexOf(userControl);

                switch (cueType)
                {
                    case "PlayAudioCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new PlayAudioCueView());
                        break;
                    case "SnapToCameraCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new SnapToCameraCueView());
                        break;
                    case "SmartObjectCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new SmartObjectCueView());
                        break;
                    case "HighlightObjectsCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new HighlightObjectsCueView());
                        break;
                    case "DelayCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new DelayCueView());
                        break;
                    case "StepCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new StepCueView());
                        break;
                    case "UnknownCue":
                        StoryboardCanvasUtils.CanvasGameCues?.Insert(index + 1, new UnknownCueView());
                        break;
                    default:
                        ECSLogger.Instance.LogError($"Unable to add cue below with name {cueType}");
                        break;
                }

                // Saves Cache data after adding a cue below is used
                StoryboardEditor.Managers.CacheDataManager.WriteCacheFile();
            }
            catch (Exception e)
            {
                ECSLogger.Instance.LogFatal($"Unable to add cue below with exception {e.Message}");
            }
        }
        #endregion
        #endregion
    }
}
