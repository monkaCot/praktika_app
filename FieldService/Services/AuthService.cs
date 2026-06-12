using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Models;
using Microsoft.EntityFrameworkCore;

namespace FieldService.Services
{
    public class AuthService
    {
        private readonly IServiceProvider _services;

        public User CurrentUser { get; private set; }

        public AuthService(IServiceProvider services)
        {
            _services = services;
        }

        // Регистрация нового пользователя
        public async Task<bool> RegisterAsync(string username, string password, string fullName)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            bool exists = await db.Users.AnyAsync(u => u.Username == username);
            if (exists)
                return false;

            var user = new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Role = UserRole.Client
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return true;
        }

        // Создание пользователя админом с выбором роли.
        public async Task<bool> CreateUserAsync(string username, string password, string fullName, UserRole role)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (await db.Users.AnyAsync(u => u.Username == username))
                return false;

            db.Users.Add(new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Role = role
            });
            await db.SaveChangesAsync();
            return true;
        }

        // Вход
        public async Task<bool> LoginAsync(string username, string password)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || user.Password != password)
                return false;

            CurrentUser = user;
            return true;
        }

        // Выход
        public void Logout()
        {
            CurrentUser = null;
        }

        // Создаём админа при первом запуске если отсутствуют пользователи
        public async Task SeedAdminAsync()
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (await db.Users.AnyAsync())
                return;

            db.Users.Add(new User
            {
                Username = "admin",
                Password = "admin",
                FullName = "Администратор",
                Role = UserRole.Admin
            });
            await db.SaveChangesAsync();
        }
    }
}
