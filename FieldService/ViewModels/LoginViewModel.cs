using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Services;

namespace FieldService.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _auth;

        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _errorMessage;

        public LoginViewModel(AuthService auth)
        {
            _auth = auth;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите логин и пароль";
                return;
            }

            bool ok = await _auth.LoginAsync(Username, Password);
            if (!ok)
            {
                ErrorMessage = "Неверный логин или пароль";
                return;
            }

            // Вход успешен переходим в основное приложение
            await Shell.Current.GoToAsync("//main");
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
