using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class DatePage : BasePage {
        // isHour = true1:時間別  false;月別
        public DatePage(Graph graph, string title,bool isHour) : base(graph, title) {

            var max = isHour ? 24 : 12;

            // データのカウント
            var counter = Enumerable.Range(0, max).Select(n => 0.0).ToList();
            foreach (var a in _graph.stream) {
                var dt = Util.ToDateTime(a.Created_time);
                if (isHour) {
                    counter[dt.Hour]++;
                } else {
                    counter[dt.Month-1]++;
                }
            }
            // ％に換算する
            var ar = new List<int>();
            foreach(var c in counter) {
                var n = c / (double)counter.Max() * 100;
                ar.Add((int)n);

            }


            var stackLayout = new StackLayout {
                Padding = new Thickness(0, 30, 0, 0),
                Spacing = 5,
            };
            for (var i=0;i<ar.Count;i++) {
                // 時間　棒グラフ ％を横に並べる
                var layout = new StackLayout {
                    Padding = new Thickness(10, 0, 0, 0),
                    Spacing = 0,
                    Orientation = StackOrientation.Horizontal, //横に並べる
                };

                var createTime = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Gray };

                var labelHour = new Label {
                    Font = Font.SystemFontOfSize(10),
                    TextColor = Color.Black ,
                    Text = string.Format("{0}{1}", isHour?i:i+1, isHour ? "時" : "月"),
                    WidthRequest = 25,
                    XAlign = TextAlignment.Start
                };
                layout.Children.Add(labelHour);

                var boxView = new BoxView {
                    Color = isHour?ExColor.FacebookMediumBlue:Color.Pink,
                    WidthRequest = ar[i]*3,
                    HeightRequest = 12,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                layout.Children.Add(boxView);

                var labelParcent = new Label {
                    Font = Font.SystemFontOfSize(10),
                    TextColor = Color.Gray,
                    Text = string.Format("{0}件", counter[i]),
                    WidthRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                };
                layout.Children.Add(labelParcent);

                //縦に並べる
                stackLayout.Children.Add(layout);
            }
            Content = stackLayout;

        }
        //セル用のテンプレート
        private class MyCell : ViewCell {
            public MyCell() {

                //画像
                var picture = new Image() { Aspect = Aspect.AspectFit, WidthRequest = 100, HeightRequest = 100 };
                picture.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
                picture.SetBinding(Image.SourceProperty, "Picture");

                var createTime = new Label { Font = Font.SystemFontOfSize(10), TextColor = Color.Gray };
                createTime.SetBinding(Label.TextProperty, "CreateTime");


                var message = new Label { Font = Font.SystemFontOfSize(10) };
                message.SetBinding(Label.TextProperty, "Message");

                var url = new Label { Font = Font.SystemFontOfSize(9) };
                url.SetBinding(Label.TextProperty, "Url");

                //いいね行
                var likesSub = new StackLayout {
                    //Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { message, createTime, url }
                };

                View = new StackLayout {
                    Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = { picture, likesSub }
                };
            }
        }

        //class OneData {
        //    public string CreateTime { get; set; }
        //    public String Message { get; set; }
        //    public String Picture { get; set; }
        //    public String Url { get; set; }
        //}
    }
}
