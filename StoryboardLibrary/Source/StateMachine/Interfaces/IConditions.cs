#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ICondition.cs
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
    public interface ICondition
    {
        /// <summary>
        /// Evaluates the condition
        /// </summary>
        /// <returns>True if the condition is true</returns>
        bool IsTrue();

        /// <summary>
        /// Resets the condition to false
        /// </summary>
        void Reset();

        /// <summary>
        /// Sets the active condition 
        /// </summary>
        /// <param name="isActive">True or False on the condition</param>
        void SetActive(bool isActive);
    }
}
