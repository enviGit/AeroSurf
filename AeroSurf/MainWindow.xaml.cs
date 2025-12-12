using CefSharp;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeroSurf
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            Browser.RequestHandler = new CustomRequestHandler();

            Browser.AddressChanged += OnBrowserAddressChanged;

            Browser.LoadingStateChanged += OnLoadingStateChanged;

            Browser.Loaded += (s, e) => Browser.Load(_viewModel.Address);
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(100);

                    CommandManager.InvalidateRequerySuggested();
                });
            }
        }

        private void OnBrowserAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!UrlTextBox.IsKeyboardFocused)
                {
                    _viewModel.UpdateAddress(e.NewValue.ToString());
                }
            });
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }
    }
}