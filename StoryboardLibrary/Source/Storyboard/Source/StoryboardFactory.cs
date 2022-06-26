#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardFactory.cs
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
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    //////////////////////////////////////////////////////////////////////////
    /// Wrapper class that specializes it's wrapped factory to make Storyboard-specific nodes
    /// Keeps the user from having to use the ridiculous templated functions in the raw factory
    //////////////////////////////////////////////////////////////////////////
    public class StoryboardFactory
    {
        #region Fields / Variables
        /// <summary>
        /// Gets the state machine factory
        /// </summary>
        public StateMachineFactory FactoryRef { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardFactory"/> class
        /// </summary>
        /// <param name="factory">The state machine factory</param>
        public StoryboardFactory(StateMachineFactory factory)
        {
            this.FactoryRef = factory;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Factory function to create a start/end cue
        /// </summary>
        /// <param name="cueType">The cue type</param>
        /// <returns></returns>
        public IStoryboardNode CreateStartEndCue(ECueType cueType)
        {
            IStoryboardNode ret = null;
            if (cueType != ECueType.START_CUE || cueType != ECueType.END_CUE)
                ECSLogger.Instance.LogError("CreateStartEndCue Failed to create node, passed unhandle cue type");

            if (cueType == ECueType.START_CUE)
                ret = (IStoryboardNode)FactoryRef.CreateState<StartCue>("start");
            else if (cueType == ECueType.END_CUE)
                ret = (IStoryboardNode)FactoryRef.CreateState<EndCue>("start");
            return ret;
        }

        /// <summary>
        /// Factory function to create a unknown (error) cue
        /// </summary>
        /// <param name="id">The unknown cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreateUnknownCue(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<UnknownCue>(id, id);
        }

        /// <summary>
        /// Factory function to create a smart object cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreateSmartObjectCue(string id, ICondition finishedCondition)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<SmartObjectCue>(id, id, finishedCondition);
        }

        /// <summary>
        /// Factory function to create a play audio cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreatePlayAudioCue(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<PlayAudioCue>(id, id);
        }

        /// <summary>
        /// Factory function to create a snap to camera cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreateSnapToCue(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<SnapToCameraCue>(id, id);
        }

        /// <summary>
        /// Factory function to create a task step cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreateTaskStepNode(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<TaskStepNode>(id, id); 
        }

        /// <summary>
        /// Factory function to create a Delay cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode CreateDelayCue(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<DelayCue>(id, id);
        }

        /// <summary>
        /// Factory function to create highlight objects cue
        /// </summary>
        /// <param name="id">The cue id</param>
        /// <returns>The node</returns>
        public IStoryboardNode HighlightObjectsCue(string id)
        {
            return (IStoryboardNode)FactoryRef?.CreateState<HighlightObjectsCue>(id, id);
        }

        /// <summary>
        /// Factory function to create a storyboard property-based condition
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="value">The condition</param>
        /// <returns>The condition</returns>
        public ICondition CreatePropertyCondition(string name, bool value)
        {
            return FactoryRef?.CreateConditional<PropertyCondition>(name, value);
        }
        #endregion
        #endregion
    }
}
