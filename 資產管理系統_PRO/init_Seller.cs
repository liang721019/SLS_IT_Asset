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
    public partial class init_Seller : Form
    {

        Asset_init_function fun = new Asset_init_function();
        init_PRO iPO = null;
        
        public init_Seller(init_PRO SS , bool KK)
        {
            InitializeComponent();
            iPO = SS;
            fun.DoubleClick_Enable = KK;
        }

        private void init_Seller_Load(object sender, EventArgs e)
        {
            default_status();
            DGV1_View();        //把資料顯示到DGV1自定義欄位            

        }

        #region 變數
        //***************************************************
        public string ServiceName
        {
            set
            {
                 this.init_sys_status.Text = value;
            }
            get
            {
                return this.init_sys_status.Text;
            }

        }

        public string USERID
        {
            set
            {
                this.init_toolStrip_UID_Value.Text = value;
            }
            get
            {
                return this.init_toolStrip_UID_Value.Text;
            }
        }

        public bool DoubleClick_Enable  //設定init_Seller中dataGridView1_CellDoubleClick開關
        {
            set;
            get;
        }
        //***************************************************
        #endregion

        #region 方法
        //============================================================================================================

        private void Get_SQL(Button x)
        {
            if (x == Seller_Query_button)           //<廠商查詢>
            {
                fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_Query] 'B','" + tb_retailNM.Text.Trim() + "'";
            }
            else if (x == Seller_AddNew_button)               //<廠商新增>
            {
                fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_insert] '" +
                                 tb_retailNM.Text.Trim() +                             //<廠商名稱>
                                 @"','" + tb_retail_Contact_NM.Text.Trim() +          //<聯絡人>
                                 @"','" + tb_retailSAPSN.Text.Trim() +                //<SAP編號>
                                 @"','" + tb_retail_Phone.Text.Trim() +               //<廠商電話>
                                 @"','" + tb_retailEmail.Text.Trim() +                //<EMAIL>
                                 @"','" + tb_retailACC.Text.Trim() + @"'";            //<地址>

            }
            else if (x == Seller_Modify_button)               //<廠商修改>
            {
                fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_updata] '" +
                        tb_retail_ID.Text.Trim() +
                        @"','" + tb_retailNM.Text.Trim() +
                        @"','" + tb_retail_Contact_NM.Text.Trim() +
                        @"','" + tb_retailSAPSN.Text.Trim() +
                        @"','" + tb_retail_Phone.Text.Trim() +
                        @"','" + tb_retailEmail.Text.Trim() +
                        @"','" + tb_retailACC.Text.Trim() + @"'";


            }
            else if (x == Seller_Del_betton)                //<廠商刪除>
            {

            }
            else if (x == Seller_Save_button)               //<廠商儲存>
            {

            }
        }

        private void default_status()        //預設狀態
        {
            this.Text = "廠商資料維護表";
            fun.Disabled_Panel(panel1);
            fun.Enabled_Panel_btn(panel2);
            Seller_Save_button.Enabled = false;
            Seller_Cancel_button.Enabled = false;
            this.MaximizeBox = false;       //最大化
            this.MinimizeBox = false;       //最小化
            this.FormBorderStyle = FormBorderStyle.FixedSingle;     //限制使用者改變form大小
            this.AutoSize = false;          //自動調整大小

            if (fun.DoubleClick_Enable == true)
            {
                Seller_Import_button.Visible = true;
            }
            else
            {
                Seller_Import_button.Visible = false;
            }
        }

        private void start_status(Button btu)    //各按鈕影響資料的初始狀態
        {
            if (btu == Seller_AddNew_button)     //廠商新增
            {
                fun.Page_Name(this, this.Text, "<新增>");
                fun.Disabled_Panel_btn(panel2);
                fun.Enabled_Panel(panel1);
                Seller_Cancel_button.Enabled = true;
                Seller_Save_button.Enabled = true;
                tb_retail_ID.ReadOnly = true;                
            }
            else if (btu == Seller_Modify_button)       //廠商修改
            {
                fun.Page_Name(this, this.Text, "<修改>");
                fun.Disabled_Panel_btn(panel2);
                fun.Enabled_Panel(panel1);
                Seller_Cancel_button.Enabled = true;
                Seller_Save_button.Enabled = true;
                tb_retail_ID.ReadOnly = true;

            }
            else if (btu == Seller_Query_button)       //廠商查詢
            {
                fun.Page_Name(this,this.Text,"<查詢>");                
                fun.Disabled_Panel_btn(panel2);
                Seller_Cancel_button.Enabled = true;
                tb_retailNM.ReadOnly = false;
                tb_retailNM.Focus();        //游標移到這個TextBox

            }
            else if (btu == Seller_Del_betton)      //廠商刪除
            {

            }
            else if (btu == Seller_Save_button)     //廠商儲存
            {

            }
            else if (btu == Seller_Cancel_button)       //廠商取消
            {
                default_status();
                fun.clearAir(panel1);
            }
        }

        public void check_key()         //確認TextBox是否有值
        {
            fun.check_OK = false;
            fun.check_str = "";
            fun.save_method(tb_retailNM, "廠商名稱: \n");
            //fun.save_method(tb_retail_Contact_NM, "聯絡人: \n");
            //fun.save_method(tb_retail_Phone, "電話: \n");
            if (fun.check_str != "")
            {
                fun.check_str = "以下欄位, 必需有值: \n" + fun.check_str;
                MessageBox.Show(fun.check_str, "警告!!");
            }
            else
            {
                fun.check_OK = true;
            }

        }

        public void DGV1_View()         //把資料顯示到DGV1自定義欄位
        {
            //DataGridView
            //dataGridView1
            dataGridView1.AutoGenerateColumns = false;
            DGV1_Column1.DataPropertyName = "廠商編號";
            DGV1_Column2.DataPropertyName = "廠商名稱";
            DGV1_Column3.DataPropertyName = "SAP編號";
            DGV1_Column4.DataPropertyName = "廠商聯絡人";
            DGV1_Column5.DataPropertyName = "電話";
            DGV1_Column6.DataPropertyName = "EMAIL";
            DGV1_Column7.DataPropertyName = "地址";
            DGV1_Column1.Frozen = true;
            DGV1_Column2.Frozen = true;
        }

        //============================================================================================================
        #endregion

        #region 事件
        //***************************************************
        private void tb_retailNM_KeyDown(object sender, KeyEventArgs e)     //廠商名稱~按下Enter要處理的事
        {
            #region 內容
            if (this.Text == "廠商資料維護表<查詢>")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Get_SQL(Seller_Query_button);
                    fun.xxx_DB(fun.Query_DB, dataGridView1);
                    default_status();
                }
            }
            #endregion

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)      //dataGridView1點二下要處理的事
        {


            if (fun.DoubleClick_Enable == true)
            {
                if (e.RowIndex >= 0)
                {
                    string seller_ID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_Query] 'A','" + seller_ID + @"'";
                    fun.ProductDB_ds(fun.Query_DB);
                    iPO.tb_retail_ID.Text = fun.ds_index.Tables[0].Rows[0]["廠商編號"].ToString();
                    iPO.tb_retailNM.Text = fun.ds_index.Tables[0].Rows[0]["廠商名稱"].ToString();
                    iPO.tb_retail_Contact_NM.Text = fun.ds_index.Tables[0].Rows[0]["廠商聯絡人"].ToString();
                    iPO.tb_retail_Phone.Text = fun.ds_index.Tables[0].Rows[0]["電話"].ToString();
                    iPO.tb_retailEmail.Text = fun.ds_index.Tables[0].Rows[0]["EMAIL"].ToString();
                    iPO.tb_retailACC.Text = fun.ds_index.Tables[0].Rows[0]["地址"].ToString();

                    this.Close();

                }
            }

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)      //dataGridView1點左鍵滑鼠事件
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    string seller_ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_Query] 'C','" + seller_ID + @"'";
                    fun.ProductDB_ds(fun.Query_DB);
                    tb_retail_ID.Text = fun.ds_index.Tables[0].Rows[0]["廠商編號"].ToString();
                    tb_retailNM.Text = fun.ds_index.Tables[0].Rows[0]["廠商名稱"].ToString();
                    tb_retail_Contact_NM.Text = fun.ds_index.Tables[0].Rows[0]["廠商聯絡人"].ToString();
                    tb_retail_Phone.Text = fun.ds_index.Tables[0].Rows[0]["電話"].ToString();
                    tb_retailSAPSN.Text = fun.ds_index.Tables[0].Rows[0]["SAP編號"].ToString();
                    tb_retailEmail.Text = fun.ds_index.Tables[0].Rows[0]["EMAIL"].ToString();
                    tb_retailACC.Text = fun.ds_index.Tables[0].Rows[0]["地址"].ToString();
                    default_status();

                }
            }

        }

        
        //***************************************************
        #endregion

        #region button
        //**********************************************************
        private void Seller_AddNew_button_Click(object sender, EventArgs e)     //新增
        {
            fun.clearAir(panel1);
            start_status(Seller_AddNew_button);
        }

        private void Seller_Del_betton_Click(object sender, EventArgs e)        //刪除
        {
            MessageBox.Show("沒有權限~~請連絡管理者", "警告!!");

        }

        private void Seller_Query_button_Click(object sender, EventArgs e)      //查詢
        {
            fun.clearAir(panel1);
            start_status(Seller_Query_button);
        }

        private void Seller_Cancel_button_Click(object sender, EventArgs e)     //取消
        {
            if (MessageBox.Show("確定要取消？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                start_status(Seller_Cancel_button);
            }

        }

        private void Seller_Save_button_Click(object sender, EventArgs e)       //儲存
        {
            check_key();
            if (fun.check_OK == true)
            {
                if (this.Text == "廠商資料維護表<新增>")
                {
                    #region 廠商資料維護表<新增>
                    if (MessageBox.Show("確定要新增？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Get_SQL(Seller_Query_button);
                        fun.ProductDB_ds(fun.Query_DB);
                        if (fun.ds_index.Tables[0].Rows.Count != 0)
                        {
                            MessageBox.Show("廠商名稱:" + tb_retailNM.Text.Trim() + "已存在!!!", "廠商資料");
                        }
                        else
                        {
                            Get_SQL(Seller_AddNew_button);
                            fun.DB_PJ_insert(fun.Query_DB, null, "新增成功", "廠商資料");
                            fun.clearAir(panel1);
                            default_status();

                        }
                    }

                    #endregion
                }
                else if (this.Text == "廠商資料維護表<修改>")
                {
                    #region 廠商資料維護表<修改>
                    if (MessageBox.Show("確定要修改？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Get_SQL(Seller_Query_button);
                        fun.ProductDB_ds(fun.Query_DB);
                        if (fun.ds_index.Tables[0].Rows.Count != 0)
                        {
                            Get_SQL(Seller_Modify_button);
                            fun.DB_PJ_insert(fun.Query_DB, null, "修改成功", "廠商資料");
                            fun.clearAir(panel1);
                            default_status();

                        }
                    }

                    #endregion

                }
            }


        }

        private void Seller_Modify_button_Click(object sender, EventArgs e)     //修改
        {
            if (tb_retail_ID.Text == "" && tb_retailNM.Text == "")
            {
                MessageBox.Show("請使用查詢帶入欲修改之資料!", "廠商資料");
            }
            else
            {
                start_status(Seller_Modify_button);
            }
        }

        private void Seller_Import_button_Click(object sender, EventArgs e)         //<導入資料>
        {
            #region 內容
            if (fun.DoubleClick_Enable == true)
            {
                #region 內容
                if (tb_retail_ID.Text != "")
                {
                    string seller_ID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_AssetSeller_Query] 'A','" + seller_ID + @"'";
                    fun.ProductDB_ds(fun.Query_DB);
                    iPO.tb_retail_ID.Text = fun.ds_index.Tables[0].Rows[0]["廠商編號"].ToString();
                    iPO.tb_retailNM.Text = fun.ds_index.Tables[0].Rows[0]["廠商名稱"].ToString();
                    iPO.tb_retail_Contact_NM.Text = fun.ds_index.Tables[0].Rows[0]["廠商聯絡人"].ToString();
                    iPO.tb_retail_Phone.Text = fun.ds_index.Tables[0].Rows[0]["電話"].ToString();
                    iPO.tb_retailEmail.Text = fun.ds_index.Tables[0].Rows[0]["EMAIL"].ToString();
                    iPO.tb_retailACC.Text = fun.ds_index.Tables[0].Rows[0]["地址"].ToString();

                    this.Close();
                }
                #endregion
            }
            #endregion
        }
        //**********************************************************
        #endregion        
    }
}
