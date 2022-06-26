#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SearchManager.cs
//  Author:            	Jonathan Ramos
//  Creation Date:  	March 19, 2019
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
using System.Windows.Controls;
using System.Collections.Generic;

namespace StoryboardEditor.Managers
{
    public static class SearchManager
    {
        #region Static Private Variables
        private static MainWindow mainRef = null;
        private static UInt32 AmountFound = 0;
        private static UInt32 CurrentIndex = 0;
        private static List<String> CueIdList = null;
        #endregion
        #region Static Public Functions
        /// <summary>
        /// Instantiates the SearchManager by passing in the "Slave" MainWindow.
        /// </summary>
        /// <param name="window"></param>
        public static void Instantiate(ref MainWindow window)
        {
            CueIdList = new List<String>();
            mainRef = window;
            mainRef.SearchInput_TextBox.TextChanged += SearchInput_TextBox_TextChanged;
            mainRef.UpButton.Click += UpButton_Click;
            mainRef.DownButton.Click += DownButton_Click;
            mainRef.button_CloseSearch.Click += Button_CloseSearch_Click;
            DisableSearch();
        }

        /// <summary>
        /// Makes the Search Window Visible.
        /// </summary>
        public static void EnableSearch()
        {
            mainRef.SearchInput_TextBox.Text = "";
            Search();
            mainRef.searchGroupBox.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Moves the ScrollView to the Cue location passed in.
        /// </summary>
        /// <param name="index"></param>
        public static void MoveTo(int index)
        {
            ScrollViewer viewer = TreeViewManager.GetScrollViewer();

            if (index == 0)
            {
                viewer.ScrollToTop();
                return;
            }

            if (index == ValidationManager.Tracker.Count)
            {
                viewer.ScrollToBottom();
                return;
            }

            if (index == 1)
            {
                viewer.ScrollToVerticalOffset(280);
                return;
            }
            else
            {
                int amount = 280 + ((index - 1) * 310);
                viewer.ScrollToVerticalOffset(amount);
            }
        }
        #endregion
        #region Static Private Functions
        /// <summary>
        /// Hides the Search Window
        /// </summary>
        private static void DisableSearch()
        {
            mainRef.searchGroupBox.Visibility = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// Search logic for the search (Ctrl + F) functionality.
        /// </summary>
        private static void Search()
        {
            AmountFound = ValidationManager.AmountOfHits(mainRef.SearchInput_TextBox.Text);

            if (AmountFound > 0)
            {
                if (CurrentIndex == uint.MaxValue)
                    CurrentIndex = AmountFound - 1;
                else
                    CurrentIndex %= AmountFound;

                mainRef.amountLabel.Text = (CurrentIndex + 1).ToString() + " / " + AmountFound.ToString();

                CueIdList = ValidationManager.GetIdsThatContainString(mainRef.SearchInput_TextBox.Text);
                MoveTo(ValidationManager.GetIndex(CueIdList[(int)CurrentIndex]));
            }
            else
                mainRef.amountLabel.Text = "0";
        }

        /// <summary>
        /// Up Arrow Logic
        /// </summary>
        private static void MoveUp()
        {
            CurrentIndex--;
            Search();
        }

        /// <summary>
        /// Down Arrow Logic
        /// </summary>
        private static void MoveDown()
        {
            CurrentIndex++;
            Search();
        }
        #endregion
        #region Events
        private static void SearchInput_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private static void DownButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveDown();
        }

        private static void UpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveUp();
        }

        private static void Button_CloseSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DisableSearch();
        }
        #endregion
    }
}
