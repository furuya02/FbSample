using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FbSample;
using FbSample.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Webkit;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FbSample.Droid {
    public class ExWebViewRenderer : WebViewRenderer {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(Android.Webkit.WebView)
            //var webView = ResourceBundle.Control;
            //var webView = this.Control; //Ver1.4.x

            //WebViewではNavigateのイベントを拾えないためWebViewClientを上書きする
            //webView.SetWebViewClient(new MyWebViewClient(exWebView));


            //クッキー（ログイン情報）の削除
            if (exWebView.DeleteCookie) {
                CookieManager.Instance.RemoveAllCookie();
            }

        }
    }
    //public class MyWebViewClient : WebViewClient {
    //    private readonly ExWebView _exWebView;

    //    public MyWebViewClient(ExWebView exWebView) {
    //        _exWebView = exWebView;
    //    }
    //    public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url) {
    //        //イベントをForms側に送る
    //        _exWebView.OnNavigate(url);
    //        return false;
    //    }
    //}
}

