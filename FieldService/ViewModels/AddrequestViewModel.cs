using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Data;
using FieldService.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Net;

namespace FieldService.ViewModels;

public partial class AddRequestViewModel : ObservableObject
{
    private readonly AppDbContext _db;

    [ObservableProperty] private string _clientName;
    [ObservableProperty] private string _phone;
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _problem;

    public AddRequestViewModel(AppDbContext db)
    {
        _db = db;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(ClientName))
        {
            await Shell.Current.DisplayAlert("Проверьте данные", "Укажите имя клиента", "Ок");
            return;
        }

        var request = new ServiceRequest
        {
            ClientName = ClientName,
            Phone = Phone,
            Address = Address,
            Problem = Problem,
            Status = "Новая",
            CreatedAt = DateTime.Now
        };

        _db.Requests.Add(request);
        await _db.SaveChangesAsync();

        await Shell.Current.GoToAsync("..");
    }
}