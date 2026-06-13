using FieldService.Data;
using FieldService.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Documents;

namespace FieldService.Services
{
    public class DocumentService : IDocumentService
    {
        public async Task PrintDocumentAsync(ServiceRequest request, string templateType)
        {
            try
            {
                byte[] pdfBytes = templateType == "act"
                    ? ActDocument.Generate(request)
                    : OrderDocument.Generate(request);

                var fileName = $"Документ_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, pdfBytes);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = templateType == "act" ? "Акт выполненных работ" : "Заказ-наряд",
                    File = new ShareFile(filePath, "application/pdf")
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка генерации PDF: {ex.Message}");
            }
        }
    }
}