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
    public partial class FrmRoleList : Form
    {
        private RoleDAL roleDAL = new RoleDAL();

        public FrmRoleList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ShowRoleInfoPage(0);
        }

        private void ShowRoleInfoPage(int roleId)
        {
            FrmRoleInfo fRole = new FrmRoleInfo();
            fRole.Tag = new FInfoModel
            {
                FId = roleId,
                ReloadList = LoadAllRoles
            };
            fRole.MdiParent = this.MdiParent;
            fRole.ShowDialog();

        }

        private void ShowRightPage(int roleId)
        {
            FrmRight fRight;
            if (!FUtility.CheckForm("FrmRight"))
            {
                fRight = new FrmRight();

            }
            else
            {
                fRight = (FrmRight)FUtility.GetOpenForm("FrmRight");
            }
            fRight.Tag = new FInfoModel
            {
                FId = roleId,
                ReloadList = LoadAllRoles
            };
            fRight.MdiParent = this.MdiParent;
            if (!fRight.Visible)
                fRight.Show();
            else
                fRight.Activate();



        }

        private void LoadAllRoles()
        {
            List<RoleInfoModel> list = roleDAL.GetAllRoles();
            dgvRoles.AutoGenerateColumns = false;
            dgvRoles.DataSource = list;
        }

        private void dgvRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var curCell = dgvRoles.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string val = curCell.FormattedValue.ToString();
                RoleInfoModel roleInfo = dgvRoles.Rows[e.RowIndex].DataBoundItem as RoleInfoModel;
                switch (val)
                {
                    case "修改":
                        ShowRoleInfoPage(roleInfo.RoleId);
                        break;
                    case "分配":
                        ShowRightPage(roleInfo.RoleId);
                        break;
                    case "删除":
                        //提示
                        if (MsgBoxHelper.MsgBoxConfirm("删除菜单", "您确定要删除该条角色数据吗?删除角色数据会同角色菜单关系数据一并删除?") == DialogResult.Yes)
                        {
                            //调用删除方法
                            bool blDel = roleDAL.DeleteRole(roleInfo.RoleId);
                            if (blDel)
                            {
                                MsgBoxHelper.MsgBoxShow("成功提示", $"角色:{roleInfo.RoleName} 信息删除成功！");
                                LoadAllRoles();
                            }
                            else
                            {
                                MsgBoxHelper.MsgErrorShow($"角色:{roleInfo.RoleName} 信息删除失败！");
                            }
                        }
                        break;
                }
            }
        }

        private void FrmRoleList_Load(object sender, EventArgs e)
        {
            LoadAllRoles();
        }
    }
}
