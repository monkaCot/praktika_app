using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FieldService.Models;
using Font = iTextSharp.text.Font;
using Element = iTextSharp.text.Element;

namespace FieldService.Documents
{
    public static class ActDocument
    {
        private static BaseFont _baseFont;

        private static void InitFont()
        {
            if (_baseFont != null) return;
            string fontPath = Path.Combine(FileSystem.AppDataDirectory, "OpenSans-Regular.ttf");
            _baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public static byte[] Generate(ServiceRequest request)
        {
            InitFont();

            using var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(document, stream);
            document.Open();

            var titleFont = new Font(_baseFont, 20, Font.BOLD);
            var boldFont = new Font(_baseFont, 12, Font.BOLD);
            var normalFont = new Font(_baseFont, 12, Font.NORMAL);
            var footerFont = new Font(_baseFont, 8, Font.NORMAL);

            var title = new Paragraph("АКТ ВЫПОЛНЕННЫХ РАБОТ", titleFont)
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20 };
            document.Add(title);

            document.Add(new Paragraph($"№ {request.Id} от {request.CreatedAt:dd.MM.yyyy HH:mm}", boldFont)
            { SpacingAfter = 10 });
            document.Add(new Paragraph($"Клиент: {request.ClientName}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Телефон: {request.Phone}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Адрес: {request.Address}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Описание работ: {request.Problem}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Статус: {request.Status}", normalFont)
            { SpacingAfter = 20 });

            document.Add(new Paragraph("Мастер: __________________", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph("Клиент: __________________", normalFont)
            { SpacingAfter = 25 });

            var footer = new Paragraph($"Сформировано в FieldService {DateTime.Now:dd.MM.yyyy HH:mm}", footerFont)
            { Alignment = Element.ALIGN_CENTER };
            document.Add(footer);

            document.Close();
            return stream.ToArray();
        }
    }
}