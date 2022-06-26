#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	SpecialCues.cs
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
    public class StartCue : NodeBase
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="StartCue"/> class
        /// Start cue this cue is not visible in the xml 
        /// was designed to be manually pushed into a cue for a "start" position
        /// </summary>
        public StartCue() : base("start")
        {
            this.nodeType = ENodeType.GAMECUE_NODE_TYPE;
        }

        /// <summary>
        /// Call back for the on entered state
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

            EventManager.Instance.TriggerEvent(EventListeners.STORYBOARD_STARTED_EVENT.ToString());
        }

        /// <summary>
        /// Automate the storyboard 
        /// </summary>
        public override void AutomateNode()
        {
            base.AutomateNode();
        }
    }
    public class EndCue : NodeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndCue"/> class
        /// End cue this cue is not visible in the xml 
        /// was designed to be manually pushed into a cue for a "end" position
        /// </summary>
        public EndCue() : base("end")
        {
            this.nodeType = ENodeType.GAMECUE_NODE_TYPE;
        }

        /// <summary>
        /// Call back for the on entered state
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

            EventManager.Instance.TriggerEvent(EventListeners.STORYBOARD_FINISHED_EVENT.ToString());
        }

        /// <summary>
        /// Automater the storyboard
        /// </summary>
        public override void AutomateNode()
        {
            base.AutomateNode();
        }
    }

    public class UnknownCue : NodeBase
    {
        /// Initializes a new instance of the <see cref="UnknownCue"/> class
        /// Unknown cue this cue is not visible in the xml 
        /// was designed to identifity an unknown cue in the xml
        /// <param name="id">The id</param>
        public UnknownCue(string id, EventManager eventSys) : base("Unknown cue")
        {
            this.nodeType = ENodeType.UNKNOWN_NODE_TYPE;
        }

        /// <summary>
        /// Call back for the on entered state
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

            EventManager.Instance.TriggerEvent(EventListeners.UNKNOWN_EVENT.ToString());
        }
    }

}
