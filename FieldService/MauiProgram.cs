using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FieldService.Data;
using FieldService.ViewModels;
using FieldService.Services;

namespace FieldService
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddTransient<RequestsViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddRequestViewModel>();
            builder.Services.AddTransient<AddRequestPage>();
            builder.Services.AddSingleton<IDocumentService, DocumentService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<UsersViewModel>();
            builder.Services.AddTransient<UsersPage>();
            builder.Services.AddTransient<AddUserViewModel>();
            builder.Services.AddTransient<AddUserPage>();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var auth = scope.ServiceProvider.GetRequiredService<AuthService>();
                auth.SeedAdminAsync().GetAwaiter().GetResult();
            }
            return app;
        }
    }
}