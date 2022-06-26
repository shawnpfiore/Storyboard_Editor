#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStoryboardNode.cs
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
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public interface IStoryboardNode
    {
        /// <summary>
        /// Gets and Sets the node type
        /// </summary>
        ENodeType nodeType { get; set; }

        /// <summary>
        /// Gets the node id
        /// </summary>
        /// <returns>Retuns the node id</returns>
        string GetId();

        /// <summary>
        /// Gets the node type
        /// </summary>
        /// <returns>The node type</returns>
        ENodeType GetNodeType();

        /// <summary>
        /// Gets the niode condition
        /// </summary>
        /// <returns>The condition</returns>
        ICondition GetCondition(); 

        /// <summary>
        /// Automates the storyboard
        /// </summary>
        void AutomateNode();
    }
}
