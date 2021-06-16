using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UPMS.Common;
using UPMS.DAL.DB;
using UPMS.Models;

namespace UPMS.DAL.Logic
{
    public class RoleDAL
    {
        public List<RoleInfoModel> GetAllRoles()
        {
             
            string sql = "SELECT RoleId,RoleName,Remark,IsAdmin FROM RoleInfos ";
            DataTable dt = DBHelper.GetDataTable(sql, 1);
            var list = new List<RoleInfoModel>();
            foreach (DataRow item in dt?.Rows)
            {
                list.Add(new RoleInfoModel
                {
                    RoleId = item["RoleId"].GetInt(),
                    RoleName = item["RoleName"].ToString(),
                    IsAdmin = Convert.ToByte(item["IsAdmin"].ToString()),
                    Remark = item["Remark"].ToString()

                });
            }
            return list;
        }

        public bool DeleteRole(int roleId)
        {
            string sqlDelRoleMenu = "delete from RoleMenuInfos where RoleId=@roleId";
            string sqlDelRole = "delete from RoleInfos where RoleId=@roleId";
            SqlParameter[] paras = { new SqlParameter("@roleId", roleId) };
            List<CommandInfo> comList = new List<CommandInfo>();
            comList.Add(new CommandInfo
            {
                CommandText = sqlDelRoleMenu,
                IsProc = false,
                Paras = paras
            });
            comList.Add(new CommandInfo
            {
                CommandText = sqlDelRole,
                IsProc = false,
                Paras = paras
            });
            return DBHelper.ExecuteTrans(comList);
        }

        public bool ExistRoleName(string roleName)
        {
            string sql = "select count(1) from RoleInfos where RoleName=@roleName";
            SqlParameter paraName = new SqlParameter("@roleName", roleName);
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

        public bool AddRoleInfo(RoleInfoModel roleInfo)
        {
            string sql = "insert into RoleInfos(RoleName,Remark) values(@roleName,@remark)";

            SqlParameter[] paras =
            {
                new SqlParameter("@roleName", roleInfo.RoleName),
                new SqlParameter("@remark", roleInfo.Remark),
            };
            return DBHelper.ExecuteNonQuery(sql, 1, paras) > 0;
        }

        public bool UpdateRoleInfo(RoleInfoModel roleInfo)
        {

            string sql = "update RoleInfos set RoleName=@roleName,Remark=@remark where RoleId=@roleId ";

            SqlParameter[] paras =
            {
                new SqlParameter("@roleId", roleInfo.RoleId),
                new SqlParameter("@roleName", roleInfo.RoleName),
                new SqlParameter("@remark", roleInfo.Remark)
            };
            return DBHelper.ExecuteNonQuery(sql, 1, paras) > 0;
        }

        public RoleInfoModel GetRoleById(int fId)
        {
            string sql = "select RoleId,RoleName,Remark,IsAdmin from RoleInfos where RoleId=@roleId ";
            SqlParameter paraId = new SqlParameter("@roleId", fId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            RoleInfoModel roleInfo = default;
            if (dr.Read())
            {
                roleInfo = new RoleInfoModel();
                roleInfo.RoleId = dr["RoleId"].ToString().GetInt();
                roleInfo.RoleName = dr["RoleName"].ToString();
                roleInfo.Remark = dr["Remark"].ToString();
                roleInfo.IsAdmin = Convert.ToByte(dr["IsAdmin"].ToString());
            }
            dr.Close();
            return roleInfo;
        }

        public DataTable GetCboRoles()
        {
            string sql = "SELECT RoleId,RoleName FROM RoleInfos ";
            return DBHelper.GetDataTable(sql, 1);
            
        }

        public bool SaveRights(int roleId, List<string> menuIds)
        {
            string menuid = String.Join(",", menuIds);
            string sql = "update RoleMenuInfos set MenuIds=@menuIds where RoleId=@roleId ";

            var res = GetMenuIdsByRoleId(roleId.ToString());
            if (res == null)
            {
                sql = "insert into RoleMenuInfos(RoleId,MenuIds) values(@roleId,@menuIds) ";
            }

            SqlParameter[] paras =
            {
                new SqlParameter("@roleId", roleId),
                new SqlParameter("@menuIds", menuid)
            };
            return DBHelper.ExecuteNonQuery(sql, 1, paras) > 0;
        }

        public string GetMenuIdsByRoleId(string roleId)
        {
            string menuids = null;

            string sql = "select RoleId,MenuIds from RoleMenuInfos where RoleId=@roleId ";
            SqlParameter paraId = new SqlParameter("@roleId", roleId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            if (dr.Read())
            {
                menuids = dr["MenuIds"].ToString();
            }
            dr.Close();

            return menuids;
        }
    }
}
