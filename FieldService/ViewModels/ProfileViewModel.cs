using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Services;
using System.Data;

namespace FieldService.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly AuthService _auth;

    [ObservableProperty] private string _fullName;
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _role;

    public ProfileViewModel(AuthService auth)
    {
        _auth = auth;
    }

    public void Refresh()
    {
        var u = _auth.CurrentUser;
        FullName = u?.FullName ?? "";
        Username = u?.Username ?? "";
        Role = u?.Role.ToString() ?? "";
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        _auth.Logout();
        await Shell.Current.GoToAsync("//login");
    }
}