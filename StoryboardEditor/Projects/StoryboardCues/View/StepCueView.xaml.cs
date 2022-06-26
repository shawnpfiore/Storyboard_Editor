using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for StepCueView.xaml
    /// </summary>
    public partial class StepCueView : UserControl
    {
        public StepCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.StepCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
