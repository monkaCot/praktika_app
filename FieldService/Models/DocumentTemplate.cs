using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Models
{
    public class DocumentTemplate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string HtmlContent { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
