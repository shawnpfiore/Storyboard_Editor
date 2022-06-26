using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for PlayAudioCue.xaml
    /// </summary>
    public partial class PlayAudioCueView : UserControl
    {
        public PlayAudioCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.PlayAudioCueModel;

            if (enable)
                model?.GenerateNewCueId();

            //AudioNameTextBox.LostKeyboardFocus +=  AudioNameTextBox_LostKeyboardFocus;
        }
    }
}
