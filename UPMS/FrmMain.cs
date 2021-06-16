using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPMS.Common;
using UPMS.DAL.Logic;
using UPMS.Models;

namespace UPMS
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        UserDAL userDAL = new UserDAL();
        MenuDAL menuDAL = new MenuDAL();
        int userId = 0;

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //获取传递过来的UserId
            if (this.Tag != null)
            {
                userId = this.Tag.GetInt();
            }
            if (userId > 0)
            {
                //获取用户的角色信息
                var roles = userDAL.GetUserRoles(userId);
                bool bl = false;
                List<MenuInfoModel> menuList = new List<MenuInfoModel>();
                if (roles.IsAdmin == 1)
                {
                    //加载所有菜单列表
                    bl = true;
                    menuList = menuDAL.GetUserMenuList(roles.RoleId.ToString(),true);
                }
                else
                {
                    menuList = menuDAL.GetUserMenuList(roles.RoleId.ToString(),false);
                }

                tvMenus.Nodes.Clear();
                //加载菜单树
                TreeNode rootNode = new TreeNode();
                rootNode.Name = "0";
                rootNode.Text = "权限管理系统";
                tvMenus.Nodes.Add(rootNode);
                CreateNode(menuList, rootNode, 0);
                if(e != null)
                {
                    tvMenus.ExpandAll();
                }
                
            }
        }

        private void CreateNode(List<MenuInfoModel> menuList, TreeNode pNode, int parentId)
        {
            if (menuList.Count > 0)
            {
                //获取所有的子菜单
                var childList = menuList.Where(m => m.ParentId == parentId).ToList();
                foreach (MenuInfoModel menu in childList)
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = menu.MenuId.ToString();
                    tn.Text = menu.MenuName;
                    if (!string.IsNullOrEmpty(menu.FrmName))
                    {
                        tn.Tag = menu.FrmName;
                    }

                    if (pNode != null)
                    {
                        pNode.Nodes.Add(tn);//节点添加
                    }
                    else
                    {
                        tvMenus.Nodes.Add(tn);
                    }
                    CreateNode(menuList, tn, menu.MenuId);
                }
            }

        }

        private void tvMenus_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selNode = tvMenus.SelectedNode;
            if (selNode.Tag != null)
            {
                string url = selNode.Tag.ToString();

                this.splitContainer1.Panel2.Controls.Clear();
                Assembly assembly = Assembly.GetExecutingAssembly();
                Form o = assembly.CreateInstance($"UPMS.{url}") as Form;
                o.TopLevel = false;
                o.FormBorderStyle = FormBorderStyle.None;
                this.splitContainer1.Panel2.Controls.Add(o);
                o.Show();

            }

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //让用户确认到底要不要退出，是--退出 否--不退出
            if (MsgBoxHelper.MsgBoxConfirm("退出系统", "您确定要退出系统吗") == DialogResult.Yes)
            {
                //Application.Exit();//有问题，弹出两次
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;//如果没有这一句，主页面仍然关闭，但没有退出应用程序
            }
        }

        private void tvMenus_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name != "FrmMain" && frm.Name != "FrmLogin")
                {
                    if (!frm.IsDisposed)
                    {
                        frm.Dispose();
                        break;
                    }
                    //frm.Close();内存会泄露
                    //break;

                }
            }
        }

        private void tvMenus_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            FrmMain_Load(sender,null);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
