using System;
using System.Collections.Generic;
using System.Text;

namespace HuaweiSoftware.Folder.Entity
{
    //对应数据库表FolderInfo的实体对象
    public class FolderInfo
    {
        //ID目录/文件ID（自增）
        private int m_ID;

        //PID父目录ID
        private int m_PID;

        //Name目录/文件名称
        private string m_Name;

        //Type类型
        private int m_Type;

        //Size大小
        private long m_Size;

        //CreateDate创建时间
        private DateTime m_CreateDate;

        /// <summary>
        /// 目录/文件ID（自增）
        /// </summary>
        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
            }
        }

        /// <summary>
        /// 目录/文件的父目录ID
        /// </summary>
        public int PID
        {
            get
            {
                return m_PID;
            }
            set
            {
                m_PID = value;
            }
        }

        /// <summary>
        /// 目录/文件的名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 类型：目录：0；文件：1
        /// </summary>
        public int Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        /// <summary>
        /// 文件大小。文件：**KB;目录：0；
        /// </summary>
        public long Size
        {
            get
            {
                return m_Size;
            }
            set
            {
                m_Size = value;
            }
        }

        /// <summary>
        /// 目录/文件创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return m_CreateDate;
            }
            set
            {
                m_CreateDate = value;
            }
        }

    }
}
