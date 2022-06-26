#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardCanvasView.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 18, 2019
//  Last Update:    	February 21, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System;
using System.Windows.Controls;
using StoryboardEditor.Managers;

namespace StoryboardEditor.View
{
    /// <summary>
    /// Interaction logic for StoryboardCanvas.xaml
    /// </summary>
    public partial class StoryboardCanvasView : UserControl
    {
        /// <summary>
        /// Sets the auto scroll to enabled
        /// </summary>
        private Boolean autoScroll = true;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCanvasView"/> class.
        /// </summary>
        public StoryboardCanvasView()
        {
            this.InitializeComponent();
            TreeViewManager.Instantiate(ref ScrollViewer);
        }
        #endregion

        /// <summary>
        /// Auto scroll event 
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event name</param>
        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    autoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    autoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
            }
        }
    }
}
