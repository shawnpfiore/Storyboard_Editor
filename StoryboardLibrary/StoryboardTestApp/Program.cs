using ECS.Utilites.Logging;
using StoryboardLibary;
using StoryboardLibary.EventsDefinitions;
using System;
using static StoryboardLibary.IStoryboardLibrary;

namespace StoryboardTestApp
{
    class Program
    {
        private static Action<EventArgs> storyboardLoadedEvent;
        private static Action<EventArgs> playAudioEvent;

        static int Main(string[] args)
        {
            EventManager eventSys = EventManager.Instance;

            //////////////////////////////////////////////////////////////////////////
            // Listen events examples 
            storyboardLoadedEvent = new Action<EventArgs>(StoryboardLoadedEvent); // The receiving function 
            eventSys.StartListening(EventListeners.STORYBOARD_LOADED_EVENT.ToString(), storyboardLoadedEvent); // The registered event 

            playAudioEvent = new Action<EventArgs>(PlayAudioEvent); // The receiving function
            eventSys.StartListening(EventListeners.ACTIVE_PAY_AUDIO_CUE_EVENT.ToString(), playAudioEvent); // The registered event 
            //////////////////////////////////////////////////////////////////////////

            StoryboardSystem sb = new StoryboardSystem();

            //////////////////////////////////////////////////////////////////////////
            // Trigger event example
            sb.LoadStoryboard(@"..\..\..\Data\StoryboardContainer.xml", IStoryboardLibrary.EStoryboardType.ROOT_STORYBOARD);
            eventSys.TriggerEvent(EventTriggers.START_LESSON_EVENT.ToString()); // The event name 
            AutomateStoryboardEventArgs automateStoryboardEventArgs = new AutomateStoryboardEventArgs()
            {
                storyboardPath = ""
            };

            eventSys.TriggerEvent(EventTriggers.AUTOMATE_STORYBOARD_EVENT.ToString(), automateStoryboardEventArgs); // The event args 
          
            return 0;
        }

        /// <summary>
        /// Receives the storyboard loaded event 
        /// </summary>
        /// <param name="eventParam">The in coming event args</param>
        private static void StoryboardLoadedEvent(EventArgs eventParam)
        {
            if (eventParam != null)
            {
                var message = eventParam as StoryboardLoadedEventArgs;

                ECSLogger.Instance.LogInfo($"Just a test {message.StoryboardName}");
            }
        }

        /// <summary>
        /// Receives a PlayAudio storyboard cue event 
        /// </summary>
        /// <param name="eventParam">The in coming event args</param>
        private static void PlayAudioEvent(EventArgs eventParam)
        {
            if (eventParam != null)
            {
                var message = eventParam as ActivePayAudioCueEventArgs;

                ECSLogger.Instance.LogInfo($"Just a test {message.AudioClip}");
            }
        }
    }
}
