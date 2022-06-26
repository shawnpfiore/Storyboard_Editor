using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for DelayCue.xaml
    /// </summary>
    public partial class DelayCueView : UserControl
    {
        public DelayCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.DelayCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
