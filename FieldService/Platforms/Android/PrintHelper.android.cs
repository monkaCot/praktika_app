using Android.Content;
using Android.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Platforms.Android
{
    public static partial class PrintHelper
    {
        public static async Task PrintHtmlAsync(string html)
        {
            var activity = Platform.CurrentActivity;
            var printManager = activity.GetSystemService(Context.PrintService) as PrintManager;
            var adapter = new HtmlPrintDocumentAdapter(html);
            printManager?.Print("Документ", adapter, null);
            await Task.CompletedTask;
        }
    }
}
