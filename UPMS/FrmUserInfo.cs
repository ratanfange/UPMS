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
    public partial class FrmUserInfo : Form
    {
        FInfoModel fModel;
        RoleDAL roleDAL = new RoleDAL();
        UserDAL userDAL = new UserDAL();
        public FrmUserInfo()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                MsgBoxHelper.MsgErrorShow("角色名不能为空！");
                username.Focus();
                return;
            }

            if (string.IsNullOrEmpty(userpwd.Text))
            {
                MsgBoxHelper.MsgErrorShow("密码不能为空！");
                userpwd.Focus();
                return;
            }

            if (cbRole.SelectedIndex == 0)
            {
                MsgBoxHelper.MsgErrorShow("角色不能为空！");
                return;
            }

            bool bl = false;

            if (fModel.FId == 0)
            {
                bl = userDAL.AddUserInfo(username.Text, userpwd.Text, Convert.ToInt32(cbRole.SelectedValue));
            }
            else if (fModel.FId > 0)
            {
                bl = userDAL.UpdateRoleInfo(fModel.FId,username.Text, userpwd.Text, Convert.ToInt32(cbRole.SelectedValue));

            }

            if (bl)
            {
                MessageBox.Show("修改成功！");
                fModel?.ReloadList?.Invoke();
            }
            else
            {
                MsgBoxHelper.MsgErrorShow($"修改失败！");
                return;
            }

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            LoadCboRoles();

            if (this.Tag != null)
            {
                fModel = this.Tag as FInfoModel;
                if (fModel != null)
                {
                    if (fModel.FId == 0)
                    {
                        username.Clear();
                        userpwd.Clear();
                        cbRole.SelectedIndex = 0;
                        //this.Text += "--新增";
                    }
                    else if (fModel.FId > 0)
                    {
                        var user = userDAL.GetUserById(fModel.FId);
                        var role = userDAL.GetUserRoles(fModel.FId);
                        if (user != null)
                        {
                            username.Text = user.UserName;
                            userpwd.Text = user.UserPwd;
                            cbRole.SelectedIndex = role.RoleId;
                            //this.Text += "--修改";
                        }
                    }
                }
            }
        }

        private void LoadCboRoles()
        {
            DataTable dt = roleDAL.GetCboRoles();
            DataRow dr = dt.NewRow();
            dr["RoleId"] = "0";
            dr["RoleName"] = "请选择";
            dt.Rows.InsertAt(dr, 0);
            cbRole.DataSource = dt;
            cbRole.DisplayMember = "RoleName";
            cbRole.ValueMember = "RoleId";
            cbRole.SelectedIndex = 0;
        }
    }
}
