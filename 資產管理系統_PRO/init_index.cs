using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using function.lib;

namespace 財產管理系統
{
    public partial class init_index : Form 
    {
        Asset_init_function fun = new Asset_init_function();
        init_PRO iPO = null;


        public init_index(init_PRO iit)
        {
            InitializeComponent();
            iPO = iit;
            

        }

        #region 方法
        //===========================================================================
        private void MYFBD(TextBox y)
        {
             FolderBrowserDialog FBD = new FolderBrowserDialog();

             if (FBD.ShowDialog() == DialogResult.OK)
             {                
                 try
                 {
                     y.Text = FBD.SelectedPath;
                 }
                 catch(Exception uo)
                 {
                     System.Windows.Forms.MessageBox.Show(uo.Message);
                 }
                 
             }

        }

        private void System_index_Load(object sender, EventArgs e)
        {
            
            System_tb_01.ReadOnly = true;
            System_tb_02.ReadOnly = true;
            System_QAS_tb_01.ReadOnly = true;
            System_QAS_tb_02.ReadOnly = true;
            System_DEV_tb_01.ReadOnly = true;
            System_DEV_tb_02.ReadOnly = true;
            File_SAccress();
            System_SettabControl.SelectedIndex = 0;
            System_filebutton.Focus();            
        }

        public void File_SAccress()            //檔案存放位置取得的方法
        {
            //x 資產主檔存放位置
            //y 資產異動存放位置
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '001'";
            fun.ProductDB_ds(fun.Query_DB);
            System_tb_01.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '002'";
            fun.ProductDB_ds(fun.Query_DB);
            System_tb_02.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS001'";
            fun.ProductDB_ds(fun.Query_DB);
            System_QAS_tb_01.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS002'";
            fun.ProductDB_ds(fun.Query_DB);
            System_QAS_tb_02.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV001'";
            fun.ProductDB_ds(fun.Query_DB);
            System_DEV_tb_01.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();
            fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV002'";
            fun.ProductDB_ds(fun.Query_DB);
            System_DEV_tb_02.Text = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString();

        }

        private void SW()               //System_SettabControl分頁設定
        {
            switch (System_SettabControl.SelectedIndex)
            {
                #region 內容
                case 0:     //PRD
                    {
                        #region PRD更新
                        if (MessageBox.Show("確定要修改？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_tb_01.Text + "' where info_1 = '001'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_tb_02.Text + "' where info_1 = '002'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            break;
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                case 1:     //QAS
                    {
                        #region QAS更新
                        if (MessageBox.Show("確定要修改？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_QAS_tb_01.Text + "' where info_1 = 'QAS001'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_QAS_tb_02.Text + "' where info_1 = 'QAS002'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            break;
                        }
                        else
                        {
                            break;
                        }
                        #endregion

                    }
                case 2:     //DEV
                    {
                        #region DEV更新
                        if (MessageBox.Show("確定要修改？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_DEV_tb_01.Text + "' where info_1 = 'DEV001'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            fun.Query_DB = @"update [dbo].[SLS_AssetInfo] set [info_2] = '" + System_DEV_tb_02.Text + "' where info_1 = 'DEV002'";
                            fun.DB_PJ_insert(fun.Query_DB, null, "更新成功", "系統訊息");
                            break;
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                #endregion
            }

        }

        //===========================================================================
        #endregion

        #region button
        //===================================================================================================================
        private void System_UP_DB_button_Click(object sender, EventArgs e)          //環境_PRD<更新>
        {
            SW();           //System_SettabControl分頁<更新>       
        }

        private void System_QAS_UP_DB_button_1_Click(object sender, EventArgs e)        //環境_QAS<更新>
        {
            SW();           //System_SettabControl分頁<更新>  
        }

        private void System_DEV_UP_DB_button_1_Click(object sender, EventArgs e)        //環境_DEV<更新>
        {
            SW();           //System_SettabControl分頁<更新> 
        }
        private void System_filebutton_Click(object sender, EventArgs e)            //環境_PRD<財產卡><選擇>
        {
            MYFBD(this.System_tb_01);
        }

        private void System_filebutton_2_Click(object sender, EventArgs e)          //環境_PRD<異動記錄><選擇>
        {
            MYFBD(this.System_tb_02);
        }

        private void System_QAS_filebutton_1_Click(object sender, EventArgs e)          //環境_QAS<財產卡><選擇>
        {
            MYFBD(this.System_QAS_tb_01);

        }

        private void System_QAS_filebutton_2_Click(object sender, EventArgs e)          //環境_QAS<異動記錄><選擇>
        {
            MYFBD(this.System_QAS_tb_02);
        }

        private void System_DEV_filebutton_1_Click(object sender, EventArgs e)          //環境_DEV<財產卡><選擇>
        {
            MYFBD(this.System_DEV_tb_01);
        }

        private void System_DEV_filebutton_2_Click(object sender, EventArgs e)          //環境_DEV<異動記錄><選擇>
        {
            MYFBD(this.System_DEV_tb_02);
        }

        private void System_QASStartup_button_Click(object sender, EventArgs e)         //環境_QAS<啟動>
        {            
            iPO.init_sys_status.Text = "QAS";            
        }

        private void System_DEVStartup_button_Click(object sender, EventArgs e)         //環境_DEV<啟動>
        {
            iPO.init_sys_status.Text = "DEV";
        }

        private void System_PRDStartup_button_Click(object sender, EventArgs e)         //環境_PRD<啟動>
        {
            iPO.init_sys_status.Text = "PRD";
        }

        //===================================================================================================================
        #endregion



    }
}
