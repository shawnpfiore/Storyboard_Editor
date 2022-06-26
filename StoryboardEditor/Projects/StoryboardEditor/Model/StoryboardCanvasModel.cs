#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardCanvasModel.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 17, 2019
//  Last Update:    	March 20, 2019
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
using StoryboardEditor.Managers;
using StoryboardFileIO;
using StoryboardTask.View;
using StoryboardUtils.BaseClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardEditor.Model
{
    public class StoryboardCanvasModel : NotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the canvas game cues
        /// </summary>
        public ObservableCollection<object> CanvasGameCues { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Add cue event listener
        /// </summary>
        private static Action<EventArgs> addCueEventListener;

        /// <summary>
        /// Remove cue event listener
        /// </summary>
        private static Action<EventArgs> deleteCueEventListener;

        /// <summary>
        /// Replace cue event listener
        /// </summary>
        private static Action<EventArgs> replaceCueEventListener;

        /// <summary>
        /// Add cue above event listener
        /// </summary>
        private static Action<EventArgs> addCueAboveEventListener;

        /// <summary>
        /// Add cue below event listener
        /// </summary>
        private static Action<EventArgs> addCueBelowEventListener;

        /// <summary>
        /// FileIO new event listener 
        /// </summary>
        private static Action<EventArgs> fileNewEventListener;

        /// <summary>
        /// File save event listener
        /// </summary>
        private static Action<EventArgs> fileSaveEventListener;

        /// <summary>
        /// File open event listener
        /// </summary>
        private static Action<EventArgs> fileOpenEventListener;
        #endregion

        private EFileType fileType;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCanvasModel"/> class.
        /// </summary>
        public StoryboardCanvasModel()
        {
            addCueEventListener = new Action<EventArgs>(this.AddCueNodeToCanvasEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.ADD_CUES_TO_CANVAS.ToString(), addCueEventListener);

            deleteCueEventListener = new Action<EventArgs>(this.DeleteCueNodeFromCanvasEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.DELETE_CUE_CLICKED.ToString(), deleteCueEventListener);

            replaceCueEventListener = new Action<EventArgs>(this.ReplaceCueEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.REPLACE_CUE_CLICKED.ToString(), replaceCueEventListener);

            addCueAboveEventListener = new Action<EventArgs>(this.AddCueAboveEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.ADD_CUE_ABOVE_CLICKED.ToString(), addCueAboveEventListener);

            addCueBelowEventListener = new Action<EventArgs>(this.AddCueBelowEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.ADD_CUE_BELOW_CLICKED.ToString(), addCueBelowEventListener);

            fileNewEventListener = new Action<EventArgs>(this.MenuOptionNewEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.FILE_IO_NEW_CLICKED.ToString(), fileNewEventListener);

            fileSaveEventListener = new Action<EventArgs>(this.SaveStoryboardEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.FILE_IO_SAVE_CLICKED.ToString(), fileSaveEventListener);

            fileOpenEventListener = new Action<EventArgs>(this.OpenStoryboardEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.FILE_IO_OPEN_CLICKED.ToString(), fileOpenEventListener);

            this.CanvasGameCues = new ObservableCollection<object>();

            StoryboardCanvasUtils.CanvasGameCues = this.CanvasGameCues;

            // Will prompt to recover from cache file if it exists. If user selects NO, cache file is deleted
            CacheDataManager.RecoverFromCacheFile();
        }

        #endregion

        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Adds the cue node to the storyboard canvas
        /// </summary>
        /// <param name="evntArgs">The cue event args</param>
        private void AddCueNodeToCanvasEvent(EventArgs evntArgs)
        {
            var cueEventArg = evntArgs as StoryboardAddCueEventArgs;
            Boolean AddedCue = true;
            if (evntArgs != null)
            {
                switch (cueEventArg?.Name)
                {
                    case "PlayAudioCue":
                        this.CanvasGameCues?.Add(new PlayAudioCueView(true));
                        break;
                    case "SnapToCameraCue":
                        this.CanvasGameCues?.Add(new SnapToCameraCueView(true));
                        break;
                    case "SmartObjectCue":
                        this.CanvasGameCues?.Add(new SmartObjectCueView(true));
                        break;
                    case "HighlightObjectsCue":
                        this.CanvasGameCues?.Add(new HighlightObjectsCueView(true));
                        break;
                    case "DelayCue":
                        this.CanvasGameCues?.Add(new DelayCueView(true));
                        break;
                    case "StepCue":
                        this.CanvasGameCues?.Add(new StepCueView(true));
                        this.EnableStepButtons();
                        break;
                    case "UnknownCue":
                        this.CanvasGameCues?.Add(new UnknownCueView(true));
                        break;
                    default:
                        ECSLogger.Instance.LogError($"Unable to create cue {cueEventArg?.Name}");
                        AddedCue = false;
                        break;
                }

               if (AddedCue)
                   ValidationManager.Tracker.Add(this.CanvasGameCues.Last());

                ECSLogger.Instance.LogInfo($"Adding cue {cueEventArg?.Name} to storyboard");

                // Saves Cache data
                SaveCache();
            }
        }

        /// <summary>
        /// Enables the step delete/replace/add above/add below buttons 
        /// </summary>
        private void EnableStepButtons()
        {
            // A little dirty here but the second node is always the first step
            // The first step is required so we need to disable some buttons 
            if (this.CanvasGameCues.Any())
            {
                var step = this.CanvasGameCues?[1] as StepCueView;

                var stepModel = step?.DataContext as StepCueModel;

                stepModel.ToggleButtons(false);
            }
        }

        /// <summary>
        /// Removes the cue for the storyboard canvas
        /// </summary>
        /// <param name="args">The cue event args</param>
        private void DeleteCueNodeFromCanvasEvent(EventArgs args)
        {
            var cueEventArg = args as StoryboardCueEventArgs;

            var cueModel = StoryboardCanvasUtils.GetCueBaseModel(cueEventArg?.Name, cueEventArg?.Id);

            if (cueModel != null)
            {
               // Removes the Cue from the Tracker first.
               if (ValidationManager.Tracker.Contains(StoryboardCanvasUtils.GetCueUserControl(cueModel)))
                   ValidationManager.Tracker.Remove(StoryboardCanvasUtils.GetCueUserControl(cueModel));

                var userControl = StoryboardCanvasUtils.GetCueUserControl(cueModel);
                StoryboardCanvasUtils.RemoveCueIdFromDictinary(cueModel);
                StoryboardCanvasUtils.RemoveControlFromCanvas(StoryboardCanvasUtils.GetCueUserControl(cueModel));
            }

            // Saves Cache data
            SaveCache();

            ECSLogger.Instance.LogInfo($"Removing cue {cueModel?.Name} with id {cueModel?.CueId} from storyboard");
        }

        /// <summary>
        /// Handle the menu option new clicked event 
        /// </summary>
        /// <param name="args">The event args</param>
        private void MenuOptionNewEvent(EventArgs args)
        {
            if (this.CanvasGameCues.Any())
            {
                MessageBoxResult result = MessageBox.Show("Would you like to save your progress first?", "WARNING", MessageBoxButton.YesNoCancel);
                this.MessageBoxResultHandler(result);
            }

            // Add a new task if there is nothing in the Cues.
            if (this.CanvasGameCues?.Count == 0)
                this.CanvasGameCues?.Add(new StoryboardTaskView());
        }

        /// <summary>
        /// Message box result handler
        /// </summary>
        /// <param name="result">The message box result</param>
        private void MessageBoxResultHandler(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // save the storyboard then clear
                    FileIO.SaveFile(this.CanvasGameCues, false);
                    this.ClearStoryboardCanvas();
                    break;
                case MessageBoxResult.No:
                    this.ClearStoryboardCanvas();
                    break;
                case MessageBoxResult.Cancel:
                    //Do nothing 
                    break;
            }
        }

        /// <summary>
        /// Clear the storyboard canvas
        /// </summary>
        private void ClearStoryboardCanvas()
        {
            ECSLogger.Instance.LogInfo($"Clearing storyboard canvas with cues of count {this.CanvasGameCues?.Count()}");

            this.CanvasGameCues?.Clear();
            CueIdDictinary?.Clear();

            // Add a new step to the new storyboard
            if (fileType != EFileType.OPEN)
                this.CanvasGameCues?.Add(new StoryboardTaskView());

            // Saves Cache data
            SaveCache();
        }

        /// <summary>
        /// Handle the Save button file option 
        /// </summary>
        /// <param name="evntArgs">Save file event args</param>
        private void SaveStoryboardEvent(EventArgs evntArgs)
        {
            var evnt = evntArgs as SaveStoryboardEventArgs;

            this.fileType = evnt.FileType;

            if (evnt != null)
            {
                if (this.CanvasGameCues.Any())
                    FileIO.SaveFile(this.CanvasGameCues, evnt.ForceNewSaveFile);
            }

            // Saves Cache data
            SaveCache();
        }

        private void OpenStoryboardEvent(EventArgs evntArgs)
        {
            var evnt = evntArgs as SaveStoryboardEventArgs;

            this.fileType = evnt.FileType;

            if (evnt != null)
            {
                if (this.CanvasGameCues.Any())
                {
                    MessageBoxResult result = MessageBox.Show("Would you like to save your progress first?", "WARNING", MessageBoxButton.YesNoCancel);
                    this.MessageBoxResultHandler(result);
                }

                FileIO.OpenFile(this.CanvasGameCues);
            }
        }

        /// <summary>
        /// Replace current cue
        /// </summary>
        /// <param name="evntArgs">Cue event args</param>
        private void ReplaceCueEvent(EventArgs evntArgs)
        {
            var cueEvent = evntArgs as StoryboardCueEventArgs;
            var canvasCueEvent = new StoryboardCanvasCueEventArgs()
            {
                Id = cueEvent?.Id,
                Name = cueEvent?.Name,
                ButtonType = ECueButtonType.REPLACE
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.UPDATE_CANVAS_CLICKED.ToString(), canvasCueEvent);
        }

        /// <summary>
        /// Add cue above 
        /// </summary>
        /// <param name="evntArgs">Cue event args</param>
        private void AddCueAboveEvent(EventArgs evntArgs)
        {
            var cueEvent = evntArgs as StoryboardCueEventArgs;
            var canvasCueEvent = new StoryboardCanvasCueEventArgs()
            {
                Id = cueEvent?.Id,
                Name = cueEvent?.Name,
                ButtonType = ECueButtonType.ADD_ABOVE
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.UPDATE_CANVAS_CLICKED.ToString(), canvasCueEvent);
        }

        /// <summary>
        /// Add cue below
        /// </summary>
        /// <param name="evntArgs">Cue Event args</param>
        private void AddCueBelowEvent(EventArgs evntArgs)
        {
            var cueEvent = evntArgs as StoryboardCueEventArgs;
            var canvasCueEvent = new StoryboardCanvasCueEventArgs()
            {
                Id = cueEvent?.Id,
                Name = cueEvent?.Name,
                ButtonType = ECueButtonType.ADD_BELOW
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.UPDATE_CANVAS_CLICKED.ToString(), canvasCueEvent);
        }

        private void SaveCache()
        {
            CacheDataManager.WriteCacheFile();
        }
    }
    #endregion
    #endregion
}
