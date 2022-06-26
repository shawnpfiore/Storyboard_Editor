#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardContainerUtils.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	February 13, 2019
//  Last Update:    	March 14, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using StoryboardContainer;
using StoryboardCues.Model;
using StoryboardCues.View;
using System.Collections.Generic;
using System.Linq;
namespace StoryboardEditor
{
    public static class StoryboardContainerUtils
    {
        #region Methods
        #region Non Public Methods
        /// <summary>
        /// Creates a StepCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static StepCueView CreateStepCueView(object cue)
        {
            StepCueView stepCueView = null;
            var step = cue as Step;

            if (step != null)
            {
                var stepModel = new StepCueModel()
                {
                    CueId = step?.Id,
                    StepCaption = step?.StepData?.Text,
                    ObjectName = step?.StepData?.ObjectName
                };

                stepCueView = new StepCueView()
                {
                    DataContext = stepModel
                };
            }
            return stepCueView;
        }

        /// <summary>
        /// Creates a SmartObjectCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static SmartObjectCueView CreateSmartObjectCueView(object cue)
        {
            SmartObjectCueView smartObjectCueView = null;
            var smartObjectCue = cue as SmartObjectCue;

            if (smartObjectCue != null)
            {
                var smartObjectModel = new SmartObjectCueModel()
                {
                    CueId = smartObjectCue?.Id,
                    SmartObjectName = smartObjectCue?.ObjectName
                };

                smartObjectCueView = new SmartObjectCueView()
                {
                    DataContext = smartObjectModel
                };
            }

            return smartObjectCueView;
        }

        /// <summary>
        /// Creates a DelayCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static DelayCueView CreateDelayCueView(object cue)
        {
            DelayCueView delayCueView = null;
            var delayCue = cue as DelayCue;

            if (delayCue != null)
            {
                var delayCueModel = new DelayCueModel()
                {
                    CueId = delayCue?.Id,
                    DelayValue = delayCue?.Delay.ToString()
                };

                delayCueView = new DelayCueView()
                {
                    DataContext = delayCueModel
                };
            }

            return delayCueView;
        }

        /// <summary>
        /// Creates a PlayAudioCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static PlayAudioCueView CreatePlayAudioCueView(object cue)
        {
            PlayAudioCueView playAudioCueView = null;
            var playAudioCue = cue as PlayAudioCue;

            if (playAudioCue != null)
            {
                var playAudioModel = new PlayAudioCueModel()
                {
                    CueId = playAudioCue?.Id,
                    AudioDescription = playAudioCue?.Caption,
                    AudioFileName = playAudioCue?.AudioClip
                };

                playAudioCueView = new PlayAudioCueView()
                {
                    DataContext = playAudioModel
                };
            }

            return playAudioCueView;
        }

        /// <summary>
        /// Creates a HighlightObjectsCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static HighlightObjectsCueView CreateHighlightObjectsCueView(object cue)
        {
            List<ListViewHighlightObjects> highlightItemsList = null;
            ListViewHighlightObjects items = null;
            HighlightObjectsCueView highlightObjectsCueView = null;
            var highlightObjectsCue = cue as HighlightObjectsCue;

            if (highlightObjectsCue != null)
            {
                highlightItemsList = new List<ListViewHighlightObjects>();

                var highlightObjectsModel = new HighlightObjectsCueModel()
                {
                    CueId = highlightObjectsCue?.Id
                };

                highlightObjectsCue?.Highlight?.ToList()?.ForEach(h =>
                {
                    items = new ListViewHighlightObjects()
                    {
                        Highlight = h
                    };

                    highlightItemsList.Add(items);
                });

                var index = 0;
                highlightObjectsCue?.GameObjects?.ToList()?.ForEach(g =>
                {
                    if (index < highlightItemsList?.Count())
                    {
                        highlightItemsList[index].Name = g;
                        index++;
                    }
                });

                highlightItemsList?.ForEach(h =>
                {
                    highlightObjectsModel?.ListViewItems?.Add(h);
                });

                highlightObjectsCueView = new HighlightObjectsCueView()
                {
                    DataContext = highlightObjectsModel
                };
            }

            return highlightObjectsCueView;
        }

        /// <summary>
        /// Creates a SnapToCameraCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static SnapToCameraCueView CreateSnapToCameraCueView(object cue)
        {
            List<ListViewSnapToCameraItems> snapToCameraItemsList = null;
            ListViewSnapToCameraItems items = null;
            SnapToCameraCueView snapToCameraCueView = null;
            var snapToCameraCue = cue as SnapToCameraCue;

            if (snapToCameraCue != null)
            {
                snapToCameraItemsList = new List<ListViewSnapToCameraItems>();

                var snapToCameraModel = new SnapToCameraCueModel()
                {
                    CueId = snapToCameraCue?.Id,
                };

                snapToCameraCue?.CameraName?.ToList()?.ForEach(c =>
                {
                    items = new ListViewSnapToCameraItems()
                    {
                        Name = c
                    };

                    snapToCameraItemsList?.Add(items);
                });

                int index = 0;
                snapToCameraCue?.Delay?.ToList()?.ForEach(d =>
                {
                    if (index < snapToCameraItemsList?.Count())
                    {
                        snapToCameraItemsList[index].Delay = d;
                        index++;
                    }
                });

                snapToCameraItemsList?.ForEach(s =>
                {
                    snapToCameraModel?.ListViewItems?.Add(s);
                });

                snapToCameraCueView = new SnapToCameraCueView()
                {
                    DataContext = snapToCameraModel
                };
            }

            return snapToCameraCueView;
        }

        /// <summary>
        /// Creates a UnknownCueView from the loaded stroyboard file 
        /// </summary>
        /// <param name="cue">The cue</param>
        /// <returns>The Object</returns>
        public static UnknownCueView CreateUnknownCueView(object cue)
        {
            UnknownCueView unknownCueView = null;
            var unknownCue = cue as UnknownCue;

            if (unknownCue != null)
            {
                var unknownModel = new UnknownCueModel()
                {
                    CueId = unknownCue?.Id,
                    CueDescription = unknownCue?.Description
                };

                unknownCueView = new UnknownCueView()
                {
                    DataContext = unknownModel
                };
            }
            return unknownCueView;
        }
        #endregion
        #endregion
    }
}
