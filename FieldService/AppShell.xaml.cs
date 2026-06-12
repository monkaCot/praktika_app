namespace FieldService
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddRequestPage), typeof(AddRequestPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        public void ApplyRole(FieldService.Models.UserRole role)
        {
            TabUsers.IsVisible = role == FieldService.Models.UserRole.Admin;
        }
    }
}
