#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardParser.cs
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
using ECS.Utilites.Logging;
using StoryboardLibary.EventsDefinitions;
using System.Collections.Generic;
using System.Linq;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class StoryboardParser
    {
        #region Fields / Variables
        #endregion

        #region Properties
        /// <summary>
        /// Stores the  map of parsed nodes 
        /// </summary>
        private Dictionary<string, IStoryboardNode> parsedNodes = new Dictionary<string, IStoryboardNode>();

        /// <summary>
        /// Stores the list of parsed transitions 
        /// </summary>
        private List<ParsedTransition> parsedTransitions = new List<ParsedTransition>();

        /// <summary>
        /// Stores the instance of the storyboard
        /// </summary>
        private StoryboardInstance stroyboard = null;

        /// <summary>
        /// Stores the instance of the Storyboard factory
        /// </summary>
        private StoryboardFactory sbFactory = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardParser"/> class
        /// </summary>
        /// <param name="sbInstance">Storyboard instance</param>
        public StoryboardParser(StoryboardInstance sbInstance)
        {
            this.stroyboard = sbInstance;
            this.sbFactory = sbInstance.SbFactory;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Parse the storyboard xml file 
        /// </summary>
        /// <param name="filename">The filename to parse</param>
        /// <param name="outInfo">The storyboard loaded event args</param>
        /// <param name="type">The storyboard type</param>
        /// <returns>The state machine</returns>
        public IStateMachine Parse(string filename, StoryboardLoadedEventArgs outInfo, EStoryboardType type)
        {
            StoryboardConfig.StoryboardFile = filename;
            ECSLogger.Instance.LogInfo($"Parsing Storyboard {filename}");
            IStateMachine cretedSm = null;

            if (StoryboardConfig.LoadStoryboardConfig() == true)
            {
                var container = StoryboardConfig.storyboardContainer;

                var title = container?.Title;
                var shortname = container?.ShortName;

                if (type == EStoryboardType.ROOT_STORYBOARD)
                {
                    outInfo.Cinematic = container?.CinematicLevel;
                    outInfo.Date = container?.Date;
                    outInfo.StoryboardToolVersion = container?.StoryboardToolVersion;
                }

                StoryboardCueParser cueParser = new StoryboardCueParser();
                StoryboardTaskParser taskParser = new StoryboardTaskParser();
                StoryboardTransitionParser transitionParser = new StoryboardTransitionParser();

                taskParser.ParseTask(this.parsedNodes, this.sbFactory);
                cueParser.ParseGameCues(this.parsedNodes, this.sbFactory);
                this.parsedTransitions = transitionParser.ParseTransition(this.sbFactory);

                // Shove in the start & end cues, since they aren't explicitly in the file
                this.MapNode(this.sbFactory?.CreateStartEndCue(ECueType.START_CUE));
                this.MapNode(this.sbFactory?.CreateStartEndCue(ECueType.END_CUE));

                cretedSm = this.Assemble();
#if DEBUG
                // Sould also make to proccess this on the build server
                this.ValidateStoryboard();
#endif
            }

            return cretedSm;
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Create a map of nodes 
        /// </summary>
        /// <param name="node">A storyboard node</param>
        private void MapNode(IStoryboardNode node)
        {
            if (node != null)
            {
                if (!this.parsedNodes.ContainsKey(node.GetId()))
                    this.parsedNodes[node.GetId()] = node;
            }
        }

        /// <summary>
        /// Validates a storyboard through valid trasitions 
        /// </summary>
        private void ValidateStoryboard()
        {
            Dictionary<string, IStoryboardNode> usedNodes = new Dictionary<string, IStoryboardNode>();

            ECSLogger.Instance.LogInfo("Validating Storyboard...");
            ECSLogger.Instance.LogInfo("====================================");
            ECSLogger.Instance.LogInfo("Checking Transitions......");

            var end = this.parsedNodes?.Values?.Last();

            this.parsedTransitions?.ForEach(t =>
            {
                var source = this.parsedNodes[t?.sourceNode];
                var dest = this.parsedNodes[t?.destNode];

                if ((source == end) && (dest == end))
                    ECSLogger.Instance.LogError($"Transition {t?.destNode} to {t?.sourceNode} references missing node(s)!");

                if (source == end)
                    ECSLogger.Instance.LogInfo($"Transition from node {t?.sourceNode} to node {t?.destNode}");
                else
                    usedNodes[source?.GetId()] = source;

                if (dest == end)
                    ECSLogger.Instance.LogInfo($"Transition from node {t?.sourceNode} to node {t?.destNode}");
                else
                    usedNodes[dest?.GetId()] = dest;
            });

            var startNode = this.parsedNodes["start"];
            var nStartTransitions = ((IState)startNode)?.GetNumTransition();

            if (nStartTransitions == 0)
                ECSLogger.Instance.LogError("ERROR: There is no transition from the 'start' node, this storyboard will not run!");

            ECSLogger.Instance.LogInfo("====================================");
            ECSLogger.Instance.LogInfo("Checking for Orphans");

            int orphanCount = 0;

            usedNodes?.Values?.ToList().ForEach(o =>
            {
                if (o == usedNodes?.Values?.Last())
                {
                    ECSLogger.Instance.LogError($"Node {o?.ToString()} is an orphand (no transition to or from it), consider deleting");

                    orphanCount++;
                }
            });

            ECSLogger.Instance.LogInfo($"{orphanCount} orphan nodes found");
            ECSLogger.Instance.LogInfo("===================================");
        }

        /// <summary>
        /// Assemble the state machine 
        /// </summary>
        /// <returns></returns>
        private IStateMachine Assemble()
        {
            IStateMachine sb = null;

            var invalidNode = this.parsedNodes?.Values?.Last();

            this.parsedTransitions?.ForEach(t =>
            {
                // Find the states that the transition references
                var source = this.parsedNodes[t?.sourceNode];
                var dest = this.parsedNodes[t?.destNode];

                if ((source != invalidNode) && (dest != invalidNode))
                {
                    // And finally build the actual transition
                    var sourceState = (IState)source;
                    var destState = (IState)dest;

                    var createdTransition = this.sbFactory?.FactoryRef?.CreateTransition("", sourceState, destState, t?.transitionCondition);

                    sourceState?.AddTransition(createdTransition);
                }
                else
                    ECSLogger.Instance.LogError($"Failed to match transition from {t?.sourceNode} -> {t?.destNode} with it's nodes");

            });

            // Assemble the actual state machine
            sb = sbFactory?.FactoryRef?.CreateStateMachine(this.stroyboard?.GetStoryboardName());

            // Add all the states into the state machine
            this.parsedNodes?.Values?.ToList()?.ForEach(s =>
            {
                sb?.AddState((IState)s);
            });

            // set the current state to the "start" node
            sb?.SetCurrentState((IState)parsedNodes["start"]);

            return sb;
        }
        #endregion
        #endregion
    }
}
