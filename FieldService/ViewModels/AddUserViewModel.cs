using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Models;
using FieldService.Services;

namespace FieldService.ViewModels;

public partial class AddUserViewModel : ObservableObject
{
    private readonly AuthService _auth;

    [ObservableProperty] private string _fullName;
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    [ObservableProperty] private UserRole _selectedRole = UserRole.Technician;
    [ObservableProperty] private string _message;

    public List<UserRole> AvailableRoles { get; } = new() { UserRole.Technician, UserRole.Admin };

    public AddUserViewModel(AuthService auth)
    {
        _auth = auth;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Message = "";

        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            Message = "Заполните все поля";
            return;
        }

        bool ok = await _auth.CreateUserAsync(Username, Password, FullName, SelectedRole);
        if (!ok)
        {
            Message = "Логин уже занят";
            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}