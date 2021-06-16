using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPMS.DAL.DB
{
    public class CommandInfo
    {
        public string CommandText;//sql或存储过程名
        public DbParameter[] Paras; //参数列表
        public bool IsProc; //是否存储过程
        public CommandInfo()
        {

        }
        public CommandInfo(string comText, bool isProc)
        {
            this.CommandText = comText;
            this.IsProc = isProc;
        }

        public CommandInfo(string comText, bool isProc, DbParameter[] para)
        {
            this.CommandText = comText;
            this.IsProc = isProc;
            this.Paras = para;
        }
    }
}
