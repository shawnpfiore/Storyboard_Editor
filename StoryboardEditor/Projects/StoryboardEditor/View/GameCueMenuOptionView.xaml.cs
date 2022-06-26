#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	GameCueMenuOptionView.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 18, 2019
//  Last Update:    	January 18, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using ECS.Utilites.Logging;
using StoryboardEditor.Model;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace StoryboardEditor.View
{
    /// <summary>
    /// Interaction logic for CameCueView.xaml
    /// </summary>
    public partial class GameCueMenuOptionView : UserControl
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCueMenuOptionView"/> class.
        /// </summary>
        /// <param name="title">The control title</param>
        public GameCueMenuOptionView(string title, bool buttonEnabled)
        {
            this.InitializeComponent();

            GameCueMenuModel gameCueModel = this.DataContext as GameCueMenuModel;

            if (gameCueModel != null)
            {
                gameCueModel.Name = title;

                gameCueModel.EnableAddButton = buttonEnabled;
                               
                try
                {
                    this.CueImage.Source = (ImageSource)FindResource(title); ;
                }
                catch (Exception e)
                {
                    ECSLogger.Instance.LogError(e.Message);
                }
            }
        }
        #endregion
    }
}
