using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for SnapToCameraCue.xaml
    /// </summary>
    public partial class SnapToCameraCueView : UserControl
    {
        public SnapToCameraCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.SnapToCameraCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
