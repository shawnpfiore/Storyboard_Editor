#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStorybard.cs
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
using StoryboardLibary.EventsDefinitions;
using System;
using System.Collections.Generic;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    /// Represents a loaded storyboard file.
    /// Owns the factory, and as a result all child state objects. Contains
    /// accessors for all information the game will need.
    //////////////////////////////////////////////////////////////////////////
    public interface IStoryboard
    {
        /// <summary>
        /// Parses and creates the storybaord 
        /// </summary>
        /// <param name="storyboardFile">The stroyboard file</param>
        /// <param name="type">Storyboard type</param>
        /// <returns>True if loaded</returns>
        bool Load(string storyboardFile, EStoryboardType type);

        /// <summary>
        /// Gets the stroyboard name
        /// </summary>
        /// <returns>The name of the storyboard</returns>
        string GetStoryboardName();

        /// <summary>
        /// Gets the storyboard file 
        /// </summary>
        /// <returns>The storyboard file</returns>
        string GetStoryboardFile();

        /// <summary>
        /// Gets the current state of the stroybord. Definitions below 
        /// STORYBOARD_UNINITIALIZED, Storyboard created but not loaded
        /// STORYBOARD_ACTIVE, Storyboard loaded successfully and ready to update
        /// STORYBOARD_LOAD_FAILED, Failed to load the storyboard file, error state
        /// </summary>
        /// <returns>Returns the current state</returns>
        EStoryboardState GetState();

        /// <summary>
        /// Gets the current active node for the storyboard. Use IStoryboardNode::GetNodeType and cast
        /// to either IStoryboardTask or IStoryboardCue for more detailed information
        /// </summary>
        /// <returns>The found node</returns>
        IStoryboardNode GetCurrentNode();

        /// <summary>
        /// Gets all the taks from the stroybaord. Fills a list with the task info
        /// from the internal task nodes.This list will be in the correct order for the storyboard
        /// 
        /// Note: This is an expensive call as it involves walking the entire storyboard.
        /// </summary>
        /// <param name="output">The taksinfo</param>
        /// <returns>The number of tasks in the array</returns>
        Int32 GetAllTasks(List<TaskInfoEventArgs> output);

        /// <summary>
        ///  Gets the tasked info for the passed task nake
        /// </summary>
        /// <param name="taskName">The name of the task</param>
        /// <param name="success">True if successfull</param>
        /// <returns></returns>
        TaskInfoEventArgs GetTaskInfo(string taskName);

        /// <summary>
        /// Gets the current task if there is one
        /// </summary>
        /// <param name="success">True if found one</param>
        /// <returns>The current task</returns>
        TaskInfoEventArgs GetCurrentTask();

        /// <summary>
        /// Gets the task info for the current task (if there is one)
        /// </summary>
        /// <param name="success">True if there is one </param>
        /// <returns>Name of the target object</returns>
        string GetCurrentTargetObject();

        /// <summary>
        /// Updates he storyboard
        /// </summary>
        void Update();

        /// <summary>
        /// Stars the storyboard
        /// </summary>
        /// <returns>Ture or False</returns>
        bool StartStoryboard();

        /// <summary>
        /// Stops the storyboard
        /// </summary>
        /// <returns>True or False</returns>
        bool StopStoryboard();

        /// <summary>
        /// Pauses the storyboard
        /// </summary>
        void PauseStoryboard();

        /// <summary>
        /// Resumes the storyboard
        /// </summary>
        void ResumeStoryboard();

        /// <summary>
        /// Resets the storyboard
        /// </summary>
        /// <returns></returns>
        bool Reset();

        /// <summary>
        /// Skips forwards through the storyboard
        /// </summary>
        void SkipForwards();

        /// <summary>
        /// Skips back through the storyboard
        /// </summary>
        void SkipBack();

        /// <summary>
        /// Automates the storyboard
        /// </summary>
        /// <param name="path">The storyboard path</param>
        void AutomateStoryboard();

    }

}
