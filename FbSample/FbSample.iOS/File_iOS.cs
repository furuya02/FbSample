using FbSample.iOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(File_iOS))]
namespace FbSample.iOS {
    class File_iOS : IFile {
        public string Read(String fileName) {

            try {
                var file = IsolatedStorageFile.GetUserStoreForApplication();

                using (IsolatedStorageFileStream strm = file.OpenFile(fileName, FileMode.Open))
                using (StreamReader reader = new StreamReader(strm)) {
                    return reader.ReadToEnd();
                }
            }catch(Exception ex) {
                return "";
            }
        }

        public bool Save(String fileName, string str) {
            try {
                var file = IsolatedStorageFile.GetUserStoreForApplication();
                using (IsolatedStorageFileStream strm = file.CreateFile(fileName))
                using (StreamWriter writer = new StreamWriter(strm)) {
                    writer.Write(str);
                }
                return true;
            }catch(Exception ex) {
                return false;
            }
        }
    }
}
