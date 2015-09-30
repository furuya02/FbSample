using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class PostPage : BasePage {
        public PostPage(Graph graph, string title) : base(graph, title) {
            var ar = new List<OneData>();

            // 「いいね」数の多さでソートする
            _graph.post.Sort((a, b) => b.Likes.Count - a.Likes.Count);
            foreach (var a in _graph.stream) {
                if (String.IsNullOrEmpty(a.Message)) {
                    continue;
                }

                var createdTime = Util.ToDateStr(a.Created_time);
                var type = a.Type.ToString();
                var isPhoto = false;
                var isLink = false;
                var isUpdate = false;

                switch (a.Type) {
                    case 80:
                        type = "リンクの投稿";
                        isLink = true;
                        break;
                    case 247:
                        type = "写真の投稿";
                        isPhoto = true;
                        break;
                    case 46:
                        type = "記事の投稿";
                        isUpdate = true;
                        break;


                }
                ar.Add(new OneData() { Message = a.Message, CreateTime = createdTime, Type = type ,IsLink = isLink,IsPhoto = isPhoto ,IsUpdate = isUpdate});
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
                var picturePhoto = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 30, HeightRequest = 30 };
                picturePhoto.Source = ImageSource.FromResource("FbSample.image.photo.png");
                picturePhoto.SetBinding(Image.IsVisibleProperty, "IsPhoto");

                var pictureLink = new Image() { Aspect = Aspect.AspectFit,WidthRequest=30,HeightRequest=30 };
                pictureLink.Source = ImageSource.FromResource("FbSample.image.link.png");
                pictureLink.SetBinding(Image.IsVisibleProperty, "IsLink");

                var pictureUpdate = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 30, HeightRequest = 30 };
                pictureUpdate.Source = ImageSource.FromResource("FbSample.image.update.png");
                pictureUpdate.SetBinding(Image.IsVisibleProperty, "IsUpdate");

                //type
                var type = new Label { Font = Font.SystemFontOfSize(12), TextColor = Color.Black };
                type.SetBinding(Label.TextProperty, "Type");

                //作成日時
                var createTime = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Gray };
                createTime.SetBinding(Label.TextProperty, "CreateTime");


                //メッセージ
                var message = new Label { Font = Font.SystemFontOfSize(9), TextColor = Color.Black };
                message.SetBinding(Label.TextProperty, "Message");

                var subLayout = new StackLayout {
                    Padding = new Thickness(1),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { pictureUpdate,picturePhoto, pictureLink, type }                };


                View = new StackLayout {
                    Padding = new Thickness(5),
                    Children = { subLayout, createTime, message }
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
                Height = row * 10 + 65;
                if (Height < 60) {
                    Height = 60;//列の高さは、最低でも60とする
                }
            }
        }


        class OneData {
            public string Message { get; set; }
            public string CreateTime { get; set; }
            public string Type { get; set; }
            public bool IsLink { get; set; }
            public bool IsPhoto { get; set; }
            public bool IsUpdate { get; set; }
        }
    }
}
