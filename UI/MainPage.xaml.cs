using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UI
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;
            btnLoad.Click+=btnLoad_Click;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = txtPath.Text;
                if (path == null || path == "")
                {
                    MessageBox.Show("路径不能为空");
                    return;
                }
                else
                {
                    FolderServiceClient.FolderWCFClient client = new FolderServiceClient.FolderWCFClient();
                    client.SaveDataAsync(path,0);
                    MessageBox.Show("保存成功!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！错误原因："+ex.Message);
            }
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderServiceClient.FolderWCFClient client = new FolderServiceClient.FolderWCFClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！错误原因："+ex.Message);
            }
        }
    }
}
