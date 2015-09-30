using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbSample {
    class Util {
        static public string ToDateStr(int created_time) {
            return ToDateTime(created_time).ToString("g");
        }

        static public DateTime ToDateTime(int created_time) {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(created_time).AddHours(9);
        }
    }
}
