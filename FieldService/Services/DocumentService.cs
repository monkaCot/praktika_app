using FieldService.Data;
using FieldService.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _dbContext;

        public DocumentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeTemplatesAsync()
        {
            if (await _dbContext.Templates.AnyAsync())
                return;

            var templates = new[]
            {
            new DocumentTemplate
            {
                Type = "act",
                Name = "Акт выполненных работ",
                HtmlContent = GetActHtmlTemplate(),
                Version = "1.0",
                CreatedAt = DateTime.Now
            },
            new DocumentTemplate
            {
                Type = "order",
                Name = "Заказ-наряд",
                HtmlContent = GetOrderHtmlTemplate(),
                Version = "1.0",
                CreatedAt = DateTime.Now
            }
        };

            await _dbContext.Templates.AddRangeAsync(templates);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> FillTemplateAsync(ServiceRequest request, string templateType)
        {
            var template = await _dbContext.Templates
                .FirstOrDefaultAsync(t => t.Type == templateType);

            if (template == null)
                throw new Exception($"Шаблон типа '{templateType}' не найден");

            var html = template.HtmlContent;
            html = html.Replace("{{Id}}", request.Id.ToString());
            html = html.Replace("{{ClientName}}", request.ClientName);
            html = html.Replace("{{Phone}}", request.Phone ?? "");
            html = html.Replace("{{Address}}", request.Address ?? "");
            html = html.Replace("{{Problem}}", request.Problem ?? "");
            html = html.Replace("{{Status}}", request.Status ?? "");
            html = html.Replace("{{CreatedAt}}", request.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
            return html;
        }

        public async Task PrintDocumentAsync(string html)
        {
            var fileName = $"Документ_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(filePath, html);
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Документ",
                File = new ShareFile(filePath, "text/html")
            });
        }

        private static string GetActHtmlTemplate() => @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Акт выполненных работ</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 2cm; }
        h1 { text-align: center; }
        .field { margin: 10px 0; }
        .label { font-weight: bold; }
    </style>
</head>
<body>
    <h1>АКТ ВЫПОЛНЕННЫХ РАБОТ</h1>
    <p><strong>№ {{Id}}</strong> от {{CreatedAt}}</p>
    <div class='field'><span class='label'>Клиент:</span> {{ClientName}}</div>
    <div class='field'><span class='label'>Телефон:</span> {{Phone}}</div>
    <div class='field'><span class='label'>Адрес:</span> {{Address}}</div>
    <div class='field'><span class='label'>Описание работ:</span> {{Problem}}</div>
    <div class='field'><span class='label'>Статус:</span> {{Status}}</div>
    <hr />
    <p>Мастер: __________________</p>
    <p>Клиент: __________________</p>
</body>
</html>";

        private static string GetOrderHtmlTemplate() => @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Заказ-наряд</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 2cm; }
        h1 { text-align: center; }
    </style>
</head>
<body>
    <h1>ЗАКАЗ-НАРЯД № {{Id}}</h1>
    <p>Дата: {{CreatedAt}}</p>
    <p>Клиент: {{ClientName}}</p>
    <p>Телефон: {{Phone}}</p>
    <p>Адрес: {{Address}}</p>
    <p>Задача: {{Problem}}</p>
    <p>Статус выполнения: {{Status}}</p>
    <hr />
    <p>Принял: ___________</p>
    <p>Сдал: ___________</p>
</body>
</html>";
    }
}