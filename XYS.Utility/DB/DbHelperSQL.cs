using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace XYS.Utility.DB
{
    public abstract class DbHelperSQL
    {
        private static string connectionstring = ConfigurationManager.ConnectionStrings["LisMSSQLConnectionString"].ConnectionString.ToString();
        public DbHelperSQL()
        { }
        public static bool ColumnExsits(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName.ToString() + "') and [name]='" + columnName.ToString() + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }
        public static int GetMaxID(string fieldName, string tableName)
        {
            string sql = "select max(" + fieldName.ToString() + ")+1 from " + tableName.ToString();
            object res = GetSingle(sql);
            if (res == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(res.ToString());
            }
            //  return Convert.ToInt32(res);
        }
        public static bool Exists(string SQLString)
        {
            object res = GetSingle(SQLString);
            int cmdResult;
            if (res == null)
            {
                cmdResult = 0;
            }
            else
            {
                cmdResult = int.Parse(res.ToString());
            }
            if (cmdResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public static bool Exists(string SQLString, params SqlParameter[] cmdParms)
        {
            object res = GetSingle(SQLString, cmdParms);
            int cmdResult;
            if (res == null)
            {
                cmdResult = 0;
            }
            else
            {
                cmdResult = int.Parse(res.ToString());
            }
            if (cmdResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool TabExists(string tableName)
        {
            string sqlStr = "select count(*) from sysobjects where id=object_id(N'[" + tableName + "]') and OBJECTPROPERTY(id,N'IsUserTable')=1";
            object res = GetSingle(sqlStr);
            int cmdResult;
            if (res == null)
            {
                cmdResult = 0;
            }
            else
            {
                cmdResult = int.Parse(res.ToString());
            }
            if (cmdResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, con))
                {
                    try
                    {
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        con.Close();
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(SQLString, cmd, null, con, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSqlByTime(string SQLString, int times)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandTimeout = times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        con.Close();
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(SQLString, con);
                System.Data.SqlClient.SqlParameter parameter = new SqlParameter("@content", SqlDbType.NText);
                parameter.Value = content;
                cmd.Parameters.Add(parameter);
                try
                {
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                    cmd.Dispose();
                }
            }
        }
        //插入文本
        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(SQLString, con);
                System.Data.SqlClient.SqlParameter parameter = new SqlParameter("@content", SqlDbType.NText);
                parameter.Value = content;
                cmd.Parameters.Add(parameter);
                try
                {
                    con.Open();
                    object res = cmd.ExecuteScalar();
                    if (Object.Equals(res, null) || Object.Equals(res, DBNull.Value))
                    {
                        return null;
                    }
                    else
                    {
                        return res;
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                    cmd.Dispose();
                }
            }
        }
        public static int ExecuteSqlInsertImg(string SQLString, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        public static SqlDataReader ExecuteReader(string SQLString)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand(SQLString, con);
            try
            {
                con.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(SQLString, cmd, null, con, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
        }
        //返回数据集
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.Fill(ds, "dt");
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }
        public static DataSet Query(string SQLString, int times)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.SelectCommand.CommandTimeout = times;
                    da.Fill(ds, "dt");
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParams)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(SQLString, cmd, null, con, cmdParams);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "dt");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    return ds;
                }
            }
        }
        //返回结果的第一行第一列
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, int times)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        object obj = cmd.ExecuteScalar();
                        if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(SQLString, cmd, null, connection, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        //
        private static void PrepareCommand(string cmdText, SqlCommand cmd, SqlTransaction trans, SqlConnection con, params SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SqlParameter parmeter in cmdParms)
                {
                    if ((parmeter.Direction == ParameterDirection.InputOutput || parmeter.Direction == ParameterDirection.Input) && parmeter.Value == null)
                    {
                        parmeter.Value = System.DBNull.Value;
                    }
                    cmd.Parameters.Add(parmeter);
                }
            }
        }
    }
}
