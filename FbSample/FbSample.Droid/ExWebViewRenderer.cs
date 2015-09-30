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

            //Xamarin.Form�̃R���g���[��(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //�l�C�e�B�u�R���g���[��(Android.Webkit.WebView)
            //var webView = ResourceBundle.Control;
            //var webView = this.Control; //Ver1.4.x

            //WebView�ł�Navigate�̃C�x���g���E���Ȃ�����WebViewClient���㏑������
            //webView.SetWebViewClient(new MyWebViewClient(exWebView));


            //�N�b�L�[�i���O�C�����j�̍폜
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
    //        //�C�x���g��Forms���ɑ���
    //        _exWebView.OnNavigate(url);
    //        return false;
    //    }
    //}
}

