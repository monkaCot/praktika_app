using Android.OS;
using Android.Print;
using Android.Webkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebView = Android.Webkit.WebView;

public class HtmlPrintDocumentAdapter : PrintDocumentAdapter
{
    private readonly string _html;
    private WebView? _webView;
    private PrintAttributes? _attributes;
    private LayoutResultCallback? _layoutCallback;

    public HtmlPrintDocumentAdapter(string html)
    {
        _html = html;
    }

    public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes,
        CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
    {
        if (cancellationSignal.IsCanceled)
        {
            callback.OnLayoutCancelled();
            return;
        }

        _attributes = newAttributes;
        _layoutCallback = callback;

        _webView = new WebView(Platform.CurrentActivity);
        _webView.SetWebViewClient(new WebViewClientImpl(this));
        _webView.LoadDataWithBaseURL(null, _html, "text/html", "UTF-8", null);
    }

    private class WebViewClientImpl : WebViewClient
    {
        private readonly HtmlPrintDocumentAdapter _adapter;

        public WebViewClientImpl(HtmlPrintDocumentAdapter adapter)
        {
            _adapter = adapter;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            var printInfo = new PrintDocumentInfo.Builder("document.pdf")
                .SetContentType(PrintContentType.Document)
                .Build();
            _adapter._layoutCallback?.OnLayoutFinished(printInfo, true);
        }
    }

    public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination,
        CancellationSignal cancellationSignal, WriteResultCallback callback)
    {
        if (_webView == null || _attributes == null)
        {
            callback.OnWriteFailed("WebView not initialized");
            return;
        }

        var printAdapter = _webView.CreatePrintDocumentAdapter("document");
        printAdapter.OnWrite(pages, destination, cancellationSignal, callback);
    }
}
