using FieldService.Models;
using FieldService.Services;
using FieldService.ViewModels;

namespace FieldService;

public partial class MainPage : ContentPage
{
    private readonly AuthService _auth;

    public MainPage(RequestsViewModel vm, AuthService auth)
    {
        InitializeComponent();
        BindingContext = vm;
        _auth = auth;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool isTechnician = _auth.CurrentUser?.Role == UserRole.Technician;

        if (isTechnician && ToolbarItems.Contains(AddButton))
            ToolbarItems.Remove(AddButton);
        else if (!isTechnician && !ToolbarItems.Contains(AddButton))
            ToolbarItems.Add(AddButton);

        await ((RequestsViewModel)BindingContext).LoadRequestsCommand.ExecuteAsync(null);
    }
}