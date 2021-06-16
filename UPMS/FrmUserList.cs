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
    public partial class FrmUserList : Form
    {
        public FrmUserList()
        {
            InitializeComponent();
        }

        RoleDAL roleDAL = new RoleDAL();
        UserDAL userDAL = new UserDAL();

        private void ShowUserInfoPage(int userId)
        {
            FrmUserInfo fRole = new FrmUserInfo();
            fRole.Tag = new FInfoModel
            {
                FId = userId,
                ReloadList = LoadAllUser
            };
            fRole.MdiParent = this.MdiParent;
            fRole.ShowDialog();

        }

        private void LoadAllUser()
        {
            List<UserInfoModel> list = userDAL.GetAllUser();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = list;
        }

        private void FrmUserList_Load(object sender, EventArgs e)
        {
            LoadAllUser();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowUserInfoPage(0);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var curCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string val = curCell.FormattedValue.ToString();
                UserInfoModel userInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as UserInfoModel;
                switch (val)
                {
                    case "修改":
                        ShowUserInfoPage(userInfo.UserId);
                        break;
                    case "删除":
                        //提示
                        if (MsgBoxHelper.MsgBoxConfirm("删除菜单", "您确定要删除该条角色数据吗?删除角色数据会同角色菜单关系数据一并删除?") == DialogResult.Yes)
                        {
                            //调用删除方法
                            bool blDel = userDAL.DeleteRole(userInfo.UserId);
                            if (blDel)
                            {
                                MsgBoxHelper.MsgBoxShow("成功提示", $"用户:{userInfo.UserName} 信息删除成功！");
                                LoadAllUser();
                            }
                            else
                            {
                                MsgBoxHelper.MsgErrorShow($"角色:{userInfo.UserName} 信息删除失败！");
                            }
                        }
                        break;
                }
            }
        }
    }
}
