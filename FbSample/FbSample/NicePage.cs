using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class NicePage : BasePage {

        public NicePage(Graph graph, string title) : base(graph, title) {

            var ar = new List<OneData>();

            foreach (var a in _graph.like) {

                var createdTime = Util.ToDateStr(a.Created_time);

                ar.Add(new OneData() { Message = a.Title, CreateTime = createdTime, Url = a.Url, Picture = a.Picture });
            }


            var listView = new ListView() {
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(MyCell)),//セルの指定
                RowHeight = 100, // 行の高さ固定
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
                var picture = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 100, HeightRequest = 100};
                picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                picture.SetBinding(Image.SourceProperty, "Picture");

                var createTime = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Gray };
                createTime.SetBinding(Label.TextProperty, "CreateTime");


                var message = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Black };
                message.SetBinding(Label.TextProperty, "Message");

                var url = new Label { Font = Font.SystemFontOfSize(9), TextColor = Color.Black };
                url.SetBinding(Label.TextProperty, "Url");

                //いいね行
                var likesSub = new StackLayout {
                    //Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { message,createTime,url }
                };

                View = new StackLayout {
                    Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { picture, likesSub }
                };
            }
        }

        class OneData {
            public string CreateTime { get; set; }
            public String Message { get; set; }
            public String Picture { get; set; }
            public String Url { get; set; }
        }
    }

}
