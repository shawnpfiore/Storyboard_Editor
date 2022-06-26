#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	BaseState.cs
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
    public class BaseState : IState
    {
        #region Properties
        /// <summary>
        /// Stores the state name 
        /// </summary>
        protected string stateName;

        /// <summary>
        /// Stores the state description
        /// </summary>
        protected string stateDescription;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseState"/> class
        /// </summary>
        /// <param name="StateName">State name </param>
        public BaseState(string StateName)
        {
            this.stateName = StateName;
            this.stateDescription = StateName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseState"/> class
        /// </summary>
        /// <param name="StateName">State name </param>
        /// <param name="StateDescription">State description</param>
        public BaseState(string StateName, string StateDescription)
        {
            this.stateName = StateName;
            this.stateDescription = StateDescription;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Updates to the next treansition 
        /// </summary>
        /// <returns>The next state</returns>
        public virtual IState Update()
        {
            bool canTransition = false;
            IState nextState = null;

            if (this.stateName == "start")
            {
                // Start node will always make the firat transition
                nextState = IStoryboardLibrary.TransitionArray?.FirstOrDefault()?.GetDestinationState();
            }
            else
            {
                var transition = IStoryboardLibrary.TransitionArray?.Find(t => t.GetSourceState() == IStoryboardLibrary.CurrentState);

                if (transition != null)
                {
                    canTransition = transition.CanTransition();
                    if (canTransition == true)
                        nextState = transition?.GetDestinationState();
                }
            }

            return nextState;
        }

        /// <summary>
        /// Adds a transition to the list 
        /// </summary>
        /// <param name="transition">A transition</param>
        public virtual void AddTransition(ITransition transition)
        {
            if (transition != null)
                // TODO 
                IStoryboardLibrary.TransitionArray?.Add(transition);
            else
                ECSLogger.Instance.LogWarn($"{transition} Attempting to add a nullptr transition to a state");
        }

        /// <summary>
        /// Gets the state name 
        /// </summary>
        /// <returns>The state name</returns>
        public virtual string GetStateName()
        {
            return this.stateName;
        }

        /// <summary>
        /// Gets the state description
        /// </summary>
        /// <returns>The description</returns>
        public virtual string GetStateDescription()
        {
            return this.stateDescription;
        }

        /// <summary>
        /// Gets the number of trnasitions 
        /// </summary>
        /// <returns>The nimber of transitions</returns>
        public virtual Int32 GetNumTransition()
        {
            //TODO
            return IStoryboardLibrary.TransitionArray.Count;
        }

        /// <summary>
        /// Interface implimentation do nothing 
        /// </summary>
        public virtual void OnEnterState() { }

        /// <summary>
        /// Interface implimentation do nothing 
        /// </summary>
        public virtual void OnTransitionOut(IState nextStep) { }
        #endregion
        #endregion
    }
}
