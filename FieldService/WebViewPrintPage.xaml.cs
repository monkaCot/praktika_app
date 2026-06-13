using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;

namespace FieldService
{
    public partial class WebViewPrintPage : ContentPage
    {
        private readonly string _htmlFilePath;
        private readonly string _pageTitle;

        public WebViewPrintPage(string htmlFilePath, string title)
        {
            InitializeComponent();
            _htmlFilePath = htmlFilePath;
            _pageTitle = title;
            Title = title;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Загружаем локальный HTML
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = await File.ReadAllTextAsync(_htmlFilePath);
            WebViewControl.Source = htmlSource;
        }

        private async void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            LoadingIndicator.IsVisible = false;
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem("Печать", "printer.png", async () =>
            {
#if ANDROID
                if (WebViewControl.Handler?.PlatformView is Android.Webkit.WebView androidWebView)
                {
                    var printManager = Android.App.Application.Context.GetSystemService(Android.Content.Context.PrintService) as Android.Print.PrintManager;
                    var printAdapter = androidWebView.CreatePrintDocumentAdapter(_pageTitle);
                    printManager.Print(_pageTitle, printAdapter, null);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Печать не поддерживается на этой платформе", "OK");
                }
#else
        await DisplayAlert("Информация", "Печать доступна только на Android", "OK");
#endif
            }));
        }

        private void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            LoadingIndicator.IsVisible = true;
        }
    }
}