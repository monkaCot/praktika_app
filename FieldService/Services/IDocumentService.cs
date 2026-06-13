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
        Task PrintDocumentAsync(ServiceRequest request, string templateType);
    }
}
