using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using FieldService.Data;
using FieldService.Models;

namespace FieldService.ViewModels;

public partial class RequestsViewModel : ObservableObject
{
    private readonly AppDbContext _db;

    public ObservableCollection<ServiceRequest> Requests { get; } = new();

    public RequestsViewModel(AppDbContext db)
    {
        _db = db;
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
}