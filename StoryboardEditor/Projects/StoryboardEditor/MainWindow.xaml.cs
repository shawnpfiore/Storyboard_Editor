using System.Windows;
using StoryboardEditor.Managers;

namespace StoryboardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            TreeViewManager.Instantiate(ref CanvasTreeView);
            TreeViewManager.XMLUpdate();

            MainWindow slave = this;
            SearchManager.Instantiate(ref slave);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CacheDataManager.DeleteCacheFile();
        }
    }
}
