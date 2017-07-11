using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using function.lib;

namespace 財產管理系統
{
    public partial class init_Asset_ID_Set : Form
    {
        Asset_init_function fun = new Asset_init_function();
        init_PRO iPO = null;
        public init_Asset_ID_Set(init_PRO x)
        {
            InitializeComponent();
            iPO = x;

        }

        private void init_Asset_ID_Set_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;       //最大化
            this.MinimizeBox = false;       //最小化
            this.FormBorderStyle = FormBorderStyle.FixedSingle;     //限制使用者改變form大小
            this.AutoSize = false;          //自動調整大小

            default_start();
        }

        #region 變數
        //=======================================================================================
        public string init_Asset_ID_Set_ServerName      //設定or取得DB連線伺服器名稱
        {
            set;
            get;
        }

        public string AssetDB_SN          //GET_SQL方法中的流水號變數
        {
            set;
            get;
        }

        public string SYS_TXT       //設定錯誤訊息的表頭名稱
        {            
            get
            {
                return iPO.SYS_TXT;
            }
        }


        //=======================================================================================
        #endregion

        #region 方法
        //=======================================================================================

        public void Get_SQL(string xx)          //SQL語法
        {
            if(xx == "公司代碼")
            {
                fun.Query_DB = @"SELECT	[SN_NAME],[SN_DES]  FROM [dbo].[SLS_AssetSN]  where [SN_ID] = 'CT' order by [SN_NAME]";
            }
            else if (xx == "財產大類")
            {
                fun.Query_DB = @"SELECT	[SN_NAME],[SN_DES]  FROM [dbo].[SLS_AssetSN]  where [SN_ID] = 'NO' order by [SN_NAME]";
            }
            else if (xx == "財產中類")
            {
                fun.Query_DB = @"SELECT	[SN_NAME],[SN_DES]  FROM [dbo].[SLS_AssetSN]  where [SN_ID] = 'B' order by [SN_NAME]";
            }
            else if (xx == "財產小類")
            {
                fun.Query_DB = @"SELECT	[SN_NAME],[SN_DES]  FROM [dbo].[SLS_AssetSN]  where [SN_ID] = 'CT' order by [SN_NAME]";
            }
            else if (xx == "G")
            {
                fun.Query_DB = @"SELECT	[SN_NAME],[SN_DES]  FROM [dbo].[SLS_AssetSN]  where [SN_ID] = 'IT' order by [SN_NAME]";
            }
            else if(xx == "流水號")
            {
                AssetDB_SN = init_Asset_ID_CB1.SelectedValue.ToString() + init_Asset_ID_CB2.SelectedValue.ToString() + init_Asset_ID_CB3.SelectedValue.ToString();

                fun.Query_DB = @"exec [dbo].[SLS_Asset_ID_GET_SN] '"+AssetDB_SN+"'";
            }
            else if (xx == "G_流水號")
            {
                AssetDB_SN = init_Asset_ID_CB1.SelectedValue.ToString() + init_Asset_ID_CB2.SelectedValue.ToString() + init_Asset_ID_CB3.SelectedValue.ToString()
                            + init_Asset_ID_CB4.SelectedValue.ToString();

                fun.Query_DB = @"exec [dbo].[SLS_Asset_ID_GET_SN] '" + AssetDB_SN + "','G'";
            }
            

        }

        public void default_start()
        {
            this.Text = "建立財產編號";
            fun.ServiceName = init_Asset_ID_Set_ServerName;     //設定DB連線伺服器名稱
            fun.CB_DMember = "SN_DES";
            fun.CB_VMember = "SN_NAME";
            Get_SQL("公司代碼");
            fun.ComboboxDB_Set(fun.Query_DB, init_Asset_ID_CB1);
            Get_SQL("財產大類");
            fun.ComboboxDB_Set(fun.Query_DB, init_Asset_ID_CB2);
            Get_SQL("財產中類");
            fun.ComboboxDB_Set(fun.Query_DB, init_Asset_ID_CB3);
            Get_SQL("G");
            fun.ComboboxDB_Set(fun.Query_DB, init_Asset_ID_CB4);
            init_Asset_ID_CB1.SelectedValue = -1;       //預設選項->空白
            init_Asset_ID_CB2.SelectedValue = -1;       //預設選項->空白
            init_Asset_ID_CB3.SelectedValue = -1;       //預設選項->空白
            init_Asset_ID_CB4.SelectedValue = 0;       //預設選項->空白
            
            init_Asset_ID_CB4.Visible = false;
            init_Asset_ID_SN.ReadOnly = true;
            init_Asset_ID_Value.ReadOnly = true;
            init_Asset_ID_GCK1.Checked = false;
            init_Asset_ID_GroupValue.ReadOnly = false;
            init_Asset_ID_GroupValue.Visible = false;
            init_Asset_ID_Refresh.Visible = false;
            

        }

        public void start_status(string x)
        {
            if(x == "群組A")
            {
                init_Asset_ID_CB1.Enabled = false;
                init_Asset_ID_CB2.Enabled = false;
                init_Asset_ID_CB3.Enabled = false;
                init_Asset_ID_CB4.Enabled = false;
                init_Asset_ID_SN.ReadOnly = true;
            }
            else if (x == "群組B")
            {
                init_Asset_ID_CB1.Enabled = true;
                init_Asset_ID_CB2.Enabled = true;
                init_Asset_ID_CB3.Enabled = true;
                init_Asset_ID_CB4.Enabled = true;
                init_Asset_ID_SN.ReadOnly = false;

            }
        }

        public void Combobox_Cot(ComboBox x)
        {
                     
        }

        public void Combobox_inTextBox()        //CB值寫入到TextBox的方法
        {
            #region 內容
            init_Asset_ID_Value.Text = "";
            init_Asset_ID_SN.Text = "";
            if (init_Asset_ID_CB1.SelectedIndex != -1)
            {
                init_Asset_ID_Value.Text += init_Asset_ID_CB1.SelectedValue.ToString();
                if (init_Asset_ID_CB2.SelectedIndex != -1)
                {
                    #region 內容
                    if (init_Asset_ID_CB3.SelectedIndex != -1)
                    {
                        if (init_Asset_ID_CB3.SelectedValue.ToString() == "G")      //選項是否為<G><電腦及周邊設備>
                        {
                            //選項為<G>
                            #region 內容
                            if (init_Asset_ID_CB4.SelectedIndex != -1)      //判斷是否有選擇項目
                            {
                                Get_SQL("G_流水號");
                                fun.ProductDB_ds(fun.Query_DB);
                                init_Asset_ID_SN.Text = fun.ds_index.Tables[0].Rows[0]["SN"].ToString();
                            }
                            #endregion
                        }
                        else
                        {
                            //選項不為<G>
                            #region 內容
                            Get_SQL("流水號");
                            fun.ProductDB_ds(fun.Query_DB);
                            init_Asset_ID_SN.Text = fun.ds_index.Tables[0].Rows[0]["SN"].ToString();
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            if (init_Asset_ID_CB2.SelectedIndex != -1)
            {
                init_Asset_ID_Value.Text += init_Asset_ID_CB2.SelectedValue.ToString();
            }
            if (init_Asset_ID_CB3.SelectedIndex != -1)
            {
                init_Asset_ID_Value.Text += init_Asset_ID_CB3.SelectedValue.ToString();
            }
            if (init_Asset_ID_CB4.SelectedIndex != -1)
            {
                init_Asset_ID_Value.Text += init_Asset_ID_CB4.SelectedValue.ToString();
            }

            init_Asset_ID_Value.Text += init_Asset_ID_SN.Text;

            #endregion
        }

        //=======================================================================================
        #endregion        
        
        #region button
        //=======================================================================================
        private void init_Asset_ID_button1_Click(object sender, EventArgs e)          //導入ID
        {

            if (MessageBox.Show("確定要導入財產編號嗎？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                #region 內容
                if (init_Asset_ID_GCK1.Checked)
                {
                    iPO.tb_Asset_ID.Text = init_Asset_ID_Value.Text;
                }
                else
                {
                    if (init_Asset_ID_Value.Text.Length >= 9)
                    {
                        iPO.tb_Asset_ID.Text = init_Asset_ID_Value.Text;
                    }
                    else
                    {
                        MessageBox.Show("財產編號格式有問題~~\n", SYS_TXT);
                    }
                }
                #endregion

            }
        }

        private void init_Asset_ID_button2_Click(object sender, EventArgs e)          //重新整理
        {
            //init_Asset_ID_set_panel1
            fun.clearAir(init_Asset_ID_set_panel1);
            default_start();
        }

        private void init_Asset_ID_button3_Click(object sender, EventArgs e)        //取消
        {
            this.Close();
        }

        private void init_Asset_ID_Refresh_Click(object sender, EventArgs e)        //群組更新
        {
            if (init_Asset_ID_GCK1.Checked == true)
            {
                if (init_Asset_ID_GroupValue.Text != "")
                {
                    #region 內容
                    fun.Query_DB = @"SELECT [ID] FROM [dbo].[SLS_Asset] where [ID] = '" + init_Asset_ID_GroupValue.Text + "'";
                    fun.ProductDB_ds(fun.Query_DB);
                    if (fun.ds_index.Tables[0].Rows.Count == 1)
                    {
                        fun.Query_DB = @"exec [dbo].[SLS_Asset_ID_GET_GSN] '" + init_Asset_ID_GroupValue.Text.Trim() + "'";
                        fun.ProductDB_ds(fun.Query_DB);
                        init_Asset_ID_Value.Text = init_Asset_ID_GroupValue.Text.Trim() + "-" + fun.ds_index.Tables[0].Rows[0]["SN_ID"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("無此編號~~\n請確認~~", SYS_TXT);
                    }

                    #endregion

                }
                else
                {
                    MessageBox.Show("請輸入財產編號~~", SYS_TXT);

                }
            }

        }

        //=======================================================================================
        #endregion

        #region 事件

        private void init_Asset_ID_CB1_SelectionChangeCommitted(object sender, EventArgs e)     //公司代碼>>選取項目後的事件
        {
            Combobox_inTextBox();           //CB值寫入到TextBox的方法
        }

        private void init_Asset_ID_CB2_SelectionChangeCommitted(object sender, EventArgs e)     //財產大類>>選取項目後的事件
        {
            Combobox_inTextBox();           //CB值寫入到TextBox的方法
        }

        private void init_Asset_ID_CB3_SelectionChangeCommitted(object sender, EventArgs e)     //財產中類>>選取項目後的事件
        {
            string iAICB1_Text = init_Asset_ID_CB3.SelectedValue.ToString();
            if (iAICB1_Text == "G")
            {
                init_Asset_ID_CB4.SelectedValue = -1;       //預設選項->空白
                init_Asset_ID_CB4.Visible = true;
                Combobox_inTextBox();           //CB值寫入到TextBox的方法
            }
            else
            {
                init_Asset_ID_CB4.SelectedValue = -1;       //預設選項->空白
                init_Asset_ID_CB4.Visible = false;

                Combobox_inTextBox();           //CB值寫入到TextBox的方法
            }
        }

        private void init_Asset_ID_CB4_SelectionChangeCommitted(object sender, EventArgs e)     //G類>>選取項目後的事件
        {
            if (init_Asset_ID_CB4.Visible == true)
            {
                Combobox_inTextBox();           //CB值寫入到TextBox的方法

            }

        }

        private void init_Asset_ID_GCK1_CheckedChanged(object sender, EventArgs e)      //群組Checkbox>>值變動後的事件
        {
            if (init_Asset_ID_GCK1.Checked)
            {                
                start_status("群組A");
                init_Asset_ID_Value.Text = "";
                init_Asset_ID_GroupValue.Visible = true;
                init_Asset_ID_Refresh.Visible = true;
            }
            else 
            {                
                start_status("群組B");
                init_Asset_ID_GroupValue.Visible = false;
                init_Asset_ID_Refresh.Visible = false;

                Combobox_inTextBox();           //CB值寫入到TextBox的方法
            }
        }

        private void init_Asset_ID_GroupValue_KeyDown(object sender, KeyEventArgs e)        //init_Asset_ID_GroupValue >> 按Enter後的事件
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (init_Asset_ID_GCK1.Checked == true)
                {
                    #region 內容
                    if (init_Asset_ID_GroupValue.Text != "")
                    {
                        #region 內容
                        fun.Query_DB = @"SELECT [ID] FROM [dbo].[SLS_Asset] where [ID] = '" + init_Asset_ID_GroupValue.Text + "'";
                        fun.ProductDB_ds(fun.Query_DB);
                        if (fun.ds_index.Tables[0].Rows.Count == 1)
                        {
                            fun.Query_DB = @"exec [dbo].[SLS_Asset_ID_GET_GSN] '" + init_Asset_ID_GroupValue.Text.Trim() + "'";
                            fun.ProductDB_ds(fun.Query_DB);
                            init_Asset_ID_Value.Text = init_Asset_ID_GroupValue.Text.Trim() + "-" + fun.ds_index.Tables[0].Rows[0]["SN_ID"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("無此編號~~\n請確認~~", SYS_TXT);
                        }

                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("請輸入財產編號~~", SYS_TXT);

                    }
                    #endregion
                }
            }

        }

        #endregion

        

        
        


        

        

        
    }
}
