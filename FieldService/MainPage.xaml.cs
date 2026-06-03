using FieldService.ViewModels;

namespace FieldService;

public partial class MainPage : ContentPage
{
    public MainPage(RequestsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((RequestsViewModel)BindingContext).LoadRequestsCommand.ExecuteAsync(null);
    }
}