#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardCanvasUtils.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	February 08, 2019
//  Last Update:    	February 08, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using StoryboardCues.Model;
using StoryboardCues.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static StoryboardEditor.IStoryboardEditorLib;

namespace StoryboardEditor
{
    public static class StoryboardCanvasUtils
    {
        #region Properties
        /// <summary>
        /// Gets or Sets the canvas game cues container 
        /// </summary>
        public static ObservableCollection<object> CanvasGameCues { get; set; }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Gets a cue user control
        /// </summary>
        /// <param name="cueBase">The cue model</param>
        /// <returns>The cue usercontrol</returns>
        public static UserControl GetCueUserControl(CueBaseModel cueBase)
        {
            UserControl control = null;

            CanvasGameCues?.ToList()?.ForEach(c =>
            {
                var view = c as FrameworkElement;

                var cue = view?.DataContext as CueBaseModel;

                if (cueBase?.CueId == cue?.CueId)
                {
                    control = c as UserControl;
                }
            });

            return control;
        }

        /// <summary>
        /// Remove ths control from the storyboard canvas
        /// </summary>
        /// <param name="control">The control to remove</param>
        public static void RemoveControlFromCanvas(UserControl control)
        {
            if (control != null)
            {
                if (CanvasGameCues.Contains(control))
                    CanvasGameCues?.Remove(control);
            }
        }

        /// <summary>
        /// Removes the cueid from the CueIdDictinary
        /// </summary>
        /// <param name="cueBase">The cue base model to remove</param>
        public static void RemoveCueIdFromDictinary(CueBaseModel cueBase)
        {
            if (cueBase.NodeType == ENodeType.GAMECUE_NODE_TYPE)
            {
                var cueKey = GetCueKeys(ENodeType.GAMECUE_NODE_TYPE);

                if (cueKey.Contains(cueBase?.CueId))
                {
                    var key = cueKey?.FirstOrDefault(k => k == cueBase.CueId);
                    CueIdDictinary?.Remove(key);
                }
            }
            else if (cueBase.NodeType == ENodeType.TASK_NODE_TYPE)
            {
                var cueKey = GetCueKeys(ENodeType.TASK_NODE_TYPE);

                if (cueKey.Contains(cueBase?.CueId))
                {
                    var key = cueKey?.FirstOrDefault(k => k == cueBase.CueId);
                    CueIdDictinary?.Remove(key);
                }
            }
            else if (cueBase.NodeType == ENodeType.UNKNOWN_NODE_TYPE)
            {
                var cueKey = GetCueKeys(ENodeType.UNKNOWN_NODE_TYPE);

                if (cueKey.Contains(cueBase?.CueId))
                {
                    var key = cueKey?.FirstOrDefault(k => k == cueBase.CueId);
                    CueIdDictinary?.Remove(key);
                }
            }
        }

        /// <summary>
        /// Gets the cue base model 
        /// </summary>
        /// <param name="name">The cue name</param>
        /// <param name="id">The cue id</param>
        /// <returns>The cue base model</returns>
        public static CueBaseModel GetCueBaseModel(string name, string id)
        {
            CueBaseModel cueModel = null;

            switch (name)
            {
                case "PlayAudioCue":
                    cueModel = FindCueBaseModel<PlayAudioCueView>(id);
                    break;
                case "SnapToCameraCue":
                    cueModel = FindCueBaseModel<SnapToCameraCueView>(id);
                    break;
                case "SmartObjectCue":
                    cueModel = FindCueBaseModel<SmartObjectCueView>(id);
                    break;
                case "HighlightObjectsCue":
                    cueModel = FindCueBaseModel<HighlightObjectsCueView>(id);
                    break;
                case "DelayCue":
                    cueModel = FindCueBaseModel<DelayCueView>(id);
                    break;
                case "StepCue":
                    cueModel = FindCueBaseModel<StepCueView>(id);
                    break;
                case "UnknownCue":
                    cueModel = FindCueBaseModel<UnknownCueView>(id);
                    break;
            }

            return cueModel;
        }
      
        /// <summary>
        /// Gets the objects view model
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <returns>The cue base model</returns>
        public static CueBaseModel FindCueBaseModel<T>(string cueId)
        {
            CueBaseModel cueBase = null;

            CanvasGameCues?.ToList()?.ForEach(c =>
            {
                if (c.GetType() == typeof(T))
                {
                    var view = c as FrameworkElement;

                    var cue = view?.DataContext as CueBaseModel;

                    if (cue?.CueId == cueId)
                        cueBase = cue;
                }
            });

            return cueBase;
        }

        // <summary>
        /// Gets the cue keys from the CueIdDictinary
        /// </summary>
        /// <param name="nodeType">The node type</param>
        /// <returns>The list of keys</returns>
        public static IEnumerable<string> GetCueKeys(ENodeType nodeType)
        {
            IEnumerable<string> cueKey = null;

            if (CueIdDictinary.Any())
            {
                cueKey = from entry in CueIdDictinary
                         where entry.Value == nodeType
                         select entry.Key;
            }

            return cueKey;
        }
        #endregion
        #endregion
    }
}
