using System.Windows;

namespace AeroSurf
{
    public partial class View : Window
    {
        private readonly Controller controller;

        public View()
        {
            InitializeComponent();
            controller = new Controller(new Model(), this);
            Loaded += OnLoaded; // Attach event handler
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Raise the Initialize event
            Initialize?.Invoke(this, e);
        }

        // Define the Initialize event
        public event RoutedEventHandler Initialize;
    }
}
