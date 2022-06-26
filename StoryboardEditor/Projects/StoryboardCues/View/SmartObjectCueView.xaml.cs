using System.Windows.Controls;

namespace StoryboardCues.View
{
    /// <summary>
    /// Interaction logic for SmartObjectCue.xaml
    /// </summary>
    public partial class SmartObjectCueView : UserControl
    {
        public SmartObjectCueView(bool enable = true)
        {
            this.InitializeComponent();

            var model = this.DataContext as Model.SmartObjectCueModel;

            if (enable)
                model?.GenerateNewCueId();
        }
    }
}
