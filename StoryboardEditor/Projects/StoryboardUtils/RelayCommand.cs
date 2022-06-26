#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	RelayCommand.cs
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
using System;
using System.Windows.Input;

namespace StoryboardUtils 
{
    public class RelayCommand : ICommand
    {
        #region Fields / Variables
        /// <summary>
        /// Stores the execute action
        /// </summary>
        private Action<object> executeFunc;

        /// <summary>
        /// Stores the results  
        /// </summary>
        private Func<bool> canExecuteFunc;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        public RelayCommand(Action<object> executeFunc, Func<bool> canExecuteFunc)
        {
            this.executeFunc = executeFunc;
            this.canExecuteFunc = canExecuteFunc;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Can Execute command
        /// </summary>
        /// <param name="parameter">The passed in argument</param>
        /// <returns>True or False</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecuteFunc();
        }

        // By explicitly putting this add and remove here, we make it clear that we are going to do nothing with this event.
        // If anyone registers to this event, we just discard it and do nothing.
        // We could throw a NotSupportedException here if we wanted to as well, but I think something internally in WPF might try to use it and we don't want exceptions all over
        public event EventHandler CanExecuteChanged { add { } remove { } }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter">The incoming command</param>
        public void Execute(object parameter)
        {
            executeFunc(parameter);
        }
        #endregion
        #endregion
    }
}
