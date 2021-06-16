using System.Windows.Forms;

namespace UPMS.Common
{
    public class MsgBoxHelper
    {
        public static DialogResult MsgBoxShow(string title, string msg)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult MsgBoxConfirm(string title, string msg)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult MsgErrorShow(string msg)
        {
            return MessageBox.Show(msg, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
