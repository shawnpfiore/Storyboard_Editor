#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	TreeViewManager.cs
//  Author:            	Jonathan Ramos
//  Creation Date:  	February 19, 2019
//  Last Update:    	March 14, 2019
//                    	Jonathan Ramos
//
//  Copyrights:        	Copyright 2019
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using System.Linq;
using StoryboardUtils;
using StoryboardContainer;
using System.Windows.Controls;
using System.Collections.Generic;

namespace StoryboardEditor.Managers
{
    public static class TreeViewManager
    {
        #region Variables
        private static TreeView treeView = null;
        private static ScrollViewer scrollView = null;
        #endregion

        #region Functions
        #region Public Static Functions
        /// <summary>
        /// Instantiates the TreeViewManager by linking the TreeView via reference.
        /// </summary>
        /// <param name="tree"></param>
        public static void Instantiate(ref TreeView tree)
        {
            treeView = tree;
        }

        /// <summary>
        /// Instantiates the TreeViewManager by linking the ScrollView via reference.
        /// </summary>
        /// <param name="ScrollView"></param>
        public static void Instantiate(ref ScrollViewer ScrollView)
        {
            scrollView = ScrollView;
        }

        /// <summary>
        /// Returns the ScrollViewer
        /// </summary>
        /// <returns></returns>
        public static ScrollViewer GetScrollViewer()
        {
            return scrollView;
        }

        /// <summary>
        /// Updates the layout of the TreeView.
        /// </summary>
        public static void Update()
        {
            treeView?.UpdateLayout();
        }

        /// <summary>
        /// Adds the object to the TreeView.
        /// </summary>
        /// <param name="obj"></param>
        public static void Add(object obj)
        {
            treeView?.Items.Add(obj);
        }

        /// <summary>
        /// Removes the object from the TreeView.
        /// </summary>
        /// <param name="obj"></param>
        public static void Remove(object obj)
        {
            treeView?.Items.Remove(obj);
        }

        /// <summary>
        /// Removes an object from the TreeView at an index.
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveAt(int index)
        {
            treeView?.Items.RemoveAt(index);
        }

        /// <summary>
        /// Updates by using the cache file.
        /// </summary>
        public static void XMLUpdate()
        {
            treeView?.Items.Clear();

            if (LoadStoryboardConfigs.LoadStoryboardContainerFile(CacheDataManager.GetFileLocation()))
            {
                Task storyboard = LoadStoryboardConfigs.StoryboardContainerFile;

                List<TreeViewItem> cues = new List<TreeViewItem>();

                for (int i = 0; i < storyboard?.GameCues?.Length; i++)
                    cues.Add(ConvertToCueItem(storyboard.GameCues[i]));

                for (int i = 0; i < storyboard?.TaskData?.Step?.Length; i++)
                    cues.Add(ConvertToStepItem(storyboard.TaskData.Step[i]));

                storyboard?.Transitions?.ToList()?.ForEach(t =>
                {
                    cues?.ForEach(c =>
                    {
                        if (t?.FromStep == GetID(c?.Header.ToString()))
                            Add(c);
                    });
                });
            }

            // Sets up the MouseDoubleClick event on the TreeView items
            foreach (TreeViewItem item in treeView.Items)
                item.MouseDoubleClick += TreeViewManager_MouseDoubleClick;

            Update();
        }
        #endregion
        #region Private Static Functions
        /// <summary>
        /// Returns the ID from the name passed in.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ID</returns>
        private static string GetID(string name)
        {
            string ID = "";

            if (name.Contains("ST"))
                return name;

            if (name.Contains(":"))
                ID = name.Substring(0, name.IndexOf(" "));

            return ID;
        }

        /// <summary>
        /// Converts the Cue passed in into a TreeViewItem of that cue and its children.
        /// </summary>
        /// <param name="cueInfo"></param>
        /// <returns>TreeViewItem Cue</returns>
        private static TreeViewItem ConvertToCueItem(CueBase cueInfo)
        {
            TreeViewItem item = new TreeViewItem();

            item.Header = cueInfo.Id.ToString() + " : " + cueInfo.GetType().Name;

            switch (cueInfo.GetType().Name)
            {
                case "PlayAudioCue":
                    {
                        TreeViewItem AudioClip = new TreeViewItem();
                        TreeViewItem Caption = new TreeViewItem();

                        AudioClip.Header = ((PlayAudioCue)cueInfo).AudioClip;
                        Caption.Header = ((PlayAudioCue)cueInfo).Caption;

                        // If the headers are null then we won't add them to the TreeView.
                        if (AudioClip.Header != null) item.Items.Add(AudioClip);
                        if (Caption.Header != null) item.Items.Add(Caption);
                    }
                    break;
                case "SnapToCameraCue":
                    {
                        SnapToCameraCue cue = ((SnapToCameraCue)cueInfo);
                        for (int i = 0; i < cue.CameraName?.Length; i++)
                        {
                            TreeViewItem CameraInfo = new TreeViewItem();
                        
                            CameraInfo.Header = cue.CameraName[i].ToString() + ", Delay " + cue.Delay[i].ToString();

                            // If the headers are null then we won't add them to the TreeView.
                            if (CameraInfo.Header != null) item.Items.Add(CameraInfo);
                        }
                    }
                    break;
                case "SmartObjectCue":
                    {
                        TreeViewItem ObjectName = new TreeViewItem();
                        TreeViewItem Highlight = new TreeViewItem();

                        ObjectName.Header = ((SmartObjectCue)cueInfo).ObjectName;
                        Highlight.Header = ((SmartObjectCue)cueInfo).Highlight;

                        // If the headers are null then we won't add them to the TreeView.
                        if (ObjectName.Header != null) item.Items.Add(ObjectName);
                        if (Highlight.Header != null) item.Items.Add(Highlight);
                    }
                    break;
                case "HighlightObjectsCue":
                    {
                        HighlightObjectsCue cue = ((HighlightObjectsCue)cueInfo);
                        for (int i = 0; i < cue.GameObjects?.Length; i++)
                        {
                            TreeViewItem HighlightsObjectInfo = new TreeViewItem();
                            HighlightsObjectInfo.Header = cue.GameObjects[i].ToString() + ", " + cue.Highlight[i].ToString();

                            // If the headers are null then we won't add them to the TreeView.
                            if (HighlightsObjectInfo.Header != null) item.Items.Add(HighlightsObjectInfo);
                        }
                    }
                    break;
                case "DelayCue":
                    {
                        TreeViewItem Delay = new TreeViewItem();

                        Delay.Header = ((DelayCue)cueInfo).Delay;

                        // If the headers are null then we won't add them to the TreeView.
                        if (Delay != null) item.Items.Add(Delay);
                    }
                    break;
                case "UnknownCue":
                    {
                        TreeViewItem Description = new TreeViewItem();

                        Description.Header = ((UnknownCue)cueInfo).Description;

                        // If the headers are null then we won't add them to the TreeView.
                        if (Description.Header != null) item.Items.Add(Description);
                    }
                    break;
                default:
                    break;
            }

            return item;
        }

        /// <summary>
        /// Converts the step variable passed in into a TreeViewItem.
        /// </summary>
        /// <param name="stepInfo"></param>
        /// <returns>TreeViewItem Step</returns>
        private static TreeViewItem ConvertToStepItem(Step stepInfo)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = stepInfo.Id;

            TreeViewItem Text = new TreeViewItem();
            Text.Header = stepInfo.StepData.Text;

            TreeViewItem ObjectName = new TreeViewItem();
            ObjectName.Header = stepInfo.StepData.ObjectName;

            if (ObjectName.Header != null) item.Items.Add(ObjectName);
            if (Text.Header != null) item.Items.Add(Text);

            return item;
        }

        /// <summary>
        /// Controls the behaviour of the double click of the tree view items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TreeViewManager_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = treeView.Items.IndexOf(sender);

            if (index == 0)
            {
                scrollView.ScrollToTop();
                return;
            }

            if (index == treeView.Items.Count)
            {
                scrollView.ScrollToBottom();
                return;
            }

            if (index == 1)
            {
                scrollView.ScrollToVerticalOffset(280);
                return;
            }
            else
            {
                int amount = 280 + ((index - 1) * 310);
                scrollView.ScrollToVerticalOffset(amount);
            }
        }
        #endregion
        #endregion
    }
}
