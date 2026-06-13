using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FieldService.Models;
using Font = iTextSharp.text.Font;
using Element = iTextSharp.text.Element;

namespace FieldService.Documents
{
    public static class ReceiptDocument
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

            var titleFont = new Font(_baseFont, 18, Font.BOLD);
            var boldFont = new Font(_baseFont, 12, Font.BOLD);
            var normalFont = new Font(_baseFont, 12, Font.NORMAL);

            var title = new Paragraph("КВИТАНЦИЯ О ПРИЁМЕ", titleFont)
            { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20 };
            document.Add(title);

            document.Add(new Paragraph($"№ {request.Id} от {request.CreatedAt:dd.MM.yyyy HH:mm}", boldFont)
            { SpacingAfter = 10 });
            document.Add(new Paragraph($"Дата приёма: {request.DateReceived:dd.MM.yyyy}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Клиент: {request.ClientName}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Телефон: {request.Phone}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Услуга: {request.ServiceType}", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph($"Описание: {request.Problem}", normalFont)
            { SpacingAfter = 20 });

            document.Add(new Paragraph("Принял: __________________", normalFont)
            { SpacingAfter = 5 });
            document.Add(new Paragraph("Дата выдачи: ______________", normalFont)
            { SpacingAfter = 25 });

            document.Close();
            return stream.ToArray();
        }
    }
}