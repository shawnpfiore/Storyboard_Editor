#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	PropertyCondition.cs
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
namespace StoryboardLibary
{
    public class PropertyCondition : ICondition
    {
        #region Fields / Variables
        #endregion

        #region Properties
        /// <summary>
        /// Stores the property condition
        /// </summary>
        protected bool condition;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCondition"/> class
        /// </summary>
        /// <param name="value">The condition</param>
        public PropertyCondition(bool value)
        {
            this.condition = value; 
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Checks if the condition
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsTrue()
        {
            return this.condition;
        }

        /// <summary>
        /// Resets the condition
        /// </summary>
        public void Reset()
        {
            this.condition = true ? false : this.condition;
        }

        /// <summary>
        /// Sets the condition
        /// </summary>
        /// <param name="isActive">True or False</param>
        public void SetActive(bool isActive)
        {
            this.condition = isActive; 
        }
        #endregion
        #endregion
    }
}
