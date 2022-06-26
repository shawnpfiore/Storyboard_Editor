#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ParsedTransition.cs
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
using System.Collections.Generic;
using System.Linq;

namespace StoryboardLibary
{
    public class ParsedTransition
    {

        #region Properties
        /// <summary>
        /// Gets or Sets the source node 
        /// </summary>
        public string sourceNode { get; set; }

        /// <summary>
        /// Gets or Sets the destination node
        /// </summary>
        public string destNode { get; set; }

        /// <summary>
        /// Gets or Sets the transition condition 
        /// </summary>
        public ICondition transitionCondition { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedTransition"/> class
        /// </summary>
        /// <param name="source">The transition sourse</param>
        /// <param name="dest">The transition destination</param>
        public ParsedTransition(string source, string dest)
        {
            this.sourceNode = source;
            this.destNode = dest;
        }
        #endregion
    }

    /// <summary>
    /// Helper class to generate storyboard transitions out of xml elements
    /// </summary>
    public class StoryboardTransitionParser
    {
        #region Fields/Variables
        /// <summary>
        /// Stores the list of parsed transitions 
        /// </summary>
        private List<ParsedTransition> parsedTransitions;

        /// <summary>
        /// Stores the list of parsed transitioin conditions 
        /// </summary>
        private List<ICondition> parsedConditions;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTransitionParser"/> class
        /// </summary>
        public StoryboardTransitionParser()
        {
            this.parsedConditions = new List<ICondition>();
            this.parsedTransitions = new List<ParsedTransition>();
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Parse transition from the storyboard 
        /// </summary>
        /// <param name="factory">The storyboard factory</param>
        /// <returns>List of parsed transition</returns>
        public List<ParsedTransition> ParseTransition(StoryboardFactory factory)
        {
            var container = StoryboardConfig.storyboardContainer;
           
            container?.Transitions?.ToList()?.ForEach(t =>
            {
                var source = t?.FromStep;
                var dest = t?.ToStep;

                source = string.IsNullOrEmpty(source) ? "start" : source;
                dest = string.IsNullOrEmpty(dest) ? "end" : dest;
                ParsedTransition created = new ParsedTransition(source, dest);

                created.transitionCondition = this.AssembleConditions(t, factory);
                
                this.parsedTransitions?.Add(created); 
            });

            ECSLogger.Instance.LogInfo($"Created {this.parsedTransitions?.Count()} transitions"); 

            return this.parsedTransitions;
        }

        /// <summary>
        /// Assemble trnasition conditions
        /// </summary>
        /// <param name="transition">The trnasition</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The condition</returns>
        public ICondition AssembleConditions(StoryboardContainer.Transition transition, StoryboardFactory factory)
        {
            bool condition;
            return factory.CreatePropertyCondition("TransitionCondition", bool.TryParse(transition?.Condition.ToString(), out condition)); ;
        }
        #endregion
        #endregion
    }
}
