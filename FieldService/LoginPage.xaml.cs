using FieldService.ViewModels;

namespace FieldService;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}