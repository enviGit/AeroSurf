using CefSharp;
using CefSharp.Wpf;
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
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            var browser = sender as ChromiumWebBrowser;
            if (browser == null) return;

            browser.RequestHandler = new CustomRequestHandler();

            var tabViewModel = browser.DataContext as TabItemViewModel;
            if (tabViewModel != null)
            {
                tabViewModel.BrowserInstance = browser;
            }
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var browser = sender as ChromiumWebBrowser;
            var tabViewModel = browser.DataContext as TabItemViewModel;
            string newUrl = e.NewValue.ToString();

            if (tabViewModel != null)
            {
                tabViewModel.Url = newUrl;

                if (tabViewModel.IsSelected)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (!UrlTextBox.IsKeyboardFocused)
                        {
                            _viewModel.Address = newUrl;
                        }

                        _viewModel.UpdateInfoFromBrowser(newUrl, null);
                    });
                }
            }
        }

        private void Browser_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var browser = sender as ChromiumWebBrowser;
            var tabViewModel = browser.DataContext as TabItemViewModel;
            string newTitle = e.NewValue.ToString();

            if (tabViewModel != null)
            {
                tabViewModel.Title = newTitle;
                if (tabViewModel.IsSelected)
                {
                    Dispatcher.Invoke(() =>
                    {
                        _viewModel.Title = "AeroSurf - " + newTitle;
                        _viewModel.UpdateInfoFromBrowser(null, newTitle);
                    });
                }
            }
        }

        private void UrlTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => (sender as TextBox).SelectAll();

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

        private void MenuBtn_Click(object sender, RoutedEventArgs e) => (sender as Button).ContextMenu.IsOpen = true;
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        private void Maximize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}