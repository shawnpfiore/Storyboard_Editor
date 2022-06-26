#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	NotifyPropertyChanged.cs
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace StoryboardUtils.BaseClasses
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the PropertyChangedEventHandler handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Invoke the NotifyChange event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        public void NotifyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }

    public class NotifyUserControl : UserControl, INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the PropertyChangedEventHandler for a usercontrol
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Invoke the NotifyChange event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected void NotifyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
