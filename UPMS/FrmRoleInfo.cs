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
    public partial class FrmRoleInfo : Form
    {
        public FrmRoleInfo()
        {
            InitializeComponent();
        }

        FInfoModel fModel;
        RoleDAL roleDAL = new RoleDAL();
        private string oName;

        /// <summary>
        /// 新增或修改提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            //1接收页面输入
            string roleName = txtRoleName.Text.Trim();
            string remark = txtRemark.Text.Trim();

            //判断角色名称是否为空
            if (string.IsNullOrEmpty(roleName))
            {
                MsgBoxHelper.MsgErrorShow("角色名称不能为空！");
                txtRoleName.Focus();
                return;
            }
            //判断存在性
            if (fModel.FId == 0 || oName != "" && oName != roleName)
            {
                if (roleDAL.ExistRoleName(roleName))
                {
                    MsgBoxHelper.MsgErrorShow("角色名称已存在！");
                    txtRoleName.Focus();
                    return;
                }
            }

            //信息封装
            RoleInfoModel roleInfo = new RoleInfoModel
            {
                RoleName = roleName,
                Remark = remark
            };

            //信息提交 到底是新增还是修改
            bool bl = false;
            if (fModel.FId == 0)
            {
                bl = roleDAL.AddRoleInfo(roleInfo);
            }
            else if (fModel.FId > 0)
            {
                roleInfo.RoleId = fModel.FId;
                bl = roleDAL.UpdateRoleInfo(roleInfo);
            }
            string actMsg = fModel.FId == 0 ? "新增" : "修改";
            if (bl)
            {
                MsgBoxHelper.MsgBoxShow($"{actMsg}角色", $"角色:{ roleName} 信息{actMsg}成功！");
                fModel?.ReloadList?.Invoke();
            }
            else
            {
                MsgBoxHelper.MsgErrorShow($"角色:{ roleName} 信息{ actMsg}失败！");
                return;
            }
            this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmRoleInfo_Load(object sender, EventArgs e)
        {
            if (this.Tag != null)
            {
                fModel = this.Tag as FInfoModel;
                if (fModel != null)
                {
                    if (fModel.FId == 0)
                    {
                        txtRoleName.Clear();
                        txtRemark.Clear();
                        this.Text += "--新增";
                    }
                    else if (fModel.FId > 0)
                    {
                        RoleInfoModel roleInfo = roleDAL.GetRoleById(fModel.FId);
                        if (roleInfo != null)
                        {
                            oName = roleInfo.RoleName;
                            txtRoleName.Text = roleInfo.RoleName;
                            txtRemark.Text = roleInfo.Remark;
                            this.Text += "--修改";
                        }
                    }
                }
            }
        }

    }
}
