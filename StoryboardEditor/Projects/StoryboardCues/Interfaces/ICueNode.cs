#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ICueNode.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 20, 2019
//  Last Update:    	January 21, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardCues.Interfaces
{
    public interface ICueNode
    {
        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        ENodeType NodeType { get; set; }

        /// <summary>
        /// Gets or Sets the cue id
        /// </summary>
        string CueId { get; set; }

        /// <summary>
        /// Gets or Sets the cue name
        /// </summary>
        string CueName { get; set; }

        /// <summary>
        /// Gets or Sets the is completed flag
        /// </summary>
        bool IsComplete { get; set; }

        /// <summary>
        /// Generates a unique cue id 
        /// </summary>
        void GenerateNewCueId();
    }
}
