using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UPMS.Common;
using UPMS.DAL.DB;
using UPMS.Models;

namespace UPMS.DAL.Logic
{
    public class MenuDAL
    {
        public List<MenuInfoAllModel> GetMenuList(int parentId, string mName)
        {
            List<MenuInfoAllModel> list = new List<MenuInfoAllModel>();

            string sql = "select m.MenuId,m.MenuName,m.ParentId,p.MenuName ParentName,m.FrmName,m.MKey from MenuInfos m left join MenuInfos p on m.ParentId=p.MenuId where 1=1";
            if (parentId > 0)
                sql += " and m.ParentId=@parentId";
            if (!string.IsNullOrEmpty(mName))
                sql += " and m.MenuName like @mName";
            SqlParameter[] paras =
            {
                new SqlParameter("@parentId",parentId),
                new SqlParameter("@mName",$"%{mName}%")
            };
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paras);
            while (dr.Read())
            {
                MenuInfoAllModel menu = new MenuInfoAllModel();
                menu.MenuId = dr["MenuId"].ToString().GetInt();
                menu.MenuName = dr["MenuName"].ToString();
                menu.ParentId = dr["ParentId"].ToString().GetInt();
                menu.ParentName = dr["ParentName"].ToString();
                menu.FrmName = dr["FrmName"].ToString();
                menu.MKey = dr["MKey"].ToString();
                list.Add(menu);
            }
            return list;
        }

        //获取父级菜单列表
        public DataTable GetParentList()
        {
            string sql = "select m.ParentId,p.MenuName ,count(1) count from MenuInfos m inner join MenuInfos p on m.ParentId=p.MenuId group by m.ParentId,p.MenuName";
            DataTable dt = DBHelper.GetDataTable(sql, 1);
            return dt;

        }
        /// <summary>
        /// 获取用户角色菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<MenuInfoModel> GetUserMenuList(string roleId,bool isAdmin)
        {
            string sql = "select MenuId,MenuName,ParentId,FrmName,MKey from MenuInfos where 1=1 ";
            if (!isAdmin)
            {
                var menuids = GetRoleMenuInfosListByRoleId(roleId);
                sql += " and MenuId in (" + menuids + ")";

            }

            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1);
            List<MenuInfoModel> list = new List<MenuInfoModel>();
            while (dr.Read())
            {
                MenuInfoModel menuInfo = new MenuInfoModel();
                menuInfo.MenuId = dr["MenuId"].ToString().GetInt();
                menuInfo.MenuName = dr["MenuName"].ToString();
                menuInfo.ParentId = dr["ParentId"].ToString().GetInt();
                menuInfo.FrmName = dr["FrmName"].ToString();
                //menuInfo.MKey = dr["MKey"].ToString();

                list.Add(menuInfo);
            }
            dr.Close();//关闭阅读器
            return list;
        }

        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool DeleteMenu(int menuId)
        {
            //string sqlDelRoleMenu = "delete from RoleMenuInfos where MenuIds=@menuId";
            string sqlDelMenu = "delete from MenuInfos where MenuId=@menuId";
            SqlParameter[] paras = { new SqlParameter("@menuId", menuId) };
            List<CommandInfo> comList = new List<CommandInfo>();
            //comList.Add(new CommandInfo
            //{
            //    CommandText = sqlDelRoleMenu,
            //    IsProc = false,
            //    Paras = paras
            //});
            comList.Add(new CommandInfo
            {
                CommandText = sqlDelMenu,
                IsProc = false,
                Paras = paras
            });
            return DBHelper.ExecuteTrans(comList);

        }

        /// <summary>
        /// 获取所有菜单数据（主要用于绑定下拉框）
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMenu()
        {
            string sql = "select MenuId,MenuName from MenuInfos";
            return DBHelper.GetDataTable(sql, 1);

        }

        public MenuInfoModel GetMenuInfoById(int menuId)
        {
            string sql = "select MenuId,MenuName,ParentId,FrmName,MKey from MenuInfos where MenuId=@menuId ";
            SqlParameter paraId = new SqlParameter("@menuId", menuId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            MenuInfoModel menuInfo = default(MenuInfoModel);
            if (dr.Read())
            {
                menuInfo = new MenuInfoModel();
                menuInfo.MenuId = dr["MenuId"].ToString().GetInt();
                menuInfo.MenuName = dr["MenuName"].ToString();
                menuInfo.ParentId = dr["ParentId"].ToString().GetInt();
                menuInfo.FrmName = dr["FrmName"].ToString();
                menuInfo.MKey = dr["MKey"].ToString();
            }
            dr.Close();
            return menuInfo;
        }

        public bool ExistMenuName(string menuName)
        {
            string sql = "select count(1) from MenuInfos where MenuName=@menuName";
            SqlParameter paraName = new SqlParameter("@menuName", menuName);
            object oCount = DBHelper.ExecuteScalar(sql, 1, paraName);
            if (oCount != null && oCount.ToString() != "")
            {
                return oCount.GetInt() > 0;
            }
            else
            {
                return false;
            }
        }

        public bool AddMenuInfo(MenuInfoModel menuInfo)
        {
            //string sql = "insert into MenuInfos(MenuName,ParentId,FrmName,MKey) values(@menuName,@parentId,@frmName,@mKey)";
            string sql = "insert into MenuInfos(MenuName,ParentId,FrmName) values(@menuName,@parentId,@frmName)";

            SqlParameter[] paras =
           {
                new SqlParameter("@menuName", menuInfo.MenuName),
                new SqlParameter("@parentId", menuInfo.ParentId),
                new SqlParameter("@frmName", menuInfo.FrmName),
                //new SqlParameter("@mKey", menuInfo.MKey)
            };
            return DBHelper.ExecuteNonQuery(sql, 1, paras) > 0;
        }
        public bool UpdateMenuInfo(MenuInfoModel menuInfo)
        {
            string sql = "update MenuInfos set MenuName=@menuName,ParentId=@parentId,FrmName=@frmName where MenuId=@menuId ";

            SqlParameter[] paras =
            {
                new SqlParameter("@menuName", menuInfo.MenuName),
                new SqlParameter("@parentId", menuInfo.ParentId),
                new SqlParameter("@frmName", menuInfo.FrmName),
                //new SqlParameter("@mKey", menuInfo.MKey),
                new SqlParameter("@menuId", menuInfo.MenuId),
            };
            return DBHelper.ExecuteNonQuery(sql, 1, paras) > 0;
        }

        public string GetRoleMenuInfosListByRoleId(string roleId)
        {
            string sql = "select RoleId,MenuIds from RoleMenuInfos where RoleId=@roleId ";
            string menuIds = string.Empty;
            SqlParameter paraId = new SqlParameter("@roleId", roleId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            if (dr.Read())
            {
                menuIds = dr["MenuIds"].ToString();
            }
            dr.Close();

            return menuIds;
        }
    }
}
