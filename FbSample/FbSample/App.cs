using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FbSample {
    public class App : Application {
        public App() {
            //メインページをパラメータとしてNavigationPageを生成する
            var navi = new NavigationPage(new MainPage());
            navi.BarBackgroundColor = ExColor.FacebookBlue;
            navi.BarTextColor = Color.White;
            MainPage = navi;

        }
    }

}

