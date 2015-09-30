using System;
using FbSample;
using FbSample.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FbSample.iOS {
    public class ExWebViewRenderer : WebViewRenderer {

        
        protected override void OnElementChanged(VisualElementChangedEventArgs e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(MonoTouch.UIKit.UIWebView)
            //var webView = this;

            // これは、例外で落ちるため使用できない
            //webView.ShouldStartLoad = (w, request, naviType) => {
            //    //イベントをForms側に送る
            //    exWebView.OnNavigate(request.Url.AbsoluteString);
            //    return true;
            //};

            //if(e.OldElement == null) { // perform initial setup
            //    base.Delegate = new MyWebViewDelegate(webView) { ExWebView = exWebView };
            //}

            if (exWebView.DeleteCookie) {
                //クッキー（ログイン情報）の削除
                var storage = NSHttpCookieStorage.SharedStorage;
                foreach (var cookie in storage.Cookies) {
                    storage.DeleteCookie(cookie);
                }
            }

        }

    }

    //public class MyWebViewDelegate : UIWebViewDelegate {
    //    private UIWebView _view;
    //    public ExWebView ExWebView { get; set; }
    //    public MyWebViewDelegate(UIWebView view) {
    //        _view = view;
    //    }

    //    public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType) {
    //        //イベントをForms側に送る
    //        ExWebView.OnNavigate(request.Url.AbsoluteString);
    //        return true;
    //    }
    //}

}

