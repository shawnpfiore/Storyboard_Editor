#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	StoryboardCueParser.cs
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
using StoryboardContainer;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardLibary
{
    public class StoryboardCueParser
    {
        #region Fields / Variables
        #endregion

        #region Properties
        /// <summary>
        /// Stores a list of storyboard nodes 
        /// </summary>
        private Dictionary<string, IStoryboardNode> cues;

        /// <summary>
        /// Stores the cue type lookup map
        /// </summary>
        private Dictionary<string, ECueType> lookupMap;
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Does a quick lookup in the map for the passed string, return UNKNOWN_CUE if the type is not found
        /// </summary>
        /// <param name="cueType">The storyboard cue type we are looking for</param>
        /// <returns>The cue type</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ECueType GetEnum(string cueType)
        {
            ECueType type = ECueType.UNKNOWN_CUE;

            if (this.lookupMap.ContainsKey(cueType))
            {
                this.lookupMap?.Keys?.ToList()?.ForEach(c =>
                {
                    if (cueType == c)
                        type = this.lookupMap[c];
                });
            }

            return type;
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCueParser"/> class
        /// </summary>
        public StoryboardCueParser()
        {
            this.cues = new Dictionary<string, IStoryboardNode>();
            this.lookupMap = new Dictionary<string, ECueType>();

            // The actual nodes in the storyboard xml 
            this.lookupMap.Add("PlayAudioCue", ECueType.PLAY_AUDIO_CUE);
            this.lookupMap.Add("SnapToCameraCue", ECueType.SNAP_TO_CAMERA_CUE);
            this.lookupMap.Add("SmartObjectCue", ECueType.SMART_OBJECT_CUE);
            this.lookupMap.Add("DelayCue", ECueType.DELAY_CUE);
            this.lookupMap.Add("HighlightObjectCue", ECueType.HIGHLIGHT_OBJECTS_CUE);
        }
        #endregion

        /// <summary>
        /// Parses all game cues from the storyboard xml and stores the created nodes in the passed map
        /// </summary>
        /// <param name="nodes">The map of nodes to search</param>
        /// <param name="factory">The storyboard factory</param>
        public void ParseGameCues(Dictionary<string, IStoryboardNode> nodes, StoryboardFactory factory)
        {
            var container = StoryboardConfig.storyboardContainer;

            container?.GameCues?.ToList()?.ForEach(cue =>
            {
                nodes.Add(cue.Id, this.CreateCueNode(cue, factory));
            });
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Creates a game cue based on the xml element passed. Depending on the 'type' attribute inside the
        /// element, it will call the appropriate create function below.
        /// </summary>
        /// <param name="cueElement">The cue element</param>
        /// <param name="factory">The storyboard factory</param>
        /// <returns></returns>
        private IStoryboardNode CreateCueNode(CueBase cueElement, StoryboardFactory factory)
        {
            IStoryboardNode ret = null;
            if (cueElement == null)
                ECSLogger.Instance.LogError($"Null cue element found in storyboard XML! Skipping { cueElement.ToString()}");
            else
            {
                var cueName = cueElement?.ToString();
                var result = cueName?.Substring(cueName.LastIndexOf('.') + 1);
               
                var cueTypes = this.GetEnum(result);

                switch (cueTypes)
                {
                    case ECueType.PLAY_AUDIO_CUE:
                        ret = this.CreatePlayAudioCue(cueElement, factory);
                        break;
                    case ECueType.SNAP_TO_CAMERA_CUE:
                        ret = this.CreateSnapToCue(cueElement, factory);
                        break;
                    case ECueType.SMART_OBJECT_CUE:
                        ret = this.CreateSmartObjectCue(cueElement, factory);
                        break;
                    case ECueType.DELAY_CUE:
                        ret = this.CreateDelayCue(cueElement, factory);
                        break;
                    case ECueType.HIGHLIGHT_OBJECTS_CUE:
                        ret = this.CreateHighlightObjectsCue(cueElement, factory);
                        break;
                    default:
                        ret = this.CreateUnknownCue(cueElement, factory);
                        break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Creates PlayAudioCue
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created game cue object</returns>
        private IStoryboardNode CreatePlayAudioCue(CueBase cueElement,
            StoryboardFactory factory)
        {
            var playAudioCue = (PlayAudioCue)factory?.CreatePlayAudioCue(cueElement?.Id?.ToString());

            if (playAudioCue != null)
            {
                var cue = cueElement as StoryboardContainer.PlayAudioCue; 
                playAudioCue.AudioClip = cue?.AudioClip?.ToString();
                playAudioCue.Caption = cue?.Caption?.ToString();

            }
            return playAudioCue;
        }

        /// <summary>
        /// Creates SnapToCamera
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created game cue object</returns>
        private IStoryboardNode CreateSnapToCue(CueBase cueElement,
           StoryboardFactory factory)
        {
            var snapToCue = (SnapToCameraCue)factory?.CreateSnapToCue(cueElement?.Id?.ToString());

            if (snapToCue != null)
            {
                var cue = cueElement as StoryboardContainer.SnapToCameraCue;
                snapToCue.cameras = cue?.CameraName?.ToList();
                snapToCue.Delay = cue.Delay?.ToList();
            }
            return snapToCue;
        }

        /// <summary>
        /// Creates SmartObjectCue
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created smart obejct cue object</returns>
        private IStoryboardNode CreateSmartObjectCue(CueBase cueElement,
           StoryboardFactory factory)
        {
            var condition = factory.CreatePropertyCondition("SmartObjectCondition", true);
            var smartObjectCue = (SmartObjectCue)factory?.CreateSmartObjectCue(cueElement?.Id?.ToString(), condition);

            if (smartObjectCue != null)
            {
                var cue = cueElement as StoryboardContainer.SmartObjectCue; 
                smartObjectCue.ObjectName = cue?.ObjectName?.ToString();
                smartObjectCue.Highlight = cue?.Highlight?.ToString();
            }
            return smartObjectCue;
        }

        /// <summary>
        /// Creates a delay cue
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created delay cue object</returns>
        private IStoryboardNode CreateDelayCue(CueBase cueElement, 
            StoryboardFactory factory)
        {
            var delayCue = (DelayCue)factory?.CreateDelayCue(cueElement?.Id?.ToString());

            if(delayCue != null)
            {
                var cue = cueElement as StoryboardContainer.DelayCue;
                delayCue.Delay = cue.Delay; 
            }

            return delayCue; 
        }

        /// <summary>
        /// Creates a highlight objects cue
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created highlight obejcts cue object</returns>
        private IStoryboardNode CreateHighlightObjectsCue(CueBase cueElement,
            StoryboardFactory factory)
        {
            var highlightObjectsCue = (HighlightObjectsCue)factory?.HighlightObjectsCue(cueElement?.Id?.ToString());

            if (highlightObjectsCue != null)
            {
                var cue = cueElement as StoryboardContainer.HighlightObjectsCue;

                //Index these one for one 
                cue?.GameObjects?.ToList()?.ForEach(o => highlightObjectsCue?.ObjectNames?.Add(o));
                cue?.Highlight?.ToList()?.ForEach(h => highlightObjectsCue?.Highlights?.Add(h));
            }

            return highlightObjectsCue;
        }

        /// <summary>
        /// Creates UnknownCue
        /// </summary>
        /// <param name="cueElement">XML Element</param>
        /// <param name="factory">Storyboard factory</param>
        /// <returns>The created game cue object</returns>
        private IStoryboardNode CreateUnknownCue(CueBase cueElement,
         StoryboardFactory factory)
        {
            var unknownCue = (UnknownCue)factory?.CreateUnknownCue(cueElement?.Id?.ToString());
            return unknownCue;
        }
        #endregion
        #endregion
    }
}
