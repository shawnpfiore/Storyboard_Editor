#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	FileIO.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 29, 2019
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
using StoryboardContainer;
using StoryboardCues.Model;
using StoryboardEditor;
using StoryboardEditor.Managers;
using StoryboardTask.Model;
using StoryboardTask.View;
using StoryboardUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardFileIO
{
    public static class FileIO
    {
        #region Fields/Variables
        /// <summary>
        /// Object for the xml writer
        /// </summary>
        private static XmlWriter writer;

        /// <summary>
        /// Object for the xml writer settings
        /// </summary>
        private static XmlWriterSettings settings;

        /// <summary>
        /// Obejct for the root node storyboard container
        /// </summary>
        private static Task task;

        /// <summary>
        /// Array for stroing the gamecue array for thoughout
        /// </summary>
        private static CueBase[] gameCueArray;

        /// <summary>
        /// Stores the game cue index for the gamecue array 
        /// </summary>
        private static int gameCueIndex;

        private static string savedFilePath;
        #endregion

        #region Events
        /// <summary>
        /// Manages the available gamecues 
        /// </summary>
        private static Dictionary<string, Action<CueBaseModel>> HandleAction
        {
            get
            {
                return new Dictionary<string, Action<CueBaseModel>>()
                {
                    { "SmartObjectCue", CreateSmartObjectNodes },
                    { "PlayAudioCue", CreatePlayAudioNodes },
                    { "SnapToCameraCue", CreateSnapToCameraNodes },
                    { "DelayCue" , CreateDelayNodes },
                    { "HighlightObjectsCue", CreateHighlightObjectsNodes },
                    { "UnknownCue" , CreateUnknownNode }
                };
            }
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Saves a stroyboard xml file
        /// </summary>
        /// <param name="storyboardCues">The canvas game vues</param>
        /// <returns>Ture or False if the file saved</returns>
        public static bool SaveFile(ObservableCollection<object> storyboardCues, bool forceNewSaveFile)
        {
            if (!ValidationManager.CuesComplete())
            {
                MessageBox.Show("A Cue is not complete!\n\nCue with ID " + ValidationManager.GetIDOfIncompleteCue() + " is incomplete.\n\nPlease complete the Cue before saving.");
                SearchManager.MoveTo(ValidationManager.GetIndex(ValidationManager.GetIDOfIncompleteCue()));
                return false;
            }

            bool success = false;
            string errorMessage = string.Empty;
            bool? result = false;
            Microsoft.Win32.SaveFileDialog dlg = null;

            if (string.IsNullOrEmpty(savedFilePath) || forceNewSaveFile == true)
            {
                dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    InitialDirectory = @"c:\",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    DefaultExt = ".xml",
                    FileName = "Storyboard.xml",
                    Filter = "XML File|*.xml"
                };

                result = dlg.ShowDialog();
            }
            else
                result = true;

            if (result == true)
            {
                try
                {
                    settings = new XmlWriterSettings();
                    settings.Indent = true;

                    if (string.IsNullOrEmpty(savedFilePath) || forceNewSaveFile == true)
                    {
                        var path = Path.GetDirectoryName(dlg.FileName);
                        savedFilePath = Path.Combine(path, dlg.FileName);
                        writer = XmlWriter.Create(Path.Combine(path, dlg.FileName), settings);
                    }
                    else
                        writer = XmlWriter.Create(savedFilePath, settings);

                    if (writer != null)
                    {
                        // The first root node is always the task node 
                        CreateTaskNode(storyboardCues?.FirstOrDefault());

                        WriteOutFile(storyboardCues);
                        success = true;
                        ECSLogger.Instance.LogInfo($"Saving storyboard file {savedFilePath}");
                    }
                    else
                    {
                        success = false;
                        errorMessage = "Failed to save storyboard due to null storyboard task object";
                    }
                }
                catch (Exception e)
                {
                    success = false;
                    errorMessage = e.Message;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ECSLogger.Instance.LogError($"Failed to save storyboard {dlg.FileName} with error message {errorMessage}");
                MessageBox.Show($"Failed to save storyboard {dlg.FileName} with error message {errorMessage}");
                DisposeWriter();
            }

            return success;
        }

        /// <summary>
        /// Saves a storyboard xml file
        /// </summary>
        /// <param name="storyboardCues">The canvas game cues</param>
        /// <returns>True or False if the file saved</returns>
        public static bool SaveFileTo(ObservableCollection<object> storyboardCues, bool forceNewSaveFile, string filePath)
        {
            bool success = false;
            string errorMessage = string.Empty;

            try
            {
                settings = new XmlWriterSettings();
                settings.Indent = true;

                writer = XmlWriter.Create(filePath, settings);

                if (writer != null)
                {
                    // The first root node is always the task node 
                    CreateTaskNode(storyboardCues?.FirstOrDefault());

                    WriteOutFile(storyboardCues);
                    success = true;
                    ECSLogger.Instance.LogInfo($"Saving storyboard cache file {filePath}");
                }
                else
                {
                    success = false;
                    errorMessage = "Failed to save storyboard due to null storyboard task object";
                }
            }
            catch (Exception e)
            {
                success = false;
                errorMessage = e.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ECSLogger.Instance.LogError($"Failed to save storyboard {filePath} with error message {errorMessage}");
                MessageBox.Show($"Failed to save storyboard {filePath} with error message {errorMessage}");
                DisposeWriter();
            }

            return success;
        }

        /// <summary>
        /// Opens a stroyboard xml file
        /// </summary>
        public static void OpenFile(ObservableCollection<object> storyboardCues)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                InitialDirectory = @"c:\",
                FilterIndex = 2,
                RestoreDirectory = true,
                DefaultExt = ".xml",
                Filter = "XML File|*.xml"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                if (LoadStoryboardConfigs.LoadStoryboardContainerFile(dlg.FileName))
                {
                    var storyboard = LoadStoryboardConfigs.StoryboardContainerFile;
                    if (!UpdateStoryboardCanvas(storyboard, storyboardCues))
                        ECSLogger.Instance.LogError($"Failed to update canvas from file name {dlg.FileName}");
                }
                else
                    ShutdownApp(dlg.FileName);
            }
        }

        /// <summary>
        /// Opens a story xml file from location
        /// </summary>
        public static void OpenFileFrom(ObservableCollection<object> storyboardCues, string filePath)
        {
            if (LoadStoryboardConfigs.LoadStoryboardContainerFile(filePath))
            {
                var storyboard = LoadStoryboardConfigs.StoryboardContainerFile;
                if (!UpdateStoryboardCanvas(storyboard, storyboardCues))
                    ECSLogger.Instance.LogError($"Failed to update canvas from file name {filePath}");
            }
        }
        #endregion

        #region Private Methods

        private static void ShutdownApp(string fileName)
        {
            ECSLogger.Instance.LogFatal($"Unable to load storyboard {fileName}");

            // TODO (SF) handle this more gracefully than just exiting the app 
            var shutDown = MessageBox.Show("Fatal error shutting down applicaation check log for more details!", "FATAL", MessageBoxButton.OK);

            if (shutDown == MessageBoxResult.OK)
                Application.Current.Shutdown();
        }
        /// <summary>
        /// Update stroyboard canvas from opened xml file 
        /// </summary>
        /// <param name="StoryboardContainer">The storyboard container object from the opened xml</param>
        /// <param name="storyboardCanvasCues">The canvas cues</param>
        /// <returns>True or False if updated canvas</returns>
        private static bool UpdateStoryboardCanvas(Task StoryboardContainer, ObservableCollection<object> storyboardCanvasCues)
        {
            bool success = false;

            task = StoryboardContainer;

            if (task != null)
            {
                var taskView = new StoryboardTaskView();
                var taskModel = taskView.DataContext as StoryboardTaskModel;

                taskModel.Title = task?.Title;
                taskModel.ShortName = task?.ShortName;
                taskModel.VersionNumber = task?.StoryboardToolVersion;
                taskModel.TaskCreationDate = task?.Date;
                storyboardCanvasCues?.Add(taskView);

                success = UpdateCanvasCues(storyboardCanvasCues);
            }

            return success;
        }

        /// <summary>
        /// Update the canvas with the loaded cues 
        /// </summary>
        /// <param name="storyboardCanvasCues">The canvas cues</param>
        /// <returns>True or False if the canvas updaeted</returns>
        private static bool UpdateCanvasCues(ObservableCollection<object> storyboardCanvasCues)
        {
            bool success = false;
            string errorMessage = string.Empty;

            try
            {
                var transitions = task?.Transitions;
                List<object> cues = new List<object>();
                task?.TaskData?.Step?.ToList()?.ForEach(s => cues?.Add(s));
                task?.GameCues?.ToList()?.ForEach(c => cues?.Add(c));
                var canvasCues = CreateCanvasCues(cues)?.Cast<FrameworkElement>();

                transitions?.ToList()?.ForEach(t =>
                {
                    canvasCues?.ToList().ForEach(c =>
                    {
                        var data = c.DataContext as CueBaseModel;

                        if (t?.FromStep == data?.CueId)
                        {
                            storyboardCanvasCues?.Add(c);
                            StoryboardEditor.Managers.ValidationManager.Tracker.Add(c);
                        }
                    });
                });

                if (canvasCues != null)
                {
                    StoryboardAddCueEventArgs evnt = new StoryboardAddCueEventArgs()
                    {
                        Enable = true,
                        Opacity = 1.0f
                    };
                    EventManager.Instance.TriggerEvent(EStoryboardEvents.CANVAS_UPDATED.ToString(), evnt);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                ECSLogger.Instance.LogError($"Fialed to update canvas with cues from xml with error message {errorMessage}");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Updates the canvas cues
        /// </summary>
        /// <param name="cueObjects">The list of game cue objects</param>
        /// <returns>The list of game cue objects</returns>
        private static List<object> CreateCanvasCues(List<object> cueObjects)
        {
            List<object> canvasCues = new List<object>();

            cueObjects?.ForEach(c =>
            {
                if (c.GetType() == typeof(Step))
                {
                    var stepView = StoryboardContainerUtils.CreateStepCueView(c);
                    if (stepView != null)
                        canvasCues?.Add(stepView);
                }
                else if (c.GetType() == typeof(SmartObjectCue))
                {
                    var smartObjectView = StoryboardContainerUtils.CreateSmartObjectCueView(c);
                    if (smartObjectView != null)
                        canvasCues?.Add(smartObjectView);
                }
                else if (c.GetType() == typeof(HighlightObjectsCue))
                {
                    var highlightObjectsView = StoryboardContainerUtils.CreateHighlightObjectsCueView(c);
                    if (highlightObjectsView != null)
                        canvasCues?.Add(highlightObjectsView);
                }
                else if (c.GetType() == typeof(DelayCue))
                {
                    var delayCueView = StoryboardContainerUtils.CreateDelayCueView(c);
                    if (delayCueView != null)
                        canvasCues?.Add(delayCueView);
                }
                else if (c.GetType() == typeof(PlayAudioCue))
                {
                    var playAudioView = StoryboardContainerUtils.CreatePlayAudioCueView(c);
                    if (playAudioView != null)
                        canvasCues?.Add(playAudioView);
                }
                else if (c.GetType() == typeof(SnapToCameraCue))
                {
                    var snapToCameraView = StoryboardContainerUtils.CreateSnapToCameraCueView(c);
                    if (snapToCameraView != null)
                        canvasCues?.Add(snapToCameraView);
                }
                else if (c.GetType() == typeof(UnknownCue))
                {
                    var unknownView = StoryboardContainerUtils.CreateUnknownCueView(c);
                    if (unknownView != null)
                        canvasCues?.Add(unknownView);
                }
            });

            return canvasCues;
        }
        /// <summary>
        /// Writes out the storyboard file
        /// </summary>
        /// <param name="cueObject">The cue objects list</param>
        private static void WriteOutFile(ObservableCollection<object> cueObject)
        {
            var stepCueList = new List<CueBaseModel>();
            var gameCueList = new List<CueBaseModel>();
            var transitionList = new List<CueBaseModel>();

            cueObject?.ToList()?.ForEach(c =>
            {
                var view = c as FrameworkElement;

                var cue = view?.DataContext as CueBaseModel;

                // we need all the cues 
                transitionList.Add(cue);

                if (cue?.NodeType == ENodeType.TASK_NODE_TYPE)
                {
                    if (cue?.GetType() == typeof(StepCueModel))
                    {
                        stepCueList?.Add(cue);
                        WriteOutTaskStepNodes(stepCueList);
                    }
                }
                else if (cue?.NodeType == ENodeType.GAMECUE_NODE_TYPE ||
                cue?.NodeType == ENodeType.UNKNOWN_NODE_TYPE)
                {
                    gameCueList?.Add(cue);
                    WriteOutGameCuesNodes(gameCueList);
                }
            });

            WriteOutTransitions(transitionList);

            var serializer = new XmlSerializer(task?.GetType());
            serializer.Serialize(writer, task);
            DisposeWriter();
        }

        /// <summary>
        /// Creates the gamecues
        /// </summary>
        /// <param name="gameCuesNodes">The list of gamecues</param>
        private static void WriteOutGameCuesNodes(List<CueBaseModel> gameCuesNodes)
        {
            gameCueIndex = 0;

            gameCueArray = new CueBase[gameCuesNodes.Count];

            gameCuesNodes?.ForEach(c =>
            {
                if (HandleAction.ContainsKey(c.CueName))
                    HandleAction[c.CueName](c);
                else
                    ECSLogger.Instance.LogWarn($"Unable to find gamecue command ({c.CueName})");
            });

            task.GameCues = gameCueArray;
        }

        /// <summary>
        /// Creates the Task Steps nodes
        /// </summary>
        /// <param name="StepNodes">The list of Step nodes</param>
        private static void WriteOutTaskStepNodes(List<CueBaseModel> StepNodes)
        {
            int stepIndex = 0;
            task.TaskData = new CommonData();
            var stepArray = new Step[StepNodes.Count];

            StepNodes?.ForEach(s =>
            {
                var stepModel = s as StepCueModel;
                stepArray[stepIndex] = new Step() { Id = s.CueId };
                stepArray[stepIndex].StepData = new CommonData();
                stepArray[stepIndex].StepData.Text = stepModel?.StepCaption;
                stepArray[stepIndex].StepData.ObjectName = stepModel?.ObjectName;
                stepIndex++;
            });

            task.TaskData.Step = stepArray;
        }

        /// <summary>
        /// Creates the transitions nodes
        /// </summary>
        /// <param name="cueNodes">All the cue nodes</param>
        private static void WriteOutTransitions(List<CueBaseModel> cueNodes)
        {
            int index = 0;
            var transitionArray = new Transition[cueNodes.Count];

            cueNodes?.ForEach(t =>
            {
                if (t != null)
                {
                    var transition = new Transition()
                    {
                        FromStep = t?.CueId
                    };

                    if (t != cueNodes.LastOrDefault())
                        transition.ToStep = cueNodes?[index + 1]?.CueId;

                    transitionArray[index] = transition;
                }

                index++;
            });

            task.Transitions = transitionArray;
        }

        /////////////////////////////TODO (SF) move to new static class///////////////////////////////////////

        /// <summary>
        /// Creates the task node
        /// </summary>
        /// <param name="taskNode">The task node</param>
        private static void CreateTaskNode(object taskNode)
        {
            var view = taskNode as FrameworkElement;
            var cue = view?.DataContext as StoryboardTaskModel;

            task = new Task()
            {
                Title = cue?.Title,
                ShortName = cue?.ShortName,
                Date = cue?.TaskCreationDate,
                StoryboardToolVersion = cue?.VersionNumber
            };
        }

        /// <summary>
        /// Create smart object nodes
        /// </summary>
        /// <param name="cueBase">The cuebase node</param>
        private static void CreateSmartObjectNodes(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(SmartObjectCueModel))
            {
                var smartObjectCueModel = cueBase as SmartObjectCueModel;
                var smartObjectCue = new SmartObjectCue()
                {
                    Id = cueBase?.CueId,
                    Highlight = ((ComboBoxItem)smartObjectCueModel?.SelectedHighlightItem)?.Content?.ToString(),
                    ObjectName = smartObjectCueModel?.SmartObjectName
                };

                gameCueArray[gameCueIndex] = smartObjectCue;
                gameCueIndex++;
            }
        }

        /// <summary>
        /// Creates the playadudio node
        /// </summary>
        /// <param name="cueBase">The cuebase node</param>
        private static void CreatePlayAudioNodes(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(PlayAudioCueModel))
            {
                var playAudioCueModel = cueBase as PlayAudioCueModel;
                var playAudioCue = new PlayAudioCue()
                {
                    Id = cueBase?.CueId,
                    AudioClip = playAudioCueModel?.AudioFileName,
                    Caption = playAudioCueModel?.AudioDescription
                };

                gameCueArray[gameCueIndex] = playAudioCue;
                gameCueIndex++;
            }
        }

        /// <summary>
        /// Creates the snap to camera node 
        /// </summary>
        /// <param name="cueBase">The cue base node</param>
        private static void CreateSnapToCameraNodes(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(SnapToCameraCueModel))
            {
                var index = 0;
                var snapToCameraCueModel = cueBase as SnapToCameraCueModel;
                var snapToCameraCue = new SnapToCameraCue()
                {
                    Id = cueBase?.CueId
                };

                if (snapToCameraCueModel?.ListViewItems != null)
                {
                    var cameraNamesArray = new string[snapToCameraCueModel.ListViewItems.Count];
                    var cameraDelaysArray = new float[snapToCameraCueModel.ListViewItems.Count];

                    snapToCameraCueModel?.ListViewItems?.ToList()?.ForEach(c =>
                    {
                        cameraNamesArray[index] = c?.Name;
                        cameraDelaysArray[index] = c.Delay;
                        index++;
                    });

                    snapToCameraCue.CameraName = cameraNamesArray;
                    snapToCameraCue.Delay = cameraDelaysArray;
                }

                gameCueArray[gameCueIndex] = snapToCameraCue;
                gameCueIndex++;
            }
        }

        /// <summary>
        /// Creates the delay node
        /// </summary>
        /// <param name="cueBase">The cuebase node</param>
        private static void CreateDelayNodes(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(DelayCueModel))
            {
                var delayCueModel = cueBase as DelayCueModel;

                float value;
                var delayCue = new DelayCue()
                {
                    Id = cueBase?.CueId,
                };

                float.TryParse(delayCueModel?.DelayValue, out value);

                delayCue.Delay = value;

                gameCueArray[gameCueIndex] = delayCue;
                gameCueIndex++;
            }
        }

        /// <summary>
        /// Creates the highlight obeject node
        /// </summary>
        /// <param name="cueBase">The cuebase node</param>
        private static void CreateHighlightObjectsNodes(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(HighlightObjectsCueModel))
            {
                var hightlightCueModel = cueBase as HighlightObjectsCueModel;

                var highlightCue = new HighlightObjectsCue()
                {
                    Id = cueBase.CueId
                };

                if (hightlightCueModel?.ListViewItems != null)
                {
                    var index = 0;
                    var highLightsArray = new string[hightlightCueModel.ListViewItems.Count];
                    var gameObectsArray = new string[hightlightCueModel.ListViewItems.Count];

                    hightlightCueModel?.ListViewItems?.ToList()?.ForEach(h =>
                    {
                        highLightsArray[index] = h?.Highlight;
                        gameObectsArray[index] = h?.Name;
                        index++;
                    });

                    highlightCue.Highlight = highLightsArray;
                    highlightCue.GameObjects = gameObectsArray;
                }

                gameCueArray[gameCueIndex] = highlightCue;
                gameCueIndex++;
            }
        }

        /// <summary>
        /// Creates an unknown cue
        /// </summary>
        /// <param name="cueBase">The cue base</param>
        private static void CreateUnknownNode(CueBaseModel cueBase)
        {
            if (cueBase.GetType() == typeof(UnknownCueModel))
            {
                var unknownCueModel = cueBase as UnknownCueModel;
                var unknownCue = new UnknownCue()
                {
                    Id = cueBase?.CueId,
                    Description = unknownCueModel?.CueDescription
                };

                gameCueArray[gameCueIndex] = unknownCue;
                gameCueIndex++;
            }
        }
        /////////////////////////////TODO (SF) ^^^ move to new static class///////////////////////////////////////

        /// <summary>
        /// Dispose of the xml writer
        /// </summary>
        private static void DisposeWriter()
        {
            writer?.Flush();
            writer?.Close();
            writer?.Dispose();
            writer = null;
        }
        #endregion
        #endregion
    }
}
