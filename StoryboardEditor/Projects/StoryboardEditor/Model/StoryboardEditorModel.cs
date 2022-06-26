#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardEditorModel.cs
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
using ECS.Utilites.Logging;
using StoryboardConfig;
using StoryboardEditor.View;
using StoryboardUtils;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardEditor.Model
{
    public class StoryboardEditorModel 
    {
        #region Fields/Variables
        /// <summary>
        /// Stores the storyboard configuration file
        /// </summary>
        private StoryboardConfiguration storyboardConfig;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the available game cues 
        /// </summary>
        public ObservableCollection<GameCueMenuOptionView> AvailableMenuOptionsCues { get; private set; }

        /// <summary>
        /// Gets or Sets the version number
        /// </summary>
        public string VersionNumber { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// An event listener for enabling the rest of the cue menu 
        /// </summary>
        private static Action<EventArgs> addCuesToCanvasEventListener;

        private static Action<EventArgs> canvasUpdatedEventListener;

        // <summary>
        /// The command that is executed when the File Button New menu items is clicked 
        /// </summary>
        public ICommand FileNewCommand { get; private set; }

        /// <summary>
        /// The command that is executed when the File Button Save menu items is clicked 
        /// </summary>
        public ICommand FileSaveCommand { get; private set; }

        /// <summary>
        /// The command that is executed when the File Button Open menu items is clicked 
        /// </summary>
        public ICommand FileOpenCommand { get; private set; }

        /// <summary>
        /// The command that is executed when the File Button Save As menu items is clicked 
        /// </summary>
        public ICommand FileSaveAsCommand { get; private set; }

        /// <summary>
        /// The command that is executed when the File Exit menu items is clicked 
        /// </summary>
        public ICommand FileExitCommand { get; private set; }

        /// <summary>
        /// The command that is executed when the Help Button User guide menu items is clicked 
        /// </summary>
        public ICommand HelpUserGuideCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardEditorModel"/> class.
        /// </summary>
        public StoryboardEditorModel()
        {
            this.AvailableMenuOptionsCues = new ObservableCollection<GameCueMenuOptionView>();

            this.FileNewCommand = new RelayCommand(new Action<object>((param) => { this.FileNewClicked(); }), () => { return true; });
            this.FileSaveCommand = new RelayCommand(new Action<object>((param) => { this.FileSaveClicked(); }), () => { return true; });
            this.FileSaveAsCommand = new RelayCommand(new Action<object>((param) => { this.FileSaveAsClicked(); }), () => { return true; });
            this.FileOpenCommand = new RelayCommand(new Action<object>((param) => { this.FileOpenClicked(); }), () => { return true; });
            this.FileExitCommand = new RelayCommand(new Action<object>((param) => { this.FileExitClicked(); }), () => { return true; });
            this.HelpUserGuideCommand = new RelayCommand(new Action<object>((param) => { this.HelpUserGuideClicked(); }), () => { return true; });
            this.SearchCommand = new RelayCommand(new Action<object>((param) => { Managers.SearchManager.EnableSearch(); }), () => { return true; });
            
            addCuesToCanvasEventListener = new Action<EventArgs>(this.EnableMenuOptionsEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.ADD_CUES_TO_CANVAS.ToString(), addCuesToCanvasEventListener);

            canvasUpdatedEventListener = new Action<EventArgs>(this.EnableMenuOptionsEvent);
            EventManager.Instance.StartListening(EStoryboardEvents.CANVAS_UPDATED.ToString(), canvasUpdatedEventListener);

            LoadStoryboardConfigs.LoadAllStoryboardConfigs();

            if (LoadStoryboardConfigs.StoryboardConfigurationFile != null)
            {
                this.storyboardConfig = LoadStoryboardConfigs.StoryboardConfigurationFile;
                //this.VersionNumber = this.storyboardConfig?.VersionNumber;
            }

            // Add the assembly version to the title string
            string appVersion = ConfigurationManager.AppSettings["VersionNumber"].ToString(CultureInfo.CurrentCulture);
            this.VersionNumber += $" Version {appVersion}";

            this.CreateMenuOptions();

           
        }
        #endregion

        #region Methods
        #region Non Public Methods

        private void FindFile()
        {
        }

        /// <summary>
        /// Sends the file new event 
        /// </summary>
        private void FileNewClicked()
        {
            // Don't need to trigger the event just call the method
            StoryboardAddCueEventArgs args = new StoryboardAddCueEventArgs()
            {
                Enable = false,
                Opacity = 0.5f
            };

            this.EnableMenuOptionsEvent(args);

            EventManager.Instance.TriggerEvent(EStoryboardEvents.FILE_IO_NEW_CLICKED.ToString());
        }

        /// <summary>
        /// Sends the file save event 
        /// </summary>
        private void FileSaveClicked()
        {
            this.ReleaseTextBoxBinding();

            SaveStoryboardEventArgs evnt = new SaveStoryboardEventArgs()
            {
                ForceNewSaveFile = false,
                FileType = EFileType.SAVE
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.FILE_IO_SAVE_CLICKED.ToString(), evnt);
        }

        /// <summary>
        /// Sends the file save as event
        /// </summary>
        private void FileSaveAsClicked()
        {
            this.ReleaseTextBoxBinding();

            SaveStoryboardEventArgs evnt = new SaveStoryboardEventArgs()
            {
                ForceNewSaveFile = true,
                FileType = EFileType.SAVE_AS
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.FILE_IO_SAVE_CLICKED.ToString(), evnt);
        }

        /// <summary>
        /// Sends the file open event 
        /// </summary>
        private void FileOpenClicked()
        {
            SaveStoryboardEventArgs evnt = new SaveStoryboardEventArgs()
            {
                FileType = EFileType.OPEN
            };

            EventManager.Instance.TriggerEvent(EStoryboardEvents.FILE_IO_OPEN_CLICKED.ToString(), evnt);
        }

        /// <summary>
        /// Kills the application
        /// </summary>
        private void FileExitClicked()
        {
            var result = MessageBox.Show("Are you sure you would like to exit the app", "EXIT", MessageBoxButton.OKCancel);

            if(result == MessageBoxResult.OK)
                Application.Current.Shutdown();
        }
        /// <summary>
        /// Displays the user guide
        /// </summary>
        private void HelpUserGuideClicked()
        {
            Process wordProcess = new Process();
            try
            {
                wordProcess.StartInfo.FileName = @".\UserGuid\Storyboard_user_guid.docx";
                wordProcess.StartInfo.UseShellExecute = true;
                wordProcess.Start();
            }
            catch(Exception e)
            {
                string errorMessage = $"Unable to load Word Document {wordProcess.StartInfo.FileName} with the exception {e.Message}";
                ECSLogger.Instance.LogError(errorMessage);
                MessageBox.Show(errorMessage, "ERROR");
            }
        }
        /// <summary>
        /// Creates the available cue list 
        /// </summary>
        private void CreateMenuOptions()
        {
            var parsegameCues = ParseStoryboardSchema.GetAvailableCues();

            // Add one for the step cue
            var stepCue = new GameCueMenuOptionView("StepCue", false);
            this.AvailableMenuOptionsCues?.Add(stepCue);
            stepCue.Opacity = 0.5; 

            parsegameCues?.ToList()?.ForEach(c =>
            {
                var gameCue = new GameCueMenuOptionView(c, false);
                gameCue.Opacity = 0.5;
                this.AvailableMenuOptionsCues?.Add(gameCue);
            });
        }

        /// <summary>
        /// Enable the Cue buttons once the first step was added
        /// </summary>
        /// <param name="args">Event args</param>
        private void EnableMenuOptionsEvent(EventArgs args)
        {
            // TODO (SF) this is getting called eveytime a menucue gets added
            var evntArgs = args as StoryboardAddCueEventArgs; 

            this.AvailableMenuOptionsCues?.ToList()?.ForEach(c =>
             {
                 var menuModel = c.DataContext as GameCueMenuModel;

                 if (menuModel != null && evntArgs != null)
                 {
                     // Enable the step by default 
                     if (menuModel.Name == "StepCue")
                     {
                         menuModel.EnableAddButton = true;
                         c.Opacity = 1.0;
                     }
                     else
                     {
                         menuModel.EnableAddButton = evntArgs.Enable;
                         c.Opacity = evntArgs.Opacity;
                     }
                 }
             });
        }

        /// <summary>
        /// Releases the text binding on the TextBox if it is in focus by the keyboard.
        /// </summary>
        private void ReleaseTextBoxBinding()
        {
            // Gets the focus element from the Keyboard and casts it as a "TextBox".
            TextBox focusedTextbox = Keyboard.FocusedElement as TextBox;
            if (focusedTextbox == null)
                return;

            // Obtains the binding element from the TextBox.
            BindingExpression textBindingExpr = focusedTextbox.GetBindingExpression(TextBox.TextProperty);

            if (textBindingExpr == null)
                return;

            // Releases and updates the binding element.
            textBindingExpr.UpdateSource();
        }
        #endregion
        #endregion
    }
}
