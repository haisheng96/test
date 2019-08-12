using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HuaweiSoftware.Folder.DataBase
{
    /// <summary>
    /// SQL数据库操作通用类
    /// </summary>
    class DbHelper
    {
        /// <summary>
        /// 获得一个数据库连接字符串
        /// </summary>
        /// <returns>返回连接字符串</returns>
        public static string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectString"].ConnectionString;
        }

        /// <summary>
        /// 根据传入的数据库连接，执行相应的SQL（查询）语句，获得执行语句后的数据表
        /// </summary>
        /// <param name="conn">传入的数据库连接变量</param>
        /// <param name="sqlString">传入的SQL查询语句</param>
        /// <returns>返回查询后的表</returns>
        public static DataTable ExecuteDataTable(IDbConnection conn, string sqlString)
        {
            DataTable dataTable = new DataTable();
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlString, conn as SqlConnection);
                dataAdapter.Fill(dataTable);
            }
            finally
            {
                conn.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// 根据传入的数据库连接，执行相应的SQL语句,返回执行后影响的条数
        /// </summary>
        /// <param name="conn">传入的数据库连接变量</param>
        /// <param name="sqlString">传入的SQL执行语句</param>
        /// <param name="sqlParams">执行语句中的参数数组</param>
        /// <returns>返回执行语句的影响条数</returns>
        public static int ExecuteNonQuery(IDbConnection conn, string sqlString, params SqlParameter[] sqlParams)
        {
            int result = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn as SqlConnection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(sqlParams);
                result = cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 根据传入的数据库连接，执行相应的SQL语句，获得执行语句后的一个SQLDataReader
        /// </summary>
        /// <param name="conn">传入的数据库连接变量</param>
        /// <param name="sqlString">传入的SQL执行语句</param>
        /// <param name="sqlParams">执行语句中的参数数组</param>
        /// <returns>返回执行语句后的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(IDbConnection conn, string sqlString, params SqlParameter[] sqlParams)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlString, conn as SqlConnection);
            cmd.CommandText = sqlString;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(sqlParams);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// 根据传入的数据库连接，执行相应的SQL语句增改非查询类方法,返回增改的ID
        /// </summary>
        /// <param name="conn">传入的数据库连接变量</param>
        /// <param name="sqlString">传入的SQL执行语句</param>
        /// <param name="sqlParams">执行语句中的参数数组</param>
        /// <returns>返回增改的ID</returns>
        public static int ExecuteScalar(IDbConnection conn, string sqlString, params SqlParameter[] sqlParams)
        {
            int result;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn as SqlConnection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(sqlParams);
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }
}
