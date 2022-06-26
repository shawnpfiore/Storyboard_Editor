#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	CacheDataManager.cs
//  Author:            	Jonathan Ramos
//  Creation Date:  	February 14, 2019
//  Last Update:    	March 14, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System;
using System.IO;
using System.Windows;

namespace StoryboardEditor.Managers
{
    public static class CacheDataManager
    {
        #region Variables
        private static string savedFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + @"\Testfile.Backup";
        #endregion

        #region Public Static Functions

        /// <summary>
        /// Writes the Storyboard Cues out to the cache file
        /// </summary>
        public static void WriteCacheFile()
        {
            // Show the cache file, in brackets so tempFile goes out of scope and releases the file.
            {
                FileInfo tempFile = new FileInfo(savedFilePath);
                if (tempFile.Exists) tempFile.Attributes &= ~FileAttributes.Hidden;
            }

            // Write to cache file
            StoryboardFileIO.FileIO.SaveFileTo(StoryboardCanvasUtils.CanvasGameCues, true, savedFilePath);

            // Hide the cache file
            new FileInfo(savedFilePath).Attributes |= FileAttributes.Hidden;

            TreeViewManager.XMLUpdate();
        }

        /// <summary>
        /// Checks to see if the cache file exists.
        /// </summary>
        /// <returns></returns>
        public static bool FileExists()
        {
            return (File.Exists(savedFilePath));
        }

        /// <summary>
        /// Promps user if they would like to restore their last session. 
        /// If the user selects NO, the cache file is deleted.
        /// </summary>
        /// <param name="storyboardCues"></param>
        public static void RecoverFromCacheFile()
        {
            // If file exists on startup prompt to restore from last session.
            if (FileExists())
            {
                // Prompt the user if they want to restore from last session.
                MessageBoxResult result = MessageBox.Show("Would you like to restore from last session?", "Cache file found", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    StoryboardFileIO.FileIO.OpenFileFrom(StoryboardCanvasUtils.CanvasGameCues, savedFilePath);
                else
                    DeleteCacheFile();
            }
        }

        /// <summary>
        /// Deletes the cache data file if it exists.
        /// </summary>
        public static void DeleteCacheFile()
        {
            if (FileExists())
                File.Delete(savedFilePath);
        }

        /// <summary>
        /// returns the cache file path as a string.
        /// </summary>
        /// <returns>cacheFilePath</returns>
        public static string GetFileLocation()
        {
            return savedFilePath;
        }
        #endregion
    }
}
