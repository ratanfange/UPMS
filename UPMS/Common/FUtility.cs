using System.Windows.Forms;

namespace UPMS.Common
{
    public class FUtility
    {
        public static bool CheckForm(string formName)
        {
            bool b1 = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == formName)
                {
                    b1 = true;
                    break;
                }
            }
            return b1;
        }

        /// <summary>
        /// 显示已打开窗体
        /// </summary>
        /// <param name="formName"></param>
        public static void OpenForm(string formName)
        {

            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == formName)
                {
                    if (!f.Visible)
                    {
                        f.Show();
                    }
                    f.Activate();
                    break;
                }
            }
        }

        /// <summary>
        /// 获取已打开窗体对象
        /// </summary>
        /// <param name="formName"></param>
        public static Form GetOpenForm(string formName)
        {
            Form form = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == formName)
                {
                    form = f;
                    break;
                }
            }
            return form;
        }
    }
}
