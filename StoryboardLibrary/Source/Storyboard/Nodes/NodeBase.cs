#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	NodeBase.cs
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
using StoryboardLibary.EventsDefinitions;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class NodeBase : BaseState, IStoryboardNode
    {
        #region Fields / Variables
        /// <summary>
        /// Gets or Sets the node type
        /// </summary>
        public ENodeType nodeType { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Stores the condition
        /// </summary>
        protected ICondition finishedCondition;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBase"/> class
        /// </summary>
        /// <param name="id">The node id</param>
        public NodeBase(string id) : base(id)
        {
            this.finishedCondition = null;
            this.nodeType = ENodeType.UNKNOWN_NODE_TYPE;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBase"/> class
        /// </summary>
        /// <param name="id">The node id</param>
        /// <param name="condition">The node condition></param>
        public NodeBase(string id, ICondition condition) : base(id)
        {
            this.nodeType = ENodeType.UNKNOWN_NODE_TYPE;
            this.finishedCondition = condition;
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// On node transtion enter call back
        /// </summary>
        public override void OnEnterState()
        {
            ECSLogger.Instance.LogInfo($"Entering stroyboard node: {this.stateName}");
            base.OnEnterState();

            finishedCondition?.Reset();
            finishedCondition?.SetActive(true);

            EventManager.Instance.TriggerEvent(EventListeners.NODE_STARTED_EVENT.ToString());
        }

        /// <summary>
        /// On node transition out
        /// </summary>
        /// <param name="nextState">The next state</param>
        public override void OnTransitionOut(IState nextState)
        {
            ECSLogger.Instance.LogInfo($"Leaving stroyboard node: {this.stateName}");
            base.OnTransitionOut(nextState);

            EventManager.Instance.TriggerEvent(EventListeners.NODE_FINISHED_EVENT.ToString());

            this.finishedCondition?.SetActive(false);
            this.finishedCondition?.Reset();

            var asNode = nextState as IStoryboardNode;

            if (asNode != null)
            {
                StateChangedEventArgs evnt = new StateChangedEventArgs()
                {
                    NewStateId = asNode.GetId(),
                    OldStateId = this.GetId(),
                    NewStateType = asNode.GetNodeType()
                };

                EventManager.Instance.TriggerEvent(EventListeners.STATE_CHANGED_EVENT.ToString(), evnt);
            }
        }

        /// <summary>
        /// Updates the next state
        /// </summary>
        /// <returns>The next state</returns>
        public override IState Update()
        {
            IState nextState = null;
            bool isTrue = true;

            if(this.finishedCondition != null)
                isTrue = this.finishedCondition.IsTrue(); 
            if (isTrue == true)
                nextState = base.Update();

            return nextState;
        }

        /// <summary>
        /// Automate node - not implimented
        /// </summary>
        public virtual void AutomateNode() { }

        /// <summary>
        /// gets the node id
        /// </summary>
        /// <returns>The node id</returns>
        public virtual string GetId()
        {
            return stateName;
        }

        /// <summary>
        /// Gets the node type
        /// </summary>
        /// <returns>The node type</returns>
        public virtual ENodeType GetNodeType()
        {
            return nodeType;
        }

        /// <summary>
        /// Gets the node condition
        /// </summary>
        /// <returns>The node condition</returns>
        public virtual ICondition GetCondition()
        {
            return finishedCondition;
        }

        #endregion
        #endregion
    }
}
