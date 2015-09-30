using FbSample.WinPhone;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(File_WinPhone))]

namespace FbSample.WinPhone {
    class File_WinPhone : IFile {
        static IsolatedStorageSettings localSettings = IsolatedStorageSettings.ApplicationSettings;

        public string Read(string fileName) {
            string data = null;

            if (!localSettings.TryGetValue(fileName, out data))
                data = string.Empty;

            return data;
        }

        public bool Save(string fileName,string str) {
            if (localSettings.Contains(fileName))//キーが存在する場合
                localSettings[fileName] = str;
            else
                localSettings.Add(fileName, str);
            return true;
        }
    }
}
