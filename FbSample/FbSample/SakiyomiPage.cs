using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class SakiyomiPage:BasePage{


        public SakiyomiPage(Graph graph,string title) :base(graph, title) {

            var ar = new List<OneData>();

            // 「いいね」数の多さでソートする
            _graph.post.Sort((a, b) => b.Likes.Count - a.Likes.Count);
            foreach (var a in _graph.post) {
                if(a.Type == "photo") { // 画像投稿のページだけを対象にする
                    ar.Add(new OneData() { Message = a.Message, Picture = a.Picture, CreateTime = a.CreateTime, Likes = a.Likes.Count });
                }
            }

            var listView = new ListView() {
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(MyCell)),//セルの指定
                ItemsSource = ar,
                //RowHeight = 100, // 行の高さ固定
                HasUnevenRows = true,//行の高さを可変とする
            };

            Content = new StackLayout {
                Padding = new Thickness(0, 0, 0, 0),
                Children = { listView }
            };
        }

        //セル用のテンプレート
        private class MyCell : ViewCell {
            public MyCell() {

                //画像
                var picture = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 100,HeightRequest=100 };
                picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                picture.SetBinding(Image.SourceProperty, "Picture");

                //作成日時
                var createTime = new Label { Font = Font.SystemFontOfSize(8), TextColor = Color.Gray };
                createTime.SetBinding(Label.TextProperty, "CreateTime");

                //いいね数
                var likes = new Label { Font = Font.SystemFontOfSize(13), TextColor = Color.Black };
                likes.SetBinding(Label.TextProperty, "Likes");

                //いいね
                var label = new Label { Font = Font.SystemFontOfSize(13), TextColor = Color.Black, Text = "いいね" };

                //メッセージ
                var message = new Label { Font = Font.SystemFontOfSize(9), TextColor = Color.Black };
                message.SetBinding(Label.TextProperty, "Message");

                //いいね行
                var likesSub = new StackLayout {
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { likes, label, createTime }
                };

                var layoutSub = new StackLayout {
                    Spacing = 0,//スペースなし
                    Children = { likesSub, message }//縦に並べる
                };

                View = new StackLayout {
                    Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { picture, layoutSub } //アイコンとサブレイアウトを横に並べる
                };
            }

            //テキストの長さに応じて行の高さを増やす
            protected override void OnBindingContextChanged() {
                base.OnBindingContextChanged();

                //メッセージ
                var message = ((OneData)BindingContext).Message;
                //メッセージを改行で区切って、各行の最大文字数を27として行数を計算する（27文字は、日本を基準にしました）
                var row = 1;
                if (message != null) {
                    row = message.Split('\n').Select(l => l.Length / 27).Select(c => c + 1).Sum();
                }
                Height = row * 10 + 33;
                if (Height < 60) {
                    Height = 100;//列の高さは、最低でも100とする
                }
            }
        }


        class OneData {
            public string Message { get; set; }
            public string Picture { get; set; }
            public string CreateTime { get; set; }
            public int Likes { get; set; }
        }

    }
}
