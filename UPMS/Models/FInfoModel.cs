using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPMS.Models
{
    public class FInfoModel
    {
        public int FId { get; set; }
        public Action ReloadList { get; set; }
        public string SelectFrmValue { get; set; }
        public string SelectParValue { get; set; }
    }
}
