using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;

using HuaweiSoftware.Folder.DataBase;
using HuaweiSoftware.Folder.Entity;

namespace HuaweiSoftware.Folder.Business
{
    public class FolderBLL
    {
        #region 成员变量

        private FolderDAL m_FolderDal = new FolderDAL();

        #endregion

        #region 方法

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

        /// <summary>
        /// 将目录信息添加到treeView上
        /// </summary>
        /// <param name="treeNodeCollection">树节点集合</param>
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

        /// <summary>
        /// 递归加入TreeView节点的子节点  
        /// </summary>
        /// <param name="treeNodeCollection">此节点的 ChildNodes集合</param>
        /// <param name="id">此节点的父节点的值(id)</param>
        /// <param name="folderList">作为数据源的目录信息集合list</param>
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

        /// <summary>
        /// 删除表数据
        /// </summary>
        public void DeleteTable()
        {
            m_FolderDal.Clear();
        }

        #endregion
    }
}
