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
    public partial class init_ComboBox_Option : Form 
    {

        public init_ComboBox_Option()
        {
            InitializeComponent();
        }

        Asset_init_function fun = new Asset_init_function();

        #region 變數        
        public string Co_SN_ID      //新增用變數
        {
            set;
            get;
        }

        public string Co_SN_Code        //新增用變數
        {
            set;
            get;
        }

        #endregion

        #region 方法
        //=============================================================================================

        public void Get_SQL(string xQL)
        {
            fun.Query_DB = "";
            if (xQL == "查詢")
            {
                fun.Query_DB = @"SELECT [SN_Code],[SN_NAME],[SN_DES]  FROM [TEST_SLSYHI].[dbo].[SLS_AssetSN]  where [SN_ID] ='" + init_CO_Query_CB.SelectedValue + "' order by 1";
            }

        }

        public void default_status()            //預設狀態
        {
            #region 內容
            fun.Disabled_Panel_btn(init_CO_panel2);
            init_CO_Modify.Visible = false;
            init_CO_Del.Visible = false;
            init_CO_Add.Enabled = true;
            init_CO_Save.Visible = false;
            init_CO_Cancel.Visible = false;
            Combobox_Content();      //Combobox設定名稱和值
            init_CO_Query_CB.SelectedIndex = -1;
            tb_init_CO_NO.ReadOnly = true;
            tb_init_CO_Name.ReadOnly = true;
            tb_init_CO_Other.ReadOnly = true;
            init_CO_Query_CB.Enabled = true;
            init_CO_Query.Enabled = true;

            #endregion
        }

        public void start_status(Button x)      //啟動狀態
        {
            #region 內容
            if (x == init_CO_Add)       //新增
            {
                #region 內容
                tb_init_CO_Name.ReadOnly = false;
                tb_init_CO_Other.ReadOnly = false;
                init_CO_Save.Visible = true;
                init_CO_Cancel.Visible = true;
                init_CO_Save.Enabled = true;
                init_CO_Cancel.Enabled = true;
                init_CO_Query_CB.Enabled = false;
                init_CO_Query.Enabled = false;
                #endregion
            }
            else if (x == init_CO_Modify)       //修改
            {
                #region 內容

                #endregion

            }
            else if (x == init_CO_Del)       //刪除
            {
                #region 內容

                #endregion

            }
            else if (x == init_CO_Save)       //儲存
            {
                #region 內容
                tb_init_CO_Name.ReadOnly = true;
                tb_init_CO_Other.ReadOnly = true;
                init_CO_Save.Visible = false;
                init_CO_Cancel.Visible = false;
                init_CO_Save.Enabled = false;
                init_CO_Cancel.Enabled = false;
                init_CO_Query_CB.Enabled = true;
                init_CO_Query.Enabled = true;
                fun.clearText(init_CO_panel1);

                #endregion
                
            }
            else if (x == init_CO_Cancel)       //取消
            {
                #region 內容
                tb_init_CO_Name.ReadOnly = true;
                tb_init_CO_Other.ReadOnly = true;
                init_CO_Save.Visible = false;
                init_CO_Cancel.Visible = false;
                init_CO_Save.Enabled = false;
                init_CO_Cancel.Enabled = false;
                init_CO_Query_CB.Enabled = true;
                init_CO_Query.Enabled = true;

                #endregion                
            }
            #endregion
        }

        public void Combobox_Content()      //Combobox設定名稱和值
        {
            DataTable dt_Com = new DataTable();
            DataColumn SDC = new DataColumn("ID" , System.Type.GetType("System.String"));
            DataColumn SDC1 = new DataColumn("Value" , System.Type.GetType("System.String"));
            dt_Com.Columns.Add(SDC);
            dt_Com.Columns.Add(SDC1);
            string[] str_ID = {"公司代碼","存放地點","財產屬性","財產類別","使用狀態","單位"};
            string[] str_Value = {"CT","OT","A","B","C","U"};
            for(int i = 0 ; i< str_ID.Length ; i++)
            {
                DataRow dr = dt_Com.NewRow();
                dr["ID"] = str_ID[i];
                dr["Value"] = str_Value[i];
                dt_Com.Rows.Add(dr);
            }
            init_CO_Query_CB.DisplayMember = "ID";
            init_CO_Query_CB.ValueMember = "Value";
            init_CO_Query_CB.DataSource = dt_Com;
            
        }

        public void init_CO_sub_()
        {
            tb_init_CO_NO.Text = fun.ds_index.Tables[0].Rows[0]["SN_Code"].ToString();
            tb_init_CO_Name.Text = fun.ds_index.Tables[0].Rows[0]["SN_NAME"].ToString();
            tb_init_CO_Other.Text = fun.ds_index.Tables[0].Rows[0]["SN_DES"].ToString();
                
        }

        public void DGV1_View()         //DB對應dataGridView1欄位
        {
            dataGridView1.AutoGenerateColumns = false;
            init_CO_DGV1_Column1.DataPropertyName = "SN_Code";
            init_CO_DGV1_Column2.DataPropertyName = "SN_NAME";
            init_CO_DGV1_Column3.DataPropertyName = "SN_DES";

        }

        //=============================================================================================
        #endregion

        private void init_ComboBox_Option_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;       //最大化
            this.MinimizeBox = false;       //最小化
            this.FormBorderStyle = FormBorderStyle.FixedSingle;     //限制使用者改變form大小
            this.AutoSize = false;          //自動調整大小
            //this.Size = new System.Drawing.Size(800, 600);      //設定Form大小

            default_status();       //預設狀態
            DGV1_View();
        }

        #region 按鈕
        //=============================================================================================

        private void init_CO_Add_Click(object sender, EventArgs e)      //新增
        {
            start_status(init_CO_Add);
        }

        private void init_CO_Modify_Click(object sender, EventArgs e)       //修改
        {
            start_status(init_CO_Modify);
        }

        private void init_CO_Del_Click(object sender, EventArgs e)      //刪除
        {
            start_status(init_CO_Del);
        }

        private void init_CO_Save_Click(object sender, EventArgs e)         //儲存
        {
            #region 內容
            fun.Check_error = false;
            Co_SN_ID = init_CO_Query_CB.SelectedValue.ToString();       //取得ComboBox設定的Value
            //MessageBox.Show(Co_SN_ID);
            fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSN_insert] '" +Co_SN_ID + "','" + tb_init_CO_Name.Text.Trim() + "','" + tb_init_CO_Other.Text.Trim() + "'";
            fun.Asset_ComboBox_Value_All(fun.Query_DB);
            
            if (fun.Check_error==false)
            {
                MessageBox.Show("新增成功!!","財產管理系統");
                start_status(init_CO_Save);
                Get_SQL("查詢");
                fun.xxx_DB(fun.Query_DB, dataGridView1);
            }
            #endregion
        }

        private void init_CO_Cancel_Click(object sender, EventArgs e)       //取消
        {
            start_status(init_CO_Cancel);
        }        

        private void init_CO_Query_Click(object sender, EventArgs e)        //查詢
        {
            Get_SQL("查詢");
            fun.xxx_DB(fun.Query_DB, dataGridView1);
        }

        //=============================================================================================
        #endregion

    }
}
