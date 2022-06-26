#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	LoadStoryboardConfigs.cs
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
using LoadAppConfigs;
using StoryboardConfig;
using StoryboardContainer;
using System;

namespace StoryboardUtils
{
    public static class LoadStoryboardConfigs
    {
        #region Properties
        /// <summary>
        /// Gets the storyboard configuration file object 
        /// </summary>
        public static StoryboardConfiguration StoryboardConfigurationFile { get; private set; }

        /// <summary>
        /// Gets or Sets the storyboard object 
        /// </summary>
        public static Task StoryboardContainerFile { get; private set; }
        #endregion

        #region Fields / Variables
        /// <summary>
        /// Stores the LoadConfigurations object 
        /// </summary>
        private static LoadConfigurations loadConfig = null;

        #endregion

        #region Constructor
        /// <summary>
        ///  Loads all configs or xml files associated with the storyboard editor
        /// </summary>
        public static void LoadAllStoryboardConfigs()
        {
            ECSLogger.Instance.Initialize(@".\data\loggerconfig.ini");
            ECSLogger.Instance.LogInfo("Loading storyboardEditor configuration files");

            loadConfig = new LoadConfigurations();

            StoryboardConfigurationFile = new StoryboardConfiguration();

            StoryboardContainerFile = new Task();

            LoadStoryboardConfigurationFile();

        }
        #endregion

        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Loads the storyboard configuration file
        /// </summary>
        private static void LoadStoryboardConfigurationFile()
        {
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var method = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

            try
            {
                if (loadConfig.LoadConfiguration<StoryboardConfiguration>(@".\data\StoryboardEditorConfig.xml"))
                    StoryboardConfigurationFile = (StoryboardConfiguration)loadConfig.Settings;
            }
            catch (Exception e)
            {
                ECSLogger.Instance.LogError(e.Message, method);
            }
        }

        /// <summary>
        /// Loads a storyboard file 
        /// </summary>
        /// <returns>True if successful</returns>
        public static bool LoadStoryboardContainerFile(string fileName)
        {
            bool success = false;
            string errorMessage = string.Empty;

            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var method = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

            try
            {
                if (loadConfig.LoadConfiguration<Task>(fileName))
                    StoryboardContainerFile = (Task)loadConfig.Settings;
                else
                    ECSLogger.Instance.LogError($"Failed to load storyboard file {fileName}");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                ECSLogger.Instance.LogError($"{e.Message} {method}");
            }

            if (string.IsNullOrEmpty(errorMessage))
                success = true;

            return success;
        }
        #endregion
        #endregion
    }
}
