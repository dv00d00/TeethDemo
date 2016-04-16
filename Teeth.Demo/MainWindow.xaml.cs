using System.Windows;

namespace Teeth.Demo
{
    using System.Windows.Input;
    using Model;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ViewContext = new ViewContext(this.Canvas);
            var mainWindowViewModel = new MainWindowViewModel();
            mainWindowViewModel.Initialize(this.ViewContext);
            this.DataContext = mainWindowViewModel;
            this.ViewContext.SelectionChanged += ViewContext_SelectionChanged;
        }

        private void ViewContext_SelectionChanged(object sender, System.EventArgs e)
        {
            this.ListView.UnselectAll();
        }

        public ViewContext ViewContext { get; }
        
        private void OnSelectNext(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewContext.HandleNavigateNext();
        }

        private void OnSelectPrevious(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewContext.HandleNavigatePrevious();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            this.ViewContext.ToogleMultiselect(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
            this.ViewContext.ToogleRangeSelect(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            this.ViewContext.ToogleMultiselect(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
            this.ViewContext.ToogleRangeSelect(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
        }
    }
}
