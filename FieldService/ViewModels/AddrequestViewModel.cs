using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Data;
using FieldService.Documents;
using FieldService.Models;
using FieldService.Services;
using System.IO;

namespace FieldService.ViewModels;

public partial class AddRequestViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;  // добавлено

    [ObservableProperty] private string _clientName;
    [ObservableProperty] private string _phone;
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _problem;
    [ObservableProperty] private string _selectedService;
    [ObservableProperty] private DateTime _dateReceived = DateTime.Today;

    public List<string> AvailableServices { get; }

    public AddRequestViewModel(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;

        AvailableServices = new List<string>
        {
            "Ремонт ПК",
            "Ремонт ноутбука",
            "Установка ПО",
            "Диагностика",
            "Чистка от пыли",
            "Замена термопасты"
        };
        SelectedService = AvailableServices.First();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(ClientName))
        {
            await Shell.Current.DisplayAlert("Проверьте данные", "Укажите имя клиента", "Ок");
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedService))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите услугу", "ОК");
            return;
        }

        var request = new ServiceRequest
        {
            ClientName = ClientName,
            Phone = Phone,
            Address = Address,
            Problem = Problem,
            Status = "Новая",
            CreatedAt = DateTime.Now,
            DateReceived = DateReceived,
            ServiceType = SelectedService
        };

        _db.Requests.Add(request);
        await _db.SaveChangesAsync();

        await GenerateReceiptAsync(request);
        await Shell.Current.GoToAsync("..");
    }

    private async Task GenerateReceiptAsync(ServiceRequest request)
    {
        try
        {
            var pdfBytes = ReceiptDocument.Generate(request);
            string fileName = $"Квитанция_{request.Id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Квитанция о приёме",
                File = new ShareFile(filePath, "application/pdf")
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось создать квитанцию: {ex.Message}", "OK");
        }
    }
}