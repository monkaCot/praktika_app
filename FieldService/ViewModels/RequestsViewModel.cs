using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldService.Data;
using FieldService.Models;
using FieldService.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace FieldService.ViewModels;

public partial class RequestsViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly IDocumentService _documentService;

    public ObservableCollection<ServiceRequest> Requests { get; } = new();

    public RequestsViewModel(AppDbContext db, IDocumentService documentService)
    {
        _db = db;
        _documentService = documentService;
    }

    [RelayCommand]
    private async Task LoadRequestsAsync()
    {
        Requests.Clear();

        var items = await _db.Requests
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        foreach (var item in items)
            Requests.Add(item);
    }

    [RelayCommand]
    private async Task GoToAddAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddRequestPage));
    }
    [RelayCommand]
    private async Task GenerateDocumentAsync(ServiceRequest request)
    {
        if (request == null) return;

        string[] options = { "Акт выполненных работ", "Заказ-наряд" };
        var action = await Shell.Current.DisplayActionSheet(
            "Выберите тип документа",
            "Отмена",
            null,
            options);

        if (action == "Отмена" || string.IsNullOrEmpty(action))
            return;

        string templateType = action == "Акт выполненных работ" ? "act" : "order";

        try
        {
            var filledHtml = await _documentService.FillTemplateAsync(request, templateType);
            await _documentService.PrintDocumentAsync(filledHtml);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
}