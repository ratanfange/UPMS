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
    public partial class FrmMenuInfo : Form
    {
        public FrmMenuInfo()
        {
            InitializeComponent();
        }

        MenuDAL menuDAL = new MenuDAL();
        FInfoModel fModel = null;
        int menuId = 0;
        string oldName = "";//修改前的名称
        private void FrmMenuInfo_Load(object sender, EventArgs e)
        {
            LoadCboParents();//加载父级下拉框
            LoadCboFrmNames();//加载关联页面下拉框
            if (this.Tag != null)
            {
                fModel = this.Tag as FInfoModel;
                if (fModel != null)
                {
                    menuId = fModel.FId;
                }
            }
            if (menuId > 0)//修改页面加载
            {
                //加载菜单信息
                MenuInfoModel menuInfo = menuDAL.GetMenuInfoById(menuId);
                if (menuInfo != null)
                {
                    txtMenuName.Text = menuInfo.MenuName;
                    oldName = menuInfo.MenuName;
                    //txtMKey.Text = menuInfo.MKey;
                    cboParent.SelectedValue = menuInfo.ParentId;
                    if (!string.IsNullOrEmpty(menuInfo.FrmName))
                    {
                        cboFrmName.SelectedValue = menuInfo.FrmName;
                    }

                }
            }
            else
            {
                txtMenuName.Clear();
                //txtMKey.Clear();
            }
        }

        private void LoadCboParents()
        {
            DataTable dtParent = menuDAL.GetAllMenu();
            DataRow dr = dtParent.NewRow();
            dr["MenuId"] = "0";
            dr["MenuName"] = "请选择";
            dtParent.Rows.InsertAt(dr, 0);
            cboParent.DataSource = dtParent;

            if (fModel != null)
            {
                cboParent.SelectedItem = fModel.SelectParValue;
            }
            else
            {
                cboParent.DisplayMember = "MenuName";
                cboParent.ValueMember = "MenuId";
            }

            

        }

        private void LoadCboFrmNames()
        {
            //string assName = this.GetType().Assembly.GetName().Name;
            //Type[] types = this.GetType().Assembly.GetTypes();
            //List<FormInfo> fList = new List<FormInfo>();
            //foreach (Type t in types)
            //{
            //    string tName = t.BaseType.Name;
            //    if (tName == "Form")
            //    {
            //        FormInfo fi = new FormInfo();
            //        Form f = (Form)Activator.CreateInstance(t);
            //        fi.FName = t.FullName.Substring(assName.Length + 1);
            //        fi.FText = f.Text;
            //        fList.Add(fi);
            //        f.Dispose();
            //    }
            //}
            //FormInfo fi0 = new FormInfo()
            //{
            //    FName = "",
            //    FText = "请选择"
            //};
            //fList.Insert(0, fi0);

            var fList = GetWindowsList();

            cboFrmName.DataSource = fList;

            if (fModel != null)
            {
                cboFrmName.SelectedItem = fModel.SelectFrmValue;
            }
            else
            {
                cboFrmName.DisplayMember = "FText";
                cboFrmName.ValueMember = "FName";
                cboFrmName.SelectedIndex = 0;
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //1 接收页面信息输入
            string mName = txtMenuName.Text.Trim();
            int parentId = cboParent.SelectedValue.GetInt();
            string frmName = cboFrmName.SelectedValue.ToString();
            //string mKey = txtMKey.Text.Trim();


            //2 菜单名称是否为空
            if (string.IsNullOrEmpty(mName))
            {
                MsgBoxHelper.MsgErrorShow("菜单名称不能为空");
                txtMenuName.Focus();
                return;
            }

            //3 不能重名(判断是否存在)
            if (menuId == 0 || (menuId > 0 && oldName != mName))
            {
                //判断菜单名是否已经存在
                if (menuDAL.ExistMenuName(mName))
                {
                    MsgBoxHelper.MsgErrorShow("菜单名称已存在");
                    txtMenuName.Focus();
                    return;
                }
            }

            //4 封装信息
            MenuInfoModel menuInfo = new MenuInfoModel()
            {
                MenuName = mName,
                ParentId = parentId,
                FrmName = frmName,
                //MKey = mKey
            };

            bool bl = false;
            //5 执行添加、修改
            if (menuId == 0)
            {
                //执行添加操作
                bl = menuDAL.AddMenuInfo(menuInfo);
            }
            else
            {
                //执行修改操作
                menuInfo.MenuId = menuId;
                bl = menuDAL.UpdateMenuInfo(menuInfo);

            }
            string actMsg = menuId > 0 ? "修改" : "添加";
            if (bl)
            {
                MsgBoxHelper.MsgBoxShow($"{actMsg}菜单", $"菜单:{mName}信息{actMsg}成功");
                //刷新列表数据
                fModel?.ReloadList?.Invoke();
            }
            else
            {
                MsgBoxHelper.MsgErrorShow($"菜单:{mName}信息{actMsg}失败");
            }

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 从当前程序中动态读取form窗体
        /// </summary>
        /// <returns></returns>
        private List<FormInfo> GetWindowsList()
        {
            string assName = this.GetType().Assembly.GetName().Name;
            Type[] types = this.GetType().Assembly.GetTypes();
            List<FormInfo> fList = new List<FormInfo>();
            FormInfo fi = null;
            foreach (Type t in types)
            {
                string baseName = t.BaseType.Name;
                if (baseName == "Form")
                {
                    if (t.Name.ToLower() == "frmlogin")
                        continue;
                    fi = new FormInfo();
                    fi.FName = t.FullName.Substring(assName.Length + 1);

                    switch (t.Name.ToLower())
                    {
                        case "frmlogin":
                            fi.FText = "登陆界面";
                            break;

                        case "frmmain":
                            fi.FText = "主界面";
                            break;

                        case "frmmenuinfo":
                            fi.FText = "菜单信息界面";
                            break;

                        case "frmmenulist":
                            fi.FText = "菜单列表界面";
                            break;

                        case "frmright":
                            fi.FText = "权限界面";
                            break;

                        case "frmroleinfo":
                            fi.FText = "角色信息界面";
                            break;

                        case "frmrolelist":
                            fi.FText = "角色列表界面";
                            break;

                        case "frmuserinfo":
                            fi.FText = "用户信息界面";
                            break;

                        case "frmuserlist":
                            fi.FText = "用户列表界面";
                            break;

                        default:
                            fi.FText = "Error";
                            break;
                    }

                    fList.Add(fi);
                }
            }

            fList.Insert(0, new FormInfo 
            {
                FName = "",
                FText = "请选择"
            });


            return fList;
        }


    }
}
