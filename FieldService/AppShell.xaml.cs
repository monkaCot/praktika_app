namespace FieldService
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddRequestPage), typeof(AddRequestPage));
        }
    }
}
