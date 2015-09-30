using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {

    class LikesPage : BasePage {


        public LikesPage(Graph graph, string title) : base(graph, title) {

            var ar = new List<OneData>();

            foreach (var a in _graph.post) {
                foreach (var s in a.Likes) {
                    var target = Search(ar, s);
                    if(target == null) {
                        var picture = string.Format("https://graph.facebook.com/{0}/picture?type=square",s);
                        ar.Add(new OneData() { Id = s ,Likes = 1 ,Picture = picture});
                    } else {
                        target.Likes++;
                    }
                }
            }
            ar.Sort((a, b) =>  b.Likes - a.Likes);

            var listView = new ListView() {
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(MyCell)),//セルの指定
                ItemsSource = ar,
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
                var picture = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 50,HeightRequest=50 };
                picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                picture.SetBinding(Image.SourceProperty, "Picture");

                //いいね数
                var likes = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Black };
                likes.HeightRequest = 50;
                likes.YAlign = TextAlignment.Center;
                likes.SetBinding(Label.TextProperty, "Likes");

                var pictureLike = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 60 };
                //picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                pictureLike.Source = ImageSource.FromResource("FbSample.image.like.png");


                //いいね行
                var likesSub = new StackLayout {
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { likes, pictureLike }
                };

                View = new StackLayout {
                    Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { picture, likesSub} //アイコンとサブレイアウトを横に並べる
                };
            }
        }

        OneData Search(List<OneData> ar,string id) {
            foreach(var a in ar) {
                if(a.Id == id) {
                    return a;
                }
            }
            return null;
        }
        class OneData {
            public String Id { get; set; }
            public String Picture { get; set; }
            public int Likes { get; set; }
        }
    }
}
