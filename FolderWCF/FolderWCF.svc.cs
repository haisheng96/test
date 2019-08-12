using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using HuaweiSoftware.Folder.DataBase;
using HuaweiSoftware.Folder.Entity;
using System.IO;
using System.Web.UI.WebControls;

namespace FolderWCF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class FolderWCF : IFolderWCF
    {
        private FolderDAL m_FolderDal;
        public FolderWCF()
        {
            m_FolderDal = new FolderDAL();
        }
        /// <summary>
        /// 递归获取该路径的目录和目录下的文件，保存到数据库。
        /// </summary>
        /// <param name="path">路径地址</param>
        /// <param name="pID">该目录的父目录ID。当目录为路径初始值，该参数请选填或填入0.</param>
        public void SaveData(string path, int pID = 0)
        {
            //保存文件夹信息
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DirectoryInfo[] dirInfos = directoryInfo.GetDirectories();
            FolderInfo folderInfo = new FolderInfo();
            folderInfo.ID = 0;
            folderInfo.PID = pID;
            folderInfo.Name = directoryInfo.Name;
            folderInfo.Size = 0;
            folderInfo.Type = 0;
            folderInfo.CreateDate = directoryInfo.CreationTime;
            int id = m_FolderDal.Insert(folderInfo,true);

            //保存子文件信息
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                folderInfo.ID = 0;
                folderInfo.PID = id;
                folderInfo.Name = fileInfo.Name;
                folderInfo.Size = fileInfo.Length;
                folderInfo.Type = 1;
                folderInfo.CreateDate = fileInfo.CreationTime;
                m_FolderDal.Insert(folderInfo,false);
            }

            //递归保存文件夹信息
            foreach (DirectoryInfo directory in dirInfos)
            {
                SaveData(directory.FullName, id);
            }
        }

        /// <summary>
        /// 根据目录ID，加载子文件信息
        /// </summary>
        /// <param name="id">加载子文件信息时所需要的父目录ID。</param>
        /// <returns>子文件信息集合</returns>
        public List<FolderInfo> LoadData(int id)
        {
            return m_FolderDal.GetFiles(id);
        }

        ///// <summary>
        ///// 将目录信息添加到treeView上
        ///// </summary>
        ///// <param name="treeNodeCollection">树节点集合</param>
        public void LoadData(TreeNodeCollection treeNodeCollection)
        {
            List<FolderInfo> folderList = m_FolderDal.GetFolders();
            TreeNode treeNode = new TreeNode();
            treeNode.Value = folderList[0].ID.ToString();
            treeNode.Text = folderList[0].Name.ToString();

            //递归加入此根节点的子节点  
            FillTreeData(treeNode.ChildNodes, treeNode.Value, folderList);
            treeNodeCollection.Add(treeNode);
        }
        public List<FolderInfo> LoadData()
        {
            List<FolderInfo> folderInfoList = new List<FolderInfo>();

            return folderInfoList;
        }

        private void FillTreeData(TreeNodeCollection treeNodeCollection, string id, List<FolderInfo> folderList)
        {
            TreeNode treeNode;
            foreach (FolderInfo folderInfo in folderList)
            {
                if (folderInfo.PID.ToString().Equals(id))
                {
                    treeNode = new TreeNode();
                    treeNode.Value = folderInfo.ID.ToString();
                    treeNode.Text = folderInfo.Name.ToString();
                    treeNodeCollection.Add(treeNode);
                    FillTreeData(treeNode.ChildNodes, treeNode.Value, folderList);
                }
            }
        }
    }
}
