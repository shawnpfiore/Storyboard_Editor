#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardConfig.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	October 19, 2018
//  Last Update:    	October 19, 2018
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using ECS.Utilites.Logging;
using StoryboardContainer;
using System;
using LoadAppConfigs;

namespace StoryboardLibary
{
    public static class StoryboardConfig
    {
        #region Properties
        /// <summary>
        ///  Stores the loaded storybaord 
        /// </summary>
        public static Task storyboardContainer = null;

        /// <summary>
        /// Storyboard file
        /// </summary>
        private static string storyboardFile;
        public static string StoryboardFile
        {
            get { return storyboardFile; }
            set { storyboardFile = value; }
        }
        #endregion

        #region Fields/Variables
        /// <summary>
        /// Stores the LoadConfiguration obejct 
        /// </summary>
        private static LoadConfigurations loadStoryboardConfiguration = null;

        /// <summary>
        /// Stored the method info used for the logger
        /// </summary>
        private static System.Reflection.MethodBase methodInfo = null;

        /// <summary>
        /// Stores the method used for the logger
        /// </summary>
        private static string method { set; get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadStoryboardConfig"/> class
        /// </summary>
        public static bool LoadStoryboardConfig()
        {
            bool success = false;

            ECSLogger.Instance.Initialize(@".\data\loggerconfig.ini");
            ECSLogger.Instance.LogInfo("Loading storyboard file");

            loadStoryboardConfiguration = new LoadConfigurations();

            if (LoadSoryboard() == false)
                ECSLogger.Instance.LogWarn($"Failed to load storyboard file {storyboardFile}");
            else
                success = true; 

            return success;  
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Loads the storyboard xml
        /// </summary>
        private static bool LoadSoryboard()
        {
            bool success = false;
            string errorMessage = string.Empty; 

            methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            method = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

            try
            {
                //@".\Data\StoryboardContainer.xml"
                if (loadStoryboardConfiguration.LoadConfiguration<Task>(storyboardFile))
                    storyboardContainer = (Task)loadStoryboardConfiguration.Settings;
                else
                    errorMessage = $"Failed to load storyboard file {storyboardFile}";
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
