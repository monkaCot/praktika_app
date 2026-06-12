using FieldService.ViewModels;

namespace FieldService;

public partial class UsersPage : ContentPage
{
    public UsersPage(UsersViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((UsersViewModel)BindingContext).LoadUsersCommand.ExecuteAsync(null);
    }
}