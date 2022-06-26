#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ITransition.cs
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
namespace StoryboardLibary
{
    public interface ITransition
    {
        /// <summary>
        /// Checks if this tranistion is allowed in it's current state
        /// </summary>
        /// <returns>True if this transition is valid</returns>
        bool CanTransition();

        /// <summary>
        /// Gets this transitions destination state
        /// </summary>
        /// <returns>The destination state for this transition</returns>
        IState GetDestinationState();

        /// <summary>
        /// Gets this transitions source state
        /// </summary>
        /// <returns>The source state for this transition</returns>
        IState GetSourceState();
    }
}
