#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	IStoryboardEvents.cs
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
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary.EventsDefinitions
{
    /// <summary>
    /// State changed event args.
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        public string NewStateId { get; set; }
        public string OldStateId { get; set; }
        public ENodeType NewStateType { get; set; }
    };

    /// <summary>
    /// TaskInfo event args.
    /// </summary>
    public class TaskInfoEventArgs : EventArgs
    {
        public string Id { get; set; }
        public string TaskText { get; set; }
        public string ParentId { get; set; }
        public string ObjectName { get; set; }
        public string StepText { get; set; }
    };

    /// <summary>
    /// Storyboard root node event args.
    /// </summary>
    public class StoryboardLoadedEventArgs : EventArgs
    {
        public string StoryboardName { get; set; }
        public string StoryboardTitle { get; set; }
        public string Date { get; set; }
        public string Cinematic { get; set; }
        public string StoryboardToolVersion { get; set; }
        public EStoryboardType StoryboardType { get; set; }
    };

    /// <summary>
    /// Load storyboard event args.
    /// </summary>
    public class LoadStoryboardEventArgs : EventArgs
    {
        public string StoryboardName { get; set; }

        public EStoryboardType storyboardType;
    };

    /// <summary>
    /// Automate storyboard event args. 
    /// </summary>
    public class AutomateStoryboardEventArgs : EventArgs
    {
        public string storyboardPath { get; set; }
    };

    ///////////////////////////////Start Game Cue Events//////////////////////////

    /// <summary>
    /// Active smartobject cue event args. 
    /// </summary>
    public class ActivateSmartObjectCueEventArgs : EventArgs
    {
        public string ObjectName { get; set; }
        public string Highlight { get; set; }
    };

    /// <summary>
    /// Actiave PlayAudio cue event args.
    /// </summary>
    public class ActivePayAudioCueEventArgs : EventArgs
    {
        public string AudioClip { get; set; }
        public string Caption { get; set; }
    }

    /// <summary>
    /// Active SnapTo cue event. 
    /// </summary>
    public class ActiveSnapToCueEventArgs : EventArgs
    {
        public List<string> Objects { get; set; }
        public List<float> Delay { get; set; }
    };

    /// <summary>
    /// Active TaskStep node event args. 
    /// </summary>
    public class ActiveTaskStepCueEventArgs : EventArgs
    {
        public string TaskText { get; set; }
        public string StepDataText { get; set; }
        public string StepDataObjectName { get; set; }
    }

    public class ActiveDelayCueEventArgs : EventArgs
    {
        public float Delay { get; set; }
    }

    public class ActiveHighlightObjectsCueEventArgs : EventArgs
    {
        public List<string> ObjectNames { get; set; }
        public List<string>Highlights { get; set; }
    }
    ///////////////////////////////End Game Cue Events//////////////////////////

}
