#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardTaskParser.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	December 20, 2018
//  Last Update:    	January 07, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using StoryboardContainer;
using StoryboardLibary.EventsDefinitions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class StoryboardTaskParser
    {
        #region Fields / Variables
        #endregion

        #region Properties
        /// <summary>
        /// Stores the storyboard task map 
        /// </summary>
        private Dictionary<string, IStoryboardNode> steps;

        /// <summary>
        /// Stores the storyboard lookuo map
        /// </summary>
        private Dictionary<string, ETaskType> lookupMap;
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Does a quick lookup in the map for the passed string, return UNKNOWN_TASK if the type is not found
        /// </summary>
        /// <param name="nodeName">The storyboard node name we are looking for</param>
        /// <returns>The cue type</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ETaskType GetEnum(string nodeName)
        {
            ETaskType type = ETaskType.UNKNOWN_TASK;

            if (lookupMap.ContainsKey(nodeName))
                lookupMap?.Keys?.ToList()?.ForEach(c => type = lookupMap[c]);

            return type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTaskParser"/> class
        /// </summary>
        public StoryboardTaskParser()
        {
            this.steps = new Dictionary<string, IStoryboardNode>();
            this.lookupMap = new Dictionary<string, ETaskType>();
            this.lookupMap.Add("StepData", ETaskType.STEP_DATA);
        }

        /// <summary>
        /// Parse the storyboard tasks
        /// </summary>
        /// <param name="nodes">The map of nodes</param>
        /// <param name="factory">The stroyboard Factory</param>
        public void ParseTask(Dictionary<string, IStoryboardNode> nodes, StoryboardFactory factory)
        {
            var container = StoryboardConfig.storyboardContainer;

            container?.TaskData?.Step?.ToList().ForEach(s =>
            {
                this.ParseStep(s, factory, "", nodes);
            });
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Parse the stroyboard steps
        /// </summary>
        /// <param name="step">The Step</param>
        /// <param name="factory">The factory</param>
        /// <param name="parentId">The parentId</param>
        /// <param name="nodes">The map of nodes</param>
        private void ParseStep(Step step, StoryboardFactory factory,
           string parentId, Dictionary<string, IStoryboardNode> nodes)
        {
            ETaskType taskType = ETaskType.UNKNOWN_TASK;

            TaskInfoEventArgs info = new TaskInfoEventArgs()
            {
                Id = step?.Id,
                TaskText = step?.StepData?.Text,
                ParentId = parentId,
                ObjectName = step?.StepData?.ObjectName,
                StepText = step?.StepData?.Text

            };

            taskType = this.GetEnum("StepData");

            if (taskType != ETaskType.UNKNOWN_TASK)
            {
                var created = this.CreateNode(step, taskType, info, factory);

                if(!nodes.ContainsKey(info.Id))
                    nodes.Add(info.Id, created);
            }
        }

        /// <summary>
        /// Create the task node
        /// </summary>
        /// <param name="step">The stpe</param>
        /// <param name="type">The tyoe of task. We only have one type at the moment</param>
        /// <param name="info">The Taskinfo event args</param>
        /// <param name="factory">The Storyboard factory</param>
        /// <returns></returns>
        private IStoryboardNode CreateNode(Step step, ETaskType type, TaskInfoEventArgs info,
            StoryboardFactory factory)
        {
            IStoryboardNode created = null;
            if (step != null && type != ETaskType.UNKNOWN_TASK)
            {
                switch (type)
                {
                    case ETaskType.STEP_DATA:
                        created = this.CreateTaskStepNode(step, info, factory);
                        break;
                    default:
                        created = this.CreateUnknownTask(step, info, factory);
                        break;
                }
            }

            return created;
        }

        /// <summary>
        /// Creates a unknown task 
        /// </summary>
        /// <param name="step">The step</param>
        /// <param name="info">The Taskinfo event args</param>
        /// <param name="factory"> The storyboard factory</param>
        /// <returns></returns>
        private IStoryboardNode CreateUnknownTask(Step step, TaskInfoEventArgs info,
            StoryboardFactory factory)
        {
            var unknownCue = (UnknownCue)factory.CreateUnknownCue(info?.Id?.ToString());
            return unknownCue;
        }

        /// <summary>
        ///  Creates a Task Step node
        /// </summary>
        /// <param name="step">The step</param>
        /// <param name="info">The TaskInfo event args</param>
        /// <param name="factory">The storyboard factory</param>
        /// <returns>A storyboard task step node</returns>
        private IStoryboardNode CreateTaskStepNode(Step step, TaskInfoEventArgs info,
           StoryboardFactory factory)
        {
            var taskStepNode = (TaskStepNode)factory.CreateTaskStepNode(info.Id);
            taskStepNode.TaskText = info?.TaskText;
            taskStepNode.StepDataObjectName = info?.ObjectName;
            taskStepNode.StepDataText = info?.StepText;

            return taskStepNode;
        }
        #endregion
        #endregion
    }
}
