using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMS.Common;
using UPMS.DAL.DB;
using UPMS.Models;

namespace UPMS.DAL.Logic
{
    public class UserDAL
    {
        public int Login(UserInfoModel user)
        {
            string sql = "select UserId from UserInfos where UserName=@UserName and UserPwd=@UserPwd";
            SqlParameter[] paras =
            {
                new SqlParameter("@UserName",user.UserName),
                new SqlParameter("@UserPwd",user.UserPwd)
            };

            var tb = DBHelper.GetDataTable(sql, 1, paras);
            if(tb?.Rows != null && tb.Rows.Count > 0)
            {
                return 1;
            }

            return 0;

            //object oId = DBHelper.ExecuteScalar(sql, 1, paras);//卡
            //if (oId != null && oId.ToString() != "")
            //{
            //    return oId.GetInt();
            //}
            //else
            //{
            //    return 0;
            //}
        }

        public RoleInfoModel GetUserRoles(int userId)
        {
            string sql = "select r.RoleId,RoleName,IsAdmin from UserRoleInfos ur right join RoleInfos r on r.RoleId=ur.RoleId where UserId=@UserId";
            SqlParameter paraId = new SqlParameter("@UserId", userId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            RoleInfoModel roleInfo = default;
            while (dr.Read())
            {
                roleInfo = new RoleInfoModel()
                {
                    RoleId = dr["RoleId"].ToString().GetInt(),
                    RoleName = dr["RoleName"].ToString(),
                    IsAdmin = Convert.ToInt32(dr["IsAdmin"].ToString()),
                };
            }
            dr.Close();//关闭阅读器
            return roleInfo;
        }

        public List<UserInfoModel> GetAllUser()
        {
            string sql = "SELECT u.UserId,u.UserName,u.UserPwd,ur.RoleId,r.RoleName FROM UserInfos u left join UserRoleInfos ur on u.UserId = ur.UserId inner join RoleInfos r on ur.RoleId = r.RoleId";
            DataTable dt = DBHelper.GetDataTable(sql, 1);
            var list = new List<UserInfoModel>();
            foreach (DataRow item in dt?.Rows)
            {
                list.Add(new UserInfoModel
                {
                    UserId = item["UserId"].GetInt(),
                    UserName = item["UserName"].ToString(),
                    UserPwd = item["UserPwd"].ToString(),
                    RoleId = item["RoleId"].ToString(),
                    RoleName = item["RoleName"].ToString(),

                });
            }
            return list;
        }

        public bool DeleteRole(int userId)
        {
            string sqlDelRoleMenu = "delete from UserInfos where UserId=@userId";
            string sqlDelRole = "delete from UserRoleInfos where UserId=@userId";
            SqlParameter[] paras = { new SqlParameter("@userId", userId) };
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

        public UserInfoModel GetUserById(int fId)
        {
            string sql = "select UserId,UserName,UserPwd from UserInfos where UserId=@userId ";
            SqlParameter paraId = new SqlParameter("@userId", fId);
            SqlDataReader dr = DBHelper.ExecuteReader(sql, 1, paraId);
            UserInfoModel userInfo = default;
            if (dr.Read())
            {
                userInfo = new UserInfoModel();
                userInfo.UserName = dr["UserName"].ToString();
                userInfo.UserPwd = dr["UserPwd"].ToString();
            }
            dr.Close();
            return userInfo;
        }

        public bool AddUserInfo(string username,string userpwd,int roleId = 0)
        {
            string sqlupdateUser = $"insert into UserInfos(UserName,UserPwd) values(@username,@userpwd)";

            SqlParameter[] paras =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@userpwd", userpwd),
            };

            var res = DBHelper.ExecuteNonQuery(sqlupdateUser, 1, paras);

            var res2 = AddUserRole(username, roleId);

            return res>0 && res2;
        }

        public bool UpdateRoleInfo(int userid, string username = null, string userpwd = null, int roleId = 0)
        {
            string sqlupdateUser = $"update UserInfos set UserName=@username,UserPwd=@userpwd where UserId=@userid";
            string sqlUpdateUserRole = $"update UserRoleInfos set RoleId=@roleId where UserId=@userid";
            SqlParameter[] paras = { 
                new SqlParameter("@userid", userid),
                new SqlParameter("@roleId", roleId),
                new SqlParameter("@username", username),
                new SqlParameter("@userpwd", userpwd),
            };
            List<CommandInfo> comList = new List<CommandInfo>();
            comList.Add(new CommandInfo
            {
                CommandText = sqlupdateUser,
                IsProc = false,
                Paras = paras
            });
            comList.Add(new CommandInfo
            {
                CommandText = sqlUpdateUserRole,
                IsProc = false,
                Paras = paras
            });
            return DBHelper.ExecuteTrans(comList);
        }

        private bool AddUserRole(string username,int roleid)
        {
            string sql = "select UserId from UserInfos where UserName=@UserName";
            SqlParameter[] paras =
            {
                new SqlParameter("@UserName",username)
            };
            object oId = DBHelper.ExecuteScalar(sql, 1, paras);
            if (oId != null && oId.ToString() != "")
            {
                var userid = oId.GetInt();

                string sqlUpdateUserRole = $"insert into UserRoleInfos(UserId,RoleId) values(@userid,@roleId)";

                SqlParameter[] paras2 =
                {
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@roleId", roleid),
                };
                return DBHelper.ExecuteNonQuery(sqlUpdateUserRole, 1, paras2) > 0;

            }

            return false;
        }

    }
}
