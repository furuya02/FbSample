using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using FbSample.Droid;
using System.IO.IsolatedStorage;
using System.IO;

[assembly: Dependency(typeof(File_Droid))]
namespace FbSample.Droid {
    class File_Droid : IFile {
        public string Read(String fileName) {
            try {
                var file = IsolatedStorageFile.GetUserStoreForApplication();

                using (IsolatedStorageFileStream strm = file.OpenFile(fileName, FileMode.Open))
                using (StreamReader reader = new StreamReader(strm)) {
                    return reader.ReadToEnd();
                }
            } catch (Exception ex) {
                return "";
            }
        }

        public bool Save(String fileName,string str) {
            try {
                var file = IsolatedStorageFile.GetUserStoreForApplication();
                using (IsolatedStorageFileStream strm = file.CreateFile(fileName))
                using (StreamWriter writer = new StreamWriter(strm)) {
                    writer.Write(str);
                }
                return true;
            } catch (Exception ex) {
                return false;
            }
        }
    }
}