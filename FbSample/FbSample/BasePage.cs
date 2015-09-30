using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class BasePage : ContentPage {
        protected Graph _graph;
        public BasePage(Graph graph, string title) {
            _graph = graph;
            Title = title;
            BackgroundColor = ExColor.FacebookLightBlue;
        }
    }

}
