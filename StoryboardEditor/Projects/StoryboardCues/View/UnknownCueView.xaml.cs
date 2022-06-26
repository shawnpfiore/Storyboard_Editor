using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for UnknownCue.xaml
    /// </summary>
    public partial class UnknownCueView : UserControl
    {
        public UnknownCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.UnknownCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
