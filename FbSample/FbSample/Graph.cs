using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FbSample {
    class Graph {

        public List<OnePost> post = new List<OnePost>();
        public List<OneStream> stream = new List<OneStream>();
        public List<OneLike> like = new List<OneLike>();  // 最近「いいね」したLINK
        private FacebookClient _fb;

        public Graph(FacebookClient fb) {
            _fb = fb;
            Refresh(false);
        }

        public void Refresh(bool isForce) {
            RefreshPost(isForce);
            RefreshStream(isForce);
            RefreshLink(isForce);
        }


        // isForce=true 強制的にRestAPIをたたく
        async void RefreshPost(bool isForce) {
            try {
                var fileName = "post.json";
                post.Clear();

                var jsonStr = DependencyService.Get<IFile>().Read(fileName);

                if (jsonStr == "" || isForce) {
                    // ※「いいね」のカウントは300まで
                    jsonStr = await _fb.GetTaskAsync("me/posts?fields=type,message,picture,link,created_time,likes.limit(300).fields(id)&limit=300");
                    DependencyService.Get<IFile>().Save(fileName,jsonStr);
                }

                var o = JObject.Parse(jsonStr);
                foreach (var d in o["data"]) {
                    var type = (string)d["type"];
                    var message = (string)d["message"];
                    var likes = new List<string>();
                    if (d["likes"] != null) {
                        foreach (var l in d["likes"]["data"]) {
                            likes.Add((string) l["id"]);
                        }
                    }
                    var createTime = (string)d["created_time"];
                    var picture = (string)d["picture"];
                    var link = (string)d["link"];
                    post.Add(new OnePost() { Type = type, Message = message, Likes = likes, CreateTime = createTime, Picture = picture, Link = link });
                }




            } catch (Exception ex) {
                //await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        // isForce=true 強制的にRestAPIをたたく
        async void RefreshStream(bool isForce) {
            try {
                var fileName = "stream.json";
                stream.Clear();

                var jsonStr = DependencyService.Get<IFile>().Read(fileName);

                if (jsonStr == "" || isForce) {
                    // ※最近５００件の自分が投降したストリーム
                    var query = "SELECT created_time, message, type FROM stream WHERE source_id = me() ORDER BY created_time DESC LIMIT 500";
                    jsonStr = await _fb.GetTaskAsync("fql?q=" + query);
                    DependencyService.Get<IFile>().Save(fileName, jsonStr);
                }

                var o = JObject.Parse(jsonStr);
                foreach (var d in o["data"]) {
                    var type = (int)d["type"];
                    var message = (string)d["message"];
                    var created_time = (int)d["created_time"];
                    stream.Add(new OneStream() { Type = type, Message = message, Created_time = created_time});
                }


            } catch (Exception ex) {
                //await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        // isForce=true 強制的にRestAPIをたたく
        async void RefreshLink(bool isForce) {
            try {
                var fileName = "link.json";
                like.Clear();

                var jsonStr = DependencyService.Get<IFile>().Read(fileName);

                if (jsonStr == "" || isForce) {
                    // ※最近５００件の自分が投降したストリーム
                    var query = "SELECT created_time,title, url , picture FROM link WHERE owner = me() ORDER BY created_time DESC";
                    jsonStr = await _fb.GetTaskAsync("fql?q=" + query);
                    DependencyService.Get<IFile>().Save(fileName, jsonStr);
                }

                var o = JObject.Parse(jsonStr);
                foreach (var d in o["data"]) {
                    var title = (string)d["title"];
                    var created_time = (int)d["created_time"];
                    var url = (string)d["url"];
                    var picture = (string)d["picture"];
                    like.Add(new OneLike() { Title = title, Url = url, Created_time = created_time ,Picture = picture});
                }


            } catch (Exception ex) {
                //await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

    }
    public class OnePost {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
        public string CreateTime { get; set; }
        public List<string> Likes { get; set; }
    }

    public class OneStream {
        public int Created_time { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
    }

    public class OneLike {
        public int Created_time { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
    }


}
