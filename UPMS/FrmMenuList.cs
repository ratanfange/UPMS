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
    public partial class FrmMenuList : Form
    {
        public FrmMenuList()
        {
            InitializeComponent();
        }

        MenuDAL menuDAL = new MenuDAL();

        private void FrmMenuList_Load(object sender, EventArgs e)
        {
            //LoadCboParents();
            LoadMenuList();
        }

        /// <summary>
        /// 加载菜单列表
        /// </summary>
        private void LoadMenuList()
        {
            //int selMenuId = cboParents.SelectedValue.ToString().GetInt();
            //string mName = txtMenuName.Text.Trim();
            List<MenuInfoAllModel> menuList = menuDAL.GetMenuList(0, null);
            dgvMenus.AutoGenerateColumns = false;
            dgvMenus.DataSource = menuList;

        }

        ///// <summary>
        ///// 下拉框绑定
        ///// </summary>
        //private void LoadCboParents()
        //{
        //    DataTable dtParent = menuDAL.GetParentList();
        //    DataRow dr = dtParent.NewRow();
        //    dr["ParentId"] = 0;
        //    dr["MenuName"] = "请选择";
        //    //dtParent.Rows.Add(dr);//表的末尾添加
        //    dtParent.Rows.InsertAt(dr, 0);//插入到第一行
        //    cboParents.DataSource = dtParent;
        //    cboParents.DisplayMember = "MenuName";
        //    cboParents.ValueMember = "ParentId";
        //}

        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    LoadMenuList();
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ShowMenuInfoPage(0);
        }

        private void ShowMenuInfoPage(int menuId,string frmName = null,string parName = null)
        {
            FrmMenuInfo frmMenuInfo = new FrmMenuInfo();
            frmMenuInfo.MdiParent = this.MdiParent;
            frmMenuInfo.Tag = new FInfoModel
            {
                FId = menuId,
                SelectFrmValue = frmName,
                SelectParValue = parName,
                ReloadList = LoadMenuList
            };
            frmMenuInfo.ShowDialog();
        }

        private void dgvMenus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var currCell = dgvMenus.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewLinkCell;
            string cellValue = currCell?.FormattedValue?.ToString().Trim();
            MenuInfoAllModel menuInfo = dgvMenus.Rows[e.RowIndex].DataBoundItem as MenuInfoAllModel;
            switch (cellValue)
            {
                case "修改":
                    ShowMenuInfoPage(menuInfo.MenuId, menuInfo.FrmName, menuInfo.ParentName);
                    break;
                case "删除":
                    //提示
                    if (MsgBoxHelper.MsgBoxConfirm("删除菜单", "您确定要删除该条菜单数据吗?删除菜单数据会同角色菜单关系数据一并删除?") == DialogResult.Yes)
                    {
                        //调用删除方法
                        bool blDel = menuDAL.DeleteMenu(menuInfo.MenuId);
                        if (blDel)
                        {
                            MsgBoxHelper.MsgBoxShow("成功提示", $"菜单:{menuInfo.MenuName} 信息删除成功！");
                            LoadMenuList();
                        }
                        else
                        {
                            MsgBoxHelper.MsgErrorShow($"菜单:{menuInfo.MenuName} 信息删除失败！");
                        }
                    }
                    break;
            }
        }
    }
}
