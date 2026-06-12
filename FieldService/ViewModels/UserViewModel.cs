using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using FieldService.Data;
using FieldService.Models;

namespace FieldService.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly AppDbContext _db;

    public ObservableCollection<User> Users { get; } = new();

    public UsersViewModel(AppDbContext db)
    {
        _db = db;
    }

    [RelayCommand]
    private async Task LoadUsersAsync()
    {
        Users.Clear();
        var items = await _db.Users.ToListAsync();
        foreach (var u in items)
            Users.Add(u);
    }

    [RelayCommand]
    private async Task GoToAddUserAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}