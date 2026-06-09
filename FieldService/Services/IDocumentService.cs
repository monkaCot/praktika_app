using FieldService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Services
{
    public interface IDocumentService
    {
        Task InitializeTemplatesAsync();
        Task<string> FillTemplateAsync(ServiceRequest request, string templateType);
        Task PrintDocumentAsync(string html);
    }
}
