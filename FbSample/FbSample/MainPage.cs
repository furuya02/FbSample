using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace FbSample{



    class MainPage :ContentPage{

        private bool _isLogin = false;
        private FacebookClient _fb = null;
        private Image _img;
        private Graph _graph = null;
        bool deleteCookie = false;

        //要求するアクセス許可                                                
        private const string ExtendedPermissions = "user_friends,user_posts";

        public MainPage(){
                    
            Title = "Xamarin.Forms FbSample"; //ページのタイトル
            NavigationPage.SetBackButtonTitle(this, "");// 戻るボタンのテキスト（空白）

            var menu = new List<OneMenu>();

            menu.Add(new OneMenu() {
                Title = "先読み自分新聞",
                Picture = ImageSource.FromResource("FbSample.image.000.png"),
                Detail = "年末恒例の「自分新聞」を先読みする！",
            });
            menu.Add(new OneMenu() {
                Title = "たくさんの「いいね」をありがとう",
                Picture = ImageSource.FromResource("FbSample.image.001.png"),
                Detail = "「いいね」を贈ってくださった方のランキング"
            });
            menu.Add(new OneMenu() {
                Title = "最近、何書いたっけ",
                Picture = ImageSource.FromResource("FbSample.image.002.png"),
                Detail = "投稿したメッセージなどの一覧"
            });
            menu.Add(new OneMenu() {
                Title = "脊髄反応ページ",
                Picture = ImageSource.FromResource("FbSample.image.003.png"),
                Detail = "最近、「いいね」」したリンクの一覧"
            });
            menu.Add(new OneMenu() {
                Title = "いつ寝るの",
                Picture = ImageSource.FromResource("FbSample.image.004.png"),
                Detail = "時間別の投稿数のグラフ"
            });
            menu.Add(new OneMenu() {
                Title = "Facebook依存月間",
                Picture = ImageSource.FromResource("FbSample.image.005.png"),
                Detail = "月別の投稿件数グラフ"
            });
            menu.Add(new OneMenu() {
                Title = "リフレッシュ",
                Picture = ImageSource.FromResource("FbSample.image.006.png"),
                Detail = "Facebook Graphからの最新のデータを取得する"
            });
            menu.Add(new OneMenu() {
                Title = "ログアウト",
                Picture = ImageSource.FromResource("FbSample.image.007.png"),
                Detail = ""
            });

            _img = new Image() {
                WidthRequest = 80,
                HeightRequest = 80
            };

            // 起動時はログアウト状態
            SetStatus(false, "", "");

            //リストビュー(メニュー)の生成
            var listViewMenu = new ListView {
                ItemsSource = menu,
                ItemTemplate = new DataTemplate(typeof(MyCell)),//セルの指定
                RowHeight = 60, // 行の高さ固定
                SeparatorColor = Color.Transparent
            };

            //リストビュー(メニュー)選択時の処理
            listViewMenu.ItemSelected += async (s, a) => {
                if (a.SelectedItem != null) {
                    var o = (OneMenu) a.SelectedItem;
                    switch (menu.IndexOf(o)) {
                        case 0:// 先読み自分新聞
                            await Navigation.PushAsync(new SakiyomiPage(_graph, o.Title));
                            break;
                        case 1:
                            await Navigation.PushAsync(new LikesPage(_graph, o.Title));
                            break;
                        case 2:
                            await Navigation.PushAsync(new PostPage(_graph, o.Title));
                            break;
                        case 3:
                            await Navigation.PushAsync(new NicePage(_graph, o.Title));
                            break;
                        case 4:
                            await Navigation.PushAsync(new DatePage(_graph, o.Title, true));
                            break;
                        case 5:
                            await Navigation.PushAsync(new DatePage(_graph, o.Title, false));
                            break;
                        case 6:// リフレッシュ
                            _graph.Refresh(true);
                            break;
                        case 7:// ログアウト
                            deleteCookie = true;
                            SetStatus(false, "", "");//いったんログアウト状態に初期化する
                            break;

                    }
                    listViewMenu.SelectedItem = null; //メニュー選択の解除
                }
            };


            //メインページの画面構成
            Content = new StackLayout {
                Padding = new Thickness(20,10, 20, 0),
                Children = { _img,listViewMenu }
            };
        }

        //セル用のテンプレート
        private class MyCell : ViewCell {
            public MyCell() {

                var picture = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 60, HeightRequest = 60 };
                picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                picture.SetBinding(Image.SourceProperty, "Picture");

                var title = new Label { Font = Font.SystemFontOfSize(14), TextColor = Color.Black };
                title.SetBinding(Label.TextProperty, "Title");


                var detail = new Label { Font = Font.SystemFontOfSize(9), TextColor = Color.Gray };
                detail.SetBinding(Label.TextProperty, "Detail");

                var likesSub = new StackLayout {
                    Children = { title, detail }
                };

                View = new StackLayout {
                    Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { picture, likesSub }
                };
            }
        }

        class OneMenu {
            public string Title { get; set; }
            public string Detail { get; set; }
            public ImageSource Picture { get; set; }
        }



        public void SetStatus(bool isLogin, string accessToken, string id) {
            _isLogin = isLogin;
            if (_isLogin) {
                _fb = new FacebookClient(accessToken);
                _img.Source = string.Format("https://graph.facebook.com/{0}/picture?width=100&height=100", id);
                _graph = new Graph(_fb);
            } else {
                _fb = null;
                Navigation.PushModalAsync(new LoginPage(this, "YOUR_FACEBOOK_APP_ID", ExtendedPermissions,deleteCookie));
                deleteCookie = false;
            }

        }
    }
}
