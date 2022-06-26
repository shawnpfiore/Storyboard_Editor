#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IState.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	December 20, 2018
//  Last Update:    	January 08, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;

namespace StoryboardLibary
{
    public interface IState
    {
        /// <summary>
        /// Updates the state. The state will attempt to transition during this time
        /// If the state transitions, the reference parameter 'nextState' will be populated
        /// with the next valid state according to this state's transitions, else it will be NULL
        /// </summary>
        /// <returns> Handle containing the next state to transition too, or containing null if no transition is valid</returns>
        IState Update();

        /// <summary>
        /// Adds a transition to the state
        /// </summary>
        /// <param name="transition">A transition</param>
        void AddTransition(ITransition transition);

        /// <summary>
        /// Called whenever the state is entered
        /// </summary>
        void OnEnterState();

        /// <summary>
        /// Called whenever the state is exited
        /// </summary>
        /// <param name="nextStep">The next step</param>
        void OnTransitionOut(IState nextStep);

        /// <summary>
        /// Gets the name of the state
        /// </summary>
        /// <returns>State name</returns>
        string GetStateName();

        /// <summary>
        /// Gets the description of the state
        /// </summary>
        /// <returns>Description</returns>
        string GetStateDescription();

        /// <summary>
        /// Gets the number of transitions in the state
        /// </summary>
        /// <returns>Number of transitions</returns>
        Int32 GetNumTransition();
    }
}
