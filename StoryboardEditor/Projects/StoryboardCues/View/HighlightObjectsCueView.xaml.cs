using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for HighlightObjectCue.xaml
    /// </summary>
    public partial class HighlightObjectsCueView : UserControl
    {
        public HighlightObjectsCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.HighlightObjectsCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
