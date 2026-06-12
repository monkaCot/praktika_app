using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Services;

namespace FieldService.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthService _auth;

    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _fullName;
    [ObservableProperty] private string _message;

    public RegisterViewModel(AuthService auth)
    {
        _auth = auth;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        Message = "";

        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(FullName))
        {
            Message = "Заполните все поля";
            return;
        }

        bool ok = await _auth.RegisterAsync(Username, Password, FullName);
        if (!ok)
        {
            Message = "Этот логин уже занят";
            return;
        }

        await Shell.Current.DisplayAlert("Готово", "Аккаунт создан, теперь войдите", "Ок");
        await Shell.Current.GoToAsync("..");
    }
}