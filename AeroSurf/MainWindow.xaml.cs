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
        private bool _isUserSwitchingTab = false;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            Browser.RequestHandler = new CustomRequestHandler();

            Browser.AddressChanged += OnBrowserAddressChanged;

            Browser.TitleChanged += OnBrowserTitleChanged;

            Browser.LoadingStateChanged += OnLoadingStateChanged;

            Browser.Loaded += (s, e) => {
                if (_viewModel.SelectedTab != null) Browser.Load(_viewModel.SelectedTab.Url);
            };
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

        private void TabListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedTab != null)
            {
                _isUserSwitchingTab = true;

                var url = _viewModel.SelectedTab.Url;

                if (string.IsNullOrWhiteSpace(url) || !url.Contains("."))
                {
                    url = "https://www.google.com";
                }

                Browser.Load(url);

                Task.Delay(200).ContinueWith(_ => _isUserSwitchingTab = false);
            }
        }

        private void UrlTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void UrlTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (!tb.IsKeyboardFocused)
            {
                e.Handled = true;
                tb.Focus();
                tb.SelectAll();
            }
        }

        private void OnBrowserAddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!_isUserSwitchingTab)
                {
                    _viewModel.UpdateAddressFromBrowser(e.NewValue.ToString());
                }
            });
        }

        private void OnBrowserTitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _viewModel.UpdateTitleFromBrowser(e.NewValue.ToString());
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