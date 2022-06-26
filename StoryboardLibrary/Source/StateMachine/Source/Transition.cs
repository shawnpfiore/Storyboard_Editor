#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	Transition.cs
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
using ECS.Utilites.Logging;

namespace StoryboardLibary
{
    public class Transition : ITransition
    {
        #region Properties
        /// <summary>
        /// Stores the transitions destination 
        /// </summary>
        private IState destinationState;

        /// <summary>
        /// Stores the transition cndition
        /// </summary>
        private ICondition condition;

        /// <summary>
        /// Stores the transition source 
        /// </summary>
        private IState sourceState;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class
        /// </summary>
        /// <param name="SourceState">Transition source</param>
        /// <param name="DestinationState">Transition destination</param>
        /// <param name="Condition">Transition condition</param>
        public Transition(IState SourceState, IState DestinationState, ICondition Condition)
        {
            this.sourceState = SourceState;
            this.destinationState = DestinationState;
            this.condition = Condition; 
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Can the transition continue 
        /// </summary>
        /// <returns>True or false based on the condition</returns>
        public virtual bool CanTransition()
        {
            bool canTransition = false;

            if (this.condition != null)
                canTransition = this.condition.IsTrue();
            else
            canTransition = true;  //  We don't ever want to get 'stuck' in a node without a condition

            return canTransition;
        }

        /// <summary>
        /// Gets the transition destination
        /// </summary>
        /// <returns>The destination state</returns>
        public virtual IState GetDestinationState()
        {
            if (this.destinationState == null)
                ECSLogger.Instance.LogFatal($"{this.destinationState} Accessing transition without a valid destination state");

            return this.destinationState;
        }

        /// <summary>
        /// Gets the transition source state
        /// </summary>
        /// <returns>The source state</returns>
        public IState GetSourceState()
        {
            if(this.sourceState == null)
                ECSLogger.Instance.LogFatal($"{this.sourceState} Accessing transition without a valid source state");

            return this.sourceState;
        }
        #endregion
        #endregion
    }
}
