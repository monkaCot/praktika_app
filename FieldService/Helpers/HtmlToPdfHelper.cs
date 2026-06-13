using System.IO;
using Microsoft.Maui.Storage;

namespace FieldService.Helpers
{
    public static class HtmlToPdfHelper
    {
        public static async Task PrintHtmlAsync(string htmlContent, string title)
        {
            string htmlFile = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.html");
            await File.WriteAllTextAsync(htmlFile, htmlContent, System.Text.Encoding.UTF8);
            await Shell.Current.Navigation.PushAsync(new WebViewPrintPage(htmlFile, title));
        }
    }
}