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
    public partial class FrmRight : Form
    {
        public FrmRight()
        {
            InitializeComponent();
        }

        FInfoModel fModel = new FInfoModel();
        RoleDAL roleDAL = new RoleDAL();
        MenuDAL menuDAL = new MenuDAL();
        bool blFlag = false;
        int roleId = 0;
        private List<TreeNode> nodeList = new List<TreeNode>();

        private void FrmRight_Load(object sender, EventArgs e)
        {
            if (this.Tag != null)
            {
                fModel = this.Tag as FInfoModel;
                if(fModel != null)
                {
                    roleId = fModel.FId;
                }
            }
            //加载角色下拉框
            LoadCboRoles();
            blFlag = true;

            //加载菜单树
            LoadTvMenus();


            //加载角色已经设置的菜单关系（菜单树中节点勾选）
            LoadCboRolesSet();
            //cbAll.Checked = false;

            LoadRigthSet();


        }

        private void LoadCboRolesSet()
        {
            if (fModel.FId > 0)
            {
                cboRoles.SelectedValue = fModel.FId;
                cboRoles.Enabled = false;
            }
        }

        private void LoadRigthSet()
        {
            for (int i = 0; i < tvMenus.Nodes.Count; i++)
            {
                FetchNode(tvMenus.Nodes[i]);//递归根节点的所有子节点
            }

            var role = roleDAL.GetRoleById(roleId);
            var menuList = new List<string>();
            if (role.IsAdmin != 1)
            {
                var menuids = roleDAL.GetMenuIdsByRoleId(roleId.ToString());
                menuList = menuids?.Split(',').ToList();
            }
            else
            {
                menuList = menuDAL.GetUserMenuList("", true).Select(x=>x.MenuId.ToString()).ToList();
            }

            if(menuList != null)
            {
                foreach (TreeNode item in nodeList)
                {
                    if (menuList.IndexOf(item.Name.ToString()) != -1)
                    {
                        item.Checked = true;
                    }
                }
            }

        }

        private void RigthSet()
        {

        }

        private void LoadTvMenus()
        {
            //获取菜单数据:编号 名称 父级编号
            //DataTable dt=  menuDAL.GetAllTvMenus();
            //DataRow dr = dt.NewRow();
            //dr["RoleId"]
            List<MenuInfoModel> menus = menuDAL.GetUserMenuList("",true);
            tvMenus.Nodes.Clear();
            tvMenus.CheckBoxes = true;
            TreeNode rootNode = new TreeNode("系统菜单");
            rootNode.Name = "0";
            tvMenus.Nodes.Add(rootNode);
            CreateTreeNode(menus, rootNode, 0);
            tvMenus.ExpandAll();
        }

        private void CreateTreeNode(List<MenuInfoModel> list, TreeNode pNode, int parentId)
        {
            var childList = list.Where(m => m.ParentId == parentId);
            foreach (MenuInfoModel menu in childList)
            {
                TreeNode tn = new TreeNode(menu.MenuName.ToString());
                tn.Name = menu.MenuId.ToString();
                if (pNode != null)
                    pNode.Nodes.Add(tn);
                CreateTreeNode(list, tn, menu.MenuId);
            }
        }

        private void LoadCboRoles()
        {
            DataTable dt = roleDAL.GetCboRoles();
            DataRow dr = dt.NewRow();
            dr["RoleId"] = "0";
            dr["RoleName"] = "请选择";
            dt.Rows.InsertAt(dr, 0);
            cboRoles.DataSource = dt;
            cboRoles.DisplayMember = "RoleName";
            cboRoles.ValueMember = "RoleId";
            cboRoles.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (blFlag)
            //{
            //    CheckChildNodes(tvMenus.Nodes[0], false);
            //    List<MenuInfoModel> list = menuDAL.GetUserMenuList(cboRoles.SelectedValue.ToString());
            //    if (list.Count > 0)
            //    {
            //        List<int> menuIds = list.Select(m => m.MenuId).ToList();
            //        CheckTvNodes(tvMenus.Nodes[0].Nodes, menuIds);
            //    }
            //}
        }

        private void CheckTvNodes(TreeNodeCollection tnc, List<int> menuIds)
        {
            foreach (TreeNode tn in tnc)
            {
                if (menuIds.Contains(tn.Name.GetInt()))
                {
                    tn.Checked = true;
                }
                CheckTvNodes(tn.Nodes, menuIds);
            }
        }

        private void CheckChildNodes(TreeNode curTn, bool check)
        {
            foreach (TreeNode tn in curTn.Nodes)
            {
                tn.Checked = check;
                CheckChildNodes(tn, check);
            }
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            //CheckChildNodes(tvMenus.Nodes[0], cbAll.Checked);
        }

        private void tvMenus_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {

                SetChildNodesCheckState(e.Node);
                SetParentNodesCheckState(e.Node);

            }
        }

        private void SetChildNodesCheckState(TreeNode tn)
        {
            foreach (TreeNode child in tn.Nodes)
            {
                child.Checked = tn.Checked;
                SetChildNodesCheckState(child);
            }
        }

        private void SetParentNodesCheckState(TreeNode tn)
        {
            TreeNode pNode = tn.Parent;
            if (pNode != null)
            {
                bool bl = false;
                foreach (TreeNode item in pNode.Nodes)
                {
                    if (item.Checked)
                    {
                        bl = true;
                        break;
                    }

                }
                pNode.Checked = bl;
                SetParentNodesCheckState(pNode);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int roleId = cboRoles.SelectedValue.ToString().GetInt();
            if (roleId > 0)
            {

                List<string> menuIds = new List<string>();
                GetCheckedMenuIds(nodeList, menuIds);
                //保存权限设置
                bool bl = roleDAL.SaveRights(roleId, menuIds.Distinct().ToList());
                if (bl)
                {
                    MsgBoxHelper.MsgBoxShow("权限设置", "权限菜单设置成功！");
                }
                else
                {
                    MsgBoxHelper.MsgErrorShow("权限设置失败！");
                }
            }
        }

        private List<string> GetCheckedMenuIds(List<TreeNode> nodes, List<string> menuIds)
        {
            foreach (TreeNode tn in nodes)
            {
                if (tn.Checked)
                {
                    menuIds.Add(tn.Name.ToString());
                }
            }
            return menuIds;
        }

        private void FetchNode(TreeNode node)
        {
            nodeList.Add(node);
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                FetchNode(node.Nodes[i]);
            }
        }

    }
}
