#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StateMachineFactory.cs
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
using System.Linq;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    ///  This factory class is used to create state machine components.
    ///  It owns ALL components that it creates,
    ///  returned by this class. 
    ///  Best usage would be to keep 1 factory for each state machine in scope
    //////////////////////////////////////////////////////////////////////////
    public class StateMachineFactory
    {
        #region Fields / Variables
        /// <summary>
        /// Gets or Sets a list of IStates 
        /// </summary>
        public List<IState> StateMap { get; private set; }

        /// <summary>
        /// Gets or Sets a list of IConditions 
        /// </summary>
        public List<ICondition> conditionMap { get; private set; }

        /// <summary>
        /// Gets or Sets a list of IStateMachines
        /// </summary>
        public List<IStateMachine> StateMachineMap { get; private set; }

        /// <summary>
        /// Gets or Sets a list of ITranstions
        /// </summary>
        public List<ITransition> TransitionMap { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineFactory"/> class
        /// </summary>
        public StateMachineFactory()
        {
            this.StateMap = new List<IState>();
            this.StateMachineMap = new List<IStateMachine>();
            this.TransitionMap = new List<ITransition>();
            this.conditionMap = new List<ICondition>();
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Creates a state 
        /// </summary>
        /// <param name="stateName">The state name</param>
        /// <returns>The created state</returns>
        public IState CreateState(string stateName)
        {
            StateMap?.Add(new BaseState(stateName));

            return StateMap?.Last();
        }

        /// <summary>
        /// Creates a state 
        /// </summary>
        /// <param name="stateName">The state name</param>
        /// <param name="stateDescription">The state description</param>
        /// <returns>The created state</returns>
        public IState CreateState(string stateName, string stateDescription)
        {
            StateMap?.Add(new BaseState(stateName, stateDescription));

            return StateMap?.Last();
        }

        /// <summary>
        /// Ceates a state object with [] constructor params.
        /// The template parameter<T> must be a type derived from IState.
        /// 
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="stateName">The state name</param>
        /// <param name="args">The prams</param>
        /// <returns>The created state</returns>
        public IState CreateState<T>(string stateName, params object[] args) where T : IState
        {
            this.StateMap?.Add((T)Activator.CreateInstance(typeof(T), args));

            return this.StateMap?.Last();
        }

        /// <summary>
        /// Ceates a condition object with [] constructor params.
        /// The template parameter<T> must be a type derived from ICondition.
        ///
        /// This function creates the condition using the format new T()
        /// </summary>
        /// <typeparam name="T">The Type to create</typeparam>
        /// <param name="conditionName">The condition name</param>
        /// <param name="args">The forward arguments</param>
        /// <returns>The created condition</returns>
        public ICondition CreateConditional<T>(string conditionName, params object[] args) where T : ICondition
        {
            this.conditionMap?.Add((T)Activator.CreateInstance(typeof(T), args));
            return this.conditionMap?.Last();
        }
        /// <summary>
        /// Creates a state machine 
        /// </summary>
        /// <param name="stateMachineName">The statemachine name</param>
        /// <returns>The created state machine</returns>
        public IStateMachine CreateStateMachine(string stateMachineName)
        {
            this.StateMachineMap?.Add(new StateMachine(stateMachineName));

            return this.StateMachineMap?.Last();
        }

        /// <summary>
        /// Creates a transition
        /// </summary>
        /// <param name="transitionName">The name</param>
        /// <param name="source">The source</param>
        /// <param name="destination">The destination</param>
        /// <returns>The created trnasition</returns>
        public ITransition CreateTransition(string transitionName,
          IState source, IState destination, ICondition conditional)
        {
            this.TransitionMap?.Add(new Transition(source, destination, conditional));

            return this.TransitionMap?.Last();
        }

        #endregion
        #endregion
    }
}
