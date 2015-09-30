using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Diagnostics;

namespace FbSample {
    internal class LoginPage : ContentPage {

        private readonly FacebookClient _fb;
        private readonly MainPage _mainPage;

        public LoginPage(MainPage mainPage, string appId, string extendedPermissions,bool deleteCookie) {

            _mainPage = mainPage;

            Title = "Login"; //ページのタイトル

            _fb = new FacebookClient();

            var exWebView = new ExWebView(deleteCookie) {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            exWebView.Navigating += (s, e) => {
                Debug.WriteLine("■Navigating=" + e.Url);
            };

            //Uri遷移のイベントを処理する
            exWebView.Navigated += (s, e) => {
                Debug.WriteLine("■Navigated="+e.Url);

                //リクエストUriから認証の状態を判断する
                FacebookOAuthResult oauthResult;
                if (!_fb.TryParseOAuthCallbackUrl(new Uri(e.Url), out oauthResult)) {
                    return; //認証継続中
                }
                if (oauthResult.IsSuccess) {
                    //認証成功
                    LoginSucceded(oauthResult.AccessToken);
                } else {
                    //認証失敗
                    LoginSucceded(string.Empty);
                }
            };
            //exWebView.Navigate += request => {
            //    //リクエストUriから認証の状態を判断する
            //    FacebookOAuthResult oauthResult;
            //    if (!_fb.TryParseOAuthCallbackUrl(new Uri(request), out oauthResult)) {
            //        return; //認証継続中
            //    }
            //    if (oauthResult.IsSuccess) {
            //        //認証成功
            //        LoginSucceded(oauthResult.AccessToken);
            //    } else {
            //        //認証失敗
            //        LoginSucceded(string.Empty);
            //    }
            //};

            //認証URLへ移動 (https://www.facebook.com/dialog/oauth)
            exWebView.Source = _fb.GetLoginUrl(appId, extendedPermissions).AbsoluteUri;
            Content = exWebView;
        }


        private async void LoginSucceded(string accessToken) {
            try {
                var fb = new FacebookClient(accessToken);
                var json = await fb.GetTaskAsync("me?fields=id");
                var o = JObject.Parse(json);
                var id = (string)o["id"];
                _mainPage.SetStatus(true, accessToken, id);//ログイン成功
                await Navigation.PopModalAsync();//メインビューへ戻る
            } catch (Exception ex) {
                DisplayAlert("ERROR", ex.Message, "OK");
            }
//            await Navigation.PopAsync();//メインビューへ戻る
        }
    }
}