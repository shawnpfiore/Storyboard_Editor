#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStateMachine.cs
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

namespace StoryboardLibary
{
    public interface IStateMachine
    {
        /// <summary>
        /// Gets or Sets the current state the state machine is in
        /// </summary>
        IState CurrentState { get; set; }

        /// <summary>
        /// Gets or Sets the previous state the state machine is in
        /// </summary>
        IState PreviousState { get; set; }

        /// <summary>
        /// Updates the state machine, this is the only way to allow the 
        /// state machine to transition
        /// </summary>
        void Update();

        /// <summary>
        /// Adds a state to the state machine 
        /// </summary>
        /// <param name="state">The state to add</param>
        void AddState(IState state);

        /// <summary>
        /// Gets the name of the state macnine 
        /// </summary>
        /// <returns>State machine name</returns>
        string GetName();

        /// <summary>
        /// Sets the currently active state in the state machine.
        /// </summary>
        /// <param name="state">The active state</param>
        void SetCurrentState(IState state);

        /// <summary>
        /// Sets the previous active state in the state machine. 
        /// Based on wether we are moving forwards or backward in the history.
        /// </summary>
        /// <param name="state">The previous state</param>
        void PopPreviousState(); //sets the previous state to the last one stored on the list
        void PushPreviousState(IState state); //adds the target state to our list of previous states for history

        /// <summary>
        /// Gets the state by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>The States ID</returns>
        IState GetStateById(string Id);

        /// <summary>
        /// Gets the total number of states in the statemachine 
        /// </summary>
        /// <returns>The total number of states</returns>
        Int32 GetNumStates();

        /// <summary>
        /// Gets the total number of transitions in the statemachine 
        /// </summary>
        /// <returns></returns>
        Int32 GetNumTransition();

    }
}
