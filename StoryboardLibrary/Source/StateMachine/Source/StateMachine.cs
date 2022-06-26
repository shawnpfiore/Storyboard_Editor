#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StateMachine.cs
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
using ECS.Utilites.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryboardLibary
{
    public class StateMachine : IStateMachine
    {
        #region Fields / Variables
        /// <summary>
        /// Gets or Sets the current state
        /// </summary>
        public virtual IState CurrentState { get; set; }

        /// <summary>
        /// Gets or Sets the previous state
        /// </summary>
        public virtual IState PreviousState { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Stores a list of current states
        /// </summary>
        private List<IState> currentStates = new List<IState>();

        /// <summary>
        /// Stotres a list of previous states 
        /// </summary>
        private List<IState> prevStates = new List<IState>();

        /// <summary>
        /// Stores the name of the statemachine 
        /// </summary>
        private string name;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class
        /// </summary>
        /// <param name="Name">The name</param>
        public StateMachine(string Name)
        {
            this.name = Name;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Updates the state machine 
        /// </summary>
        public virtual void Update()
        {
            IStoryboardLibrary.CurrentState = this.CurrentState;

            var nextState = this.CurrentState?.Update();

            if (nextState != null)
                this.TransitionToState(nextState);
        }

        /// <summary>
        /// Adds a state to the state machine 
        /// </summary>
        /// <param name="state">The state to add</param>
        public virtual void AddState(IState state)
        {
            this.currentStates?.Add(state);

            if (this.currentStates == null)
                 ECSLogger.Instance.LogWarn($"{state} Attempting to Add a NULL state to a state machine");
        }

        /// <summary>
        /// Sets the current state
        /// </summary>
        /// <param name="state">The state to set</param>
        public virtual void SetCurrentState(IState state)
        {
            if(state != null)
            {
                ECSLogger.Instance.LogInfo($"Setting current state to {state.GetStateName()}");

                this.CurrentState = state;

                this.CurrentState?.OnEnterState();
            }
            else
            {
                ECSLogger.Instance.LogWarn($"{state} Attempting to Set a NULL state as an active state");
            }
        }

        /// <summary>
        /// Sets the previous state to the last one stored in the list 
        /// </summary>
        public virtual void PopPreviousState()
        {
            //Make sure we don't set states that don't exist
            if (this.prevStates.Any())
            {
                this.PreviousState = this.prevStates?.Last();

                if (this.PreviousState != null)
                    this.prevStates.Add(this.PreviousState);
                else
                    ECSLogger.Instance.LogInfo($"{this.PreviousState} No more previous states");
            }
        }

        /// <summary>
        /// Adds the target state to our list of previous states for history
        /// </summary>
        /// <param name="state"></param>
        public virtual void PushPreviousState(IState state)
        {
            if(state !=null)
            {
                this.prevStates?.Add(state);
                this.PreviousState = this.CurrentState; 
            }
            else
                ECSLogger.Instance.LogInfo($"{state} Attempting to add a NULL state to the previous state stack");
        }

        /// <summary>
        /// Gets the total number of current states
        /// </summary>
        /// <returns>The numbeer of states</returns>
        public virtual Int32 GetNumStates()
        {
            return currentStates.Count();
        }

        /// <summary>
        /// Gets the total number ot transitioins
        /// </summary>
        /// <returns>The nunmber of transitions</returns>
        public virtual Int32 GetNumTransition()
        {
            Int32 count = 0;

            this.currentStates?.ForEach(s =>
            {
                count += s.GetNumTransition();
            });

            return count; 
        }

        /// <summary>
        /// Gets the name of the state machine 
        /// </summary>
        /// <returns>The name</returns>
        public virtual string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Gets a state by id
        /// </summary>
        /// <param name="id">The state id</param>
        /// <returns>The state</returns>
        public virtual IState GetStateById(string id)
        {
            IState found = null;

            this.currentStates?.ForEach(s =>
            {
                if (s?.GetStateName() == id)
                    found = s;
            });

            return found; 
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Transitions to the next state 
        /// </summary>
        /// <param name="destinationState">The destination state</param>
        private void TransitionToState(IState destinationState)
        {
            if (destinationState != null)
            {
                ECSLogger.Instance.LogInfo($"Transitioning to state: {destinationState.GetStateName()} from state: {this.CurrentState?.GetStateName()}");

                // Call the exit callback for the old state
                this.CurrentState?.OnTransitionOut(destinationState);
                this.CurrentState = destinationState;
                // Call the entry callback for the next state
                this.CurrentState?.OnEnterState();
            }
        }
        #endregion
        #endregion
    }
}
