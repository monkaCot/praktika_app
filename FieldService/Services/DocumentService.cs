using FieldService.Data;
using FieldService.Models;
using Microsoft.EntityFrameworkCore;
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

        // Замена переменных в HTML
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

        public Task PrintDocumentAsync(string html)
        {
            return Task.CompletedTask;
        }

        private static string GetActHtmlTemplate()
        {
            string v = "Clear";
            return v;
        }

        private static string GetOrderHtmlTemplate()
        {
            string v = "Clear";
            return v;
        }
    }
}