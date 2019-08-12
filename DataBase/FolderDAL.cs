
using HuaweiSoftware.Folder.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HuaweiSoftware.Folder.DataBase
{
    public class FolderDAL
    {
        #region 成员变量

        private readonly string m_ConnString = DbHelper.GetConnString();
        private IDbConnection m_Connection;

        #endregion

        #region 初始化SqlConnection实例
        public FolderDAL()
        {
            m_Connection = new SqlConnection(m_ConnString);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存一条目录或者文件数据，返回该目录或文件ID
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns>返回该目录或文件ID</returns>
        public int Insert(FolderInfo fileInfo, bool isFolder)
        {
            int id;
            string sql;
            SqlParameter[] param;

            if (isFolder)
            {
                sql = "INSERT INTO [Folder]([PID],[Name],[CreateDate]) VALUES(@PID,@Name,@CreateDate) SELECT @@identity";
                param = new SqlParameter[] { new SqlParameter("@PID",fileInfo.PID),
                                             new SqlParameter("@Name", fileInfo.Name),
                                             new SqlParameter("@CreateDate", fileInfo.CreateDate)};
            }
            else
            {
                sql = "INSERT INTO [Files]([PID],[Name],[Type],[Size],[CreateDate]) VALUES(@PID,@Name,@Type,@Size,@CreateDate) SELECT @@identity";
                param = new SqlParameter[] { new SqlParameter("@PID",fileInfo.PID),
                                             new SqlParameter("@Name", fileInfo.Name),
                                             new SqlParameter("@Type", fileInfo.Type),
                                             new SqlParameter("@Size", fileInfo.Size),
                                             new SqlParameter("@CreateDate", fileInfo.CreateDate)};
            }
            id = Convert.ToInt32(DbHelper.ExecuteScalar(m_Connection, sql, param).ToString());

            return id;
        }

        /// <summary>
        /// 获取所有目录信息
        /// </summary>
        /// <returns>目录信息集合</returns>
        public List<FolderInfo> GetFolders()
        {
            string sql = "SELECT [ID],[PID],[Name],[CreateDate] FROM [Folder]";
            DataTable dataTable = DbHelper.ExecuteDataTable(m_Connection, sql);
            List<FolderInfo> folderList = ConvertDataTableToList(dataTable);

            return folderList;
        }

        /// <summary>
        /// 根据目录ID，获取目录下所有子文件的信息（不包括子目录）
        /// </summary>
        /// <param name="id">目录ID</param>
        /// <returns>子文件信息的集合</returns>
        public List<FolderInfo> GetFiles(int id)
        {
            string sql = string.Format("SELECT [ID],[PID],[Name],[Size]=[Size]/1024,[CreateDate] FROM [Files] WHERE PID={0}", id);

            DataTable dataTable = DbHelper.ExecuteDataTable(m_Connection, sql);
            List<FolderInfo> folderList = ConvertDataTableToList(dataTable);

            return folderList;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        public void Clear()
        {
            //删除表Files数据
            string sqlFiles = "TRUNCATE TABLE Files";
            DbHelper.ExecuteNonQuery(m_Connection, sqlFiles);

            //删除外键
            string sqlDrop = "ALTER TABLE [Files] DROP CONSTRAINT [FK_Files_Folder]";
            DbHelper.ExecuteNonQuery(m_Connection, sqlDrop);

            //删除表Folder数据
            string sqlFolder = "TRUNCATE TABLE Folder";
            DbHelper.ExecuteNonQuery(m_Connection, sqlFolder);

            //添加外键
            string sqlAdd = "ALTER TABLE [Files] WITH CHECK ADD CONSTRAINT [FK_Files_Folder] FOREIGN KEY([PID]) REFERENCES [Folder] ([ID])";
            DbHelper.ExecuteNonQuery(m_Connection, sqlAdd);
        }

        /// <summary>
        /// 把DataTable转换成List<FolderInfo>
        /// </summary>
        /// <param name="dataTable">要转化的DataTable</param>
        /// <returns>返回FolderList集合</returns>
        private List<FolderInfo> ConvertDataTableToList(DataTable dataTable)
        {
            List<FolderInfo> folderList = new List<FolderInfo>();

            foreach (DataRow row in dataTable.Rows)
            {
                FolderInfo folderInfo = new FolderInfo();
                folderInfo.ID = (int)row["ID"];
                folderInfo.PID = (int)row["PID"];
                folderInfo.Name = row["Name"].ToString();
                folderInfo.CreateDate = (DateTime)row["CreateDate"];
                folderList.Add(folderInfo);
            }

            return folderList;
        }

        #endregion
    }
}
