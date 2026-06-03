using FieldService.ViewModels;

namespace FieldService;

public partial class AddRequestPage : ContentPage
{
    public AddRequestPage(AddRequestViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}