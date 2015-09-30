using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbSample {
    public interface IFile {
        bool Save(String fileName,String str);
        String Read(String fileName);
    }
}
