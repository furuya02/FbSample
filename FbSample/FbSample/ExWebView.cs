using Xamarin.Forms;

namespace FbSample {
    public class ExWebView : WebView {

        public ExWebView(bool deleteCookie) {
            DeleteCookie = deleteCookie;
        }

        public bool DeleteCookie { get; private set; }

        //public event NavigateHandler Navigate;
        //public delegate void NavigateHandler(string request);

        //public void OnNavigate(string request) {
        //    if (Navigate != null) {
        //        Navigate(request);
        //    }
        //}
    }
}

