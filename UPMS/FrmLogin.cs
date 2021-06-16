using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPMS.Common;
using UPMS.DAL.Logic;
using UPMS.Models;

namespace UPMS
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //1.接收页面输入
            string userName = txtUName.Text.Trim();
            string userPwd = txtUPwd.Text.Trim();

            //2判断账号 密码 是否为空
            if (string.IsNullOrEmpty(userName))
            {
                MsgBoxHelper.MsgErrorShow("账号不能为空!");
                txtUName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(userPwd))
            {
                MsgBoxHelper.MsgErrorShow("密码不能为空!");
                txtUPwd.Focus();
                return;
            }

            UserInfoModel userInfo = new UserInfoModel
            {
                UserName = userName,
                UserPwd = userPwd
            };

            //3 到数据库里检查存在性 --成功 否则 --失败
            UserDAL userDAL = new UserDAL();
            int userId = userDAL.Login(userInfo);
            if (userId > 0)
            {
                //MsgBoxHelper.MsgBoxShow("登录提示", "登录成功");
                //显示到主页面
                FrmMain frmMain = new FrmMain();
                frmMain.Tag = userId;
                frmMain.Show();
                this.Hide();
            }
            else
            {
                MsgBoxHelper.MsgErrorShow("账号或密码输入有误!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
