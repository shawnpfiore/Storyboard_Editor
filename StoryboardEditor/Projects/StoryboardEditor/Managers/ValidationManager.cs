#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ValidationManager.cs
//  Author:            	Jonathan Ramos
//  Creation Date:  	March 13, 2019
//  Last Update:    	March 20, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion

using System;
using StoryboardCues.Model;
using System.Collections.Generic;

namespace StoryboardEditor.Managers
{
    public static class ValidationManager
    {
        // This is a dud

        #region Static Variables
        /// <summary>
        /// Keeps track of all the cues.
        /// </summary>
        public static List<Object> Tracker = new List<Object>();
        #endregion
        #region Static Public Functions
        /// <summary>
        /// Verifies if all cues are completed.
        /// </summary>
        /// <returns></returns>
        public static Boolean CuesComplete()
        {
            // Go through all the items and if there is one that is not complete then we return false.
            foreach (System.Windows.FrameworkElement item in Tracker)
                if (!((CueBaseModel)item.DataContext).IsComplete)
                    return false;

            return true;
        }

        /// <summary>
        /// Return the ID of the first Incomplete Cue.
        /// If it can't find an Incomplete Cue then it returns an empty string.
        /// </summary>
        /// <returns>CueID</returns>
        public static String GetIDOfIncompleteCue()
        {
            // Go through all the items and if there is one that is not complete we return that items ID.
            foreach (System.Windows.FrameworkElement item in Tracker)
                if (!((CueBaseModel)item.DataContext).IsComplete)
                    return ((CueBaseModel)item.DataContext).CueId;

            return "";
        }

        /// <summary>
        /// Returns true if there is a match between the cues and the string passed in.
        /// </summary>
        /// <param name="input">String to be checked.</param>
        /// <returns></returns>
        public static Boolean Contains(String input)
        {
            // Goes through all items and checks if their contents contain the strings we're looking for.
            foreach (System.Windows.FrameworkElement item in Tracker)
                foreach (String str in ((CueBaseModel)item.DataContext).CueContents)
                    if (str.ToLower().Contains(input.ToLower()))
                        return true;

            return false;
        }

        /// <summary>
        /// Returns a list of matched strings.
        /// </summary>
        /// <param name="input">String to be searched.</param>
        /// <returns></returns>
        public static List<String> GetStrings(String input)
        {
            List<String> strings = new List<String>();

            // Goes through all items and checks if their contents contain the strings we're looking for.
            foreach (System.Windows.FrameworkElement item in Tracker)
                foreach (String str in ((CueBaseModel)item.DataContext).CueContents)
                    if (str.ToLower().Contains(input.ToLower()))
                        strings.Add(str);

            return strings;
        }

        /// <summary>
        /// Returns a list of CueIds that have a hit on the string passed in.
        /// </summary>
        /// <param name="input">String to be searched.</param>
        /// <returns></returns>
        public static List<String> GetIdsThatContainString(String input)
        {
            List<String> Ids = new List<String>();

            // Goes through all items and checks if their contents contain the strings we're looking for.
            foreach (System.Windows.FrameworkElement item in Tracker)
            {
                foreach (String str in ((CueBaseModel)item.DataContext).CueContents)
                {
                    if (str.ToLower().Contains(input.ToLower()))
                    {
                        // Found who contains it, we can go to the next Cue now.
                        Ids.Add(((CueBaseModel)item.DataContext).CueId);
                    }
                }
            }

            return Ids;
        }

        /// <summary>
        /// Returns the Index of the CueId passed in.
        /// </summary>
        /// <param name="Id">CueId</param>
        /// <returns>Index of Cue</returns>
        public static int GetIndex(String Id)
        {
            for (int i = 0; i < Tracker.Count; i++)
            {
                System.Windows.FrameworkElement item = (System.Windows.FrameworkElement) Tracker[i];
                CueBaseModel cue = ((CueBaseModel)item.DataContext);
                if (cue.CueId == Id)
                    return i;
            }
            return 0;
        }

        /// <summary>
        /// Returns the amount of hits of the string passed in.
        /// </summary>
        /// <param name="input">String to be searched.</param>
        /// <returns></returns>
        public static uint AmountOfHits(String input)
        {
            if (input == "") return 0;
            return (uint)GetStrings(input).Count;
        }
        #endregion
    }
}
