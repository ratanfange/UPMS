using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPMS.DAL.DB
{
    /// <summary>
    /// 数据库帮组类(工具类) 静态类操作起来更快
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        /// <summary>
        /// 增、删、改的通用方法
        /// 执行sql语句或者存储过程，返回受影响的行数
        /// sql注入
        /// </summary>
        /// <param name="sql">sql语句或者存储过程名</param>
        /// <param name="cmdType">执行的脚步类型 1:sql语句 2:存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, int cmdType, params SqlParameter[] parameters)
        {
            int result = 0;//返回结果
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //执行脚本的对象cmd
                SqlCommand cmd = BuilderCommand(conn, sql, cmdType, null, parameters);
                result = cmd.ExecuteNonQuery();//执行T-SQL并返回受影响的行数
                cmd.Parameters.Clear();
            }
            //using原理：类似于try finally
            return result;
        }

        /// <summary>       
        /// 执行sql返回第一行第一列的值
        /// sql注入
        /// </summary>
        /// <param name="sql">sql语句或者存储过程名</param>
        /// <param name="cmdType">执行的脚步类型 1:sql语句 2:存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, int cmdType, params SqlParameter[] parameters)
        {
            //params 只能写在一维数组前，且只能标识参数列表中的最后一个参数
            //select @@Indentity 返回上一次插入记录自动产生的ID
            object result = null;//返回结果
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //执行脚本的对象cmd
                SqlCommand cmd = BuilderCommand(conn, sql, cmdType, null, parameters);
                result = cmd.ExecuteScalar();//执行T-SQL并返回第一行第一列的值
                cmd.Parameters.Clear();
                if (result == null || result == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return result;
                }
            }
            //using原理：类似于try finally
            //return result;
        }


        /// <summary>
        /// 执行sql查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, int cmdType, params SqlParameter[] parameters)
        {
            //params 只能写在一维数组前，且只能标识参数列表中的最后一个参数
            SqlConnection conn = new SqlConnection(connStr);
            //执行脚本的对象cmd
            SqlCommand cmd = BuilderCommand(conn, sql, cmdType, null, parameters);
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;

            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("创建reader对象发生异常", ex);

            }

        }

        /// <summary>
        /// 执行查询，查询结果填充到DataTable 只针对查询一个表的情况
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, int cmdType, params SqlParameter[] parameters)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = BuilderCommand(conn, sql, cmdType, null, parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 执行查询，数据填充到DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, int cmdType, params SqlParameter[] parameters)
        {
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = BuilderCommand(conn, sql, cmdType, null, parameters);
                //数据适配器
                //conn 自动打开 断开式连接
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                //自动关闭conn
            }
            return ds;
        }

        /// <summary>
        /// 事务 执行批量sql
        /// </summary>
        /// <param name="listSql"></param>
        /// <returns></returns>
        public static bool ExecuteTrans(List<string> listSql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = BuilderCommand(conn, "", 1, trans);
                try
                {
                    int count = 0;
                    for (int i = 0; i < listSql.Count; i++)
                    {
                        cmd.CommandText = listSql[i];
                        cmd.CommandType = CommandType.Text;
                        count += cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("执行事务出现异常", ex);
                }
            }
        }

        /// <summary>
        /// 事务 批量执行 CommandInfo 包括sql,脚本类型，参数列表
        /// </summary>
        /// <param name="comList"></param>
        /// <returns></returns>
        public static bool ExecuteTrans(List<CommandInfo> comList)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = BuilderCommand(conn, "", 1, trans);
                try
                {
                    int count = 0;
                    for (int i = 0; i < comList.Count; i++)
                    {
                        cmd.CommandText = comList[i].CommandText;
                        if (comList[i].IsProc)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                        }
                        if (comList[i].Paras.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            foreach (var p in comList[i].Paras)
                            {
                                cmd.Parameters.Add(p);
                            }

                        }
                        count += cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("执行事务出现异常", ex);
                }
            }
        }

        public static T ExecuteTrans<T>(Func<IDbCommand, T> action)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                IDbCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                return action(cmd);
            }
        }


        private static SqlCommand BuilderCommand(SqlConnection conn, string sql, int cmdType, SqlTransaction trans, SqlParameter[] paras)
        {
            if (conn == null) throw new ArgumentNullException("连接对象不能为空!");
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (cmdType == 2)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            if (trans != null)
                cmd.Transaction = trans;
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);
            }
            return cmd;
        }
        private static SqlCommand BuilderCommand(SqlConnection conn, string sql, int cmdType, SqlTransaction trans)
        {
            return BuilderCommand(conn, sql, cmdType, trans, null);
        }
    }
}
