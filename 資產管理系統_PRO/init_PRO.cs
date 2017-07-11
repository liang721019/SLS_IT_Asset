using Novacode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using 財產管理系統;
using function.lib;

/*
 * ****************************************************************************************
 * @author                  :ericchen
 * @version                 :1.0
 * @Creation                :2016/10/6
 * @Modify                  :2017/06/14
 * ****************************************************************************************
 * 2017/06/14  :@增加財產卡對應的欄位(請購單號、購置日期、規格型式)
 * 2017/06/13  :@增加登入系統記錄
 * 2017/06/05  :@修改LOGIN程式
 * 2017/05/11  :@新增檔案選取-word檔
 * 2017/05/11  :@當按<取消>後-不會清除tb_PDFPosition中的文字 
*/

namespace 財產管理系統
{
    public partial class init_PRO : Form
    {
        Asset_init_function fun = new Asset_init_function();
        init_function initfun = new init_function();
        Asset_DS AssetDS = new Asset_DS();        
        
        public init_PRO()
        {
            InitializeComponent();
        }

        private void init_PRO_Load(object sender, EventArgs e)
        {            
            default_status();                      
            combobox_set();   //Combobox設定
            default_value(); //清空值
            SYS_log("登入成功");             //在DB記錄登入狀態            
            #region 日期格式自訂
            this.DTP_Asset_TrDate.CustomFormat = "yyyy/MM/dd";
            this.DTP_Asset_TrDate.Format = DateTimePickerFormat.Custom;
            this.dTP_Asset_GetDate_input.CustomFormat = "yyyy/MM/dd";
            this.dTP_Asset_GetDate_input.Format = DateTimePickerFormat.Custom;
            #endregion
        }

        #region  變數
        //======================================================================================

        public string SYS_TXT = "財產管理系統";

        public Asset_init_function AssetFUN      //Asset_init_function
        {
            get
            {
                return fun;
            }
        }

        public Asset_DS MYDS         //DS
        {
            get
            {
                return AssetDS;
            }
        }

        private string QueryDB          //SQL語法變數
        {
            set;
            get;
        }

        private string Query_DB_AssetATR     //SQL語法變數
        {
            set;
            get;
        }

        public string default_FileRoot
        {
            set;
            get;
        }

        public string Sys_AssetCard_Accress     //財產卡檔案存放位置
        {
            set;
            get;
        }

        public string Sys_Asset_ModifyInfo     //異動記錄檔存放位置
        {
            set;
            get;
        }

        public string Service_ENV           //伺服器變數
        {
            set;
            get;
        }

        public string UID               //使用者ID
        {
            set;
            get;
        }

        public string QueryDB_Condition         //其他查詢條件=>SQL語法的變數
        {
            set;
            get;
        }

        private string checkdata       //從DB取得審核資料-Y or N
        {
            set;
            get;
        }

        private string Local_PCNAME          //取得本機電腦名稱
        {
            get
            {
                return Environment.MachineName;
            }
        }

        private string Local_USERNAME        //取得登入win使用者名稱
        {
            get
            {
                return Environment.UserName;
            }
        }

        public string Local_MYMAC             //取得本機MAC位置
        {
            set;
            get;
        }

        public string Local_MYIP              //取得本機IP
        {
            set;
            get;
        }

        //======================================================================================
        #endregion

        #region  方法
        //======================================================================================
        private void Get_SQL(string xQL, string Q_value, string Q_value1)  //SQL語法
        {
            fun.Query_DB = "";
            if (xQL == "查詢")
            {
                #region <查詢>SQL語法
                fun.Query_DB = @"exec [dbo].[SLS_Asset_Query] '" + Q_value + "'";

                #endregion
            }
            else if (xQL == "保管人所有資產")
            {
                #region <保管人所有資產>SQL語法
                fun.Query_DB = @"exec [dbo].[SLS_Asset_Owner_QueryAll] '" + Q_value + "'";

                #endregion
            }
            else if (xQL == "資產異動紀錄")
            {
                #region <資產異動紀錄>SQL語法
                fun.Query_DB = @"exec [dbo].[SLS_Asset_ModifyQuery] '" + Q_value + "'";

                #endregion
            }
            else if (xQL == "新增-儲存")
            {
                #region <新增>SQL語法
                fun.Query_DB = @"exec [dbo].[SLS_Asset_insert]                                  
	                                     '" + tb_Asset_ID.Text.Trim().ToUpper() +             //資產編號         
                                         @"','" + tb_Asset_NM.Text.Trim() +           //資產名稱
                                         @"','" + cb_AssetATR.Text.Trim() +           //屬性
                                         @"','" + cb_AssetClass.Text.Trim() +          //類別
                                         @"','" + tb_Asset_Brand.Text.Trim() +        //品牌
                                         @"','" + tb_Asset_MD.Text.Trim() +            //型號
                                         @"','" + tb_Asset_Spec.Text.Trim() +         //規格
                                         @"','" + tb_Asset_SN.Text.Trim() +           //原廠序號
                                         @"','" + tb_Asset_Warranty.Text.Trim() +     //保固期限
                                         @"','" + tb_retail_ID.Text.Trim() +          //廠商ID
                                         @"','" + tb_QTY.Text.Trim() +                //數量
                                         @"','" + cb_Unit.Text.Trim() +               //單位
                                         @"','" + tb_Asset_GetDate.Text.Trim() +      //取得日期
                                         @"','" + tb_Asset_ExpDT.Text.Trim() +        //使用年限
                                         @"','" + cb_AssetStat.Text.Trim() +          //使用狀態
                                         @"','" + tb_Asset_Cost.Text.Trim() +         //原始價值   
                                         @"','" + tb_addValue.Text.Trim() +           //附加價值
                                         @"','" + tb_PR_NO.Text.Trim() +              //請購單號
                                         @"','" + cb_StoredLoc.Text.Trim() +          //存放地點
                    //@"','" + tb_OwnerDept_Lv1.Text.Trim() +      //保管人部門
                                         @"','" + tb_OwnerID.Text.Trim() +      //保管人員編
                                         @"','" + tb_Asset_REMARK.Text.Trim() +       //備註
                                         @"','N'" +
                                         @",''" +            //主檔PDF檔存放位置
                                         @",'" + tb_PDFPosition.Text.Trim() +       //主檔PDF檔檔名
                                         @"','" + init_toolStrip_UID_Value.Text +       //使用者ID
                                         @"','" + DTP_Asset_TrDate.Text +       //保管人異動日期
                                         "'";



                #endregion
            }
            else if (xQL == "修改-儲存")
            {
                #region <修改>SQL語法


                fun.Query_DB = @"exec [dbo].[SLS_Asset_Update] 
                                  '" + tb_Asset_ID.Text.Trim().ToUpper() +                  //資產ID
                                 "','" + tb_Asset_NM.Text.Trim() +                //資產名稱
                                 "','" + cb_AssetATR.Text.Trim() +                  //屬性
                                 "','" + cb_AssetClass.Text.Trim() +              //類別
                                 "','" + tb_Asset_Brand.Text.Trim() +             //品牌
                                 "','" + tb_Asset_MD.Text.Trim() +                //型號
                                 "','" + tb_Asset_Spec.Text.Trim() +              //規格
                                 "','" + tb_Asset_SN.Text.Trim() +                //原廠序號
                                 "','" + tb_Asset_Warranty.Text.Trim() +          //保固期限
                                 "','" + tb_retail_ID.Text.Trim() +               //廠商ID
                                 "','" + tb_QTY.Text.Trim() +                     //數量
                                 "','" + cb_Unit.Text.Trim() +                    //單位
                                 "','" + tb_Asset_GetDate.Text.Trim() +           //取得日期
                                 "','" + tb_Asset_ExpDT.Text.Trim() +             //使用年限
                                 "','" + cb_AssetStat.Text.Trim() +               //使用狀態
                                 "','" + tb_Asset_Cost.Text.Trim() +              //原始價值
                                 "','" + tb_addValue.Text.Trim() +                //附加價值
                                 "','" + tb_PR_NO.Text.Trim() +                   //請購單號
                                 "','" + cb_StoredLoc.Text.Trim() +               //存放地點
                                 "',''" +               //主檔PDF檔存檔位置
                                 ",'" + fun.SAUkey_tb_PDFPosition +               //主檔PDF檔名稱
                                 "','" + tb_OwnerID.Text.Trim() +               //保管人員編
                                 "','" +               //修改記錄檔存放位置
                                 "','" + fun.SAUkey_tb_PDFmodify_Position +            //修改記錄檔檔案名稱
                                 "','" + init_toolStrip_UID_Value.Text +               //User
                                 "','" + DTP_Asset_TrDate.Text +               //保管人異動日期
                                 "','" + tb_Asset_REMARK.Text.Trim() +       //備註
                                 "'";

                #endregion
            }
            else if (xQL == "保管人資產統計")
            {
                fun.Query_DB = @"exec [dbo].[SLS_Asset_QtyQuery] '" + Q_value + "'";

            }
            else if (xQL == "審核資料")
            {
                fun.Query_DB = @"exec [dbo].[SLS_Asset_Update_CheckData] '" + tb_Asset_ID.Text.Trim() + "','" + init_toolStrip_UID_Value.Text + "'";
            }
            else if (xQL == "其他查詢")
            {
                #region 其他查詢
                fun.Query_DB = @"select [Asset_ID]	As 資產編號
		                             ,[Asset_NAME]	AS 資產名稱
		                             ,[Asset_LOCATION]	AS 存放位置
		                             ,[EMP_ID]		AS 員工編號
		                             ,[EMP_Name]	AS 員工名稱
		                             ,[Dept]		AS 員工部門
		                             ,[ASSET_CheckData] AS 審核資料
                                     ,[ASSET_CKID]  AS 審核人員
                                from [dbo].[SLS_Asset_View]()
                                where [Asset_ID] is not null ";
                #endregion
            }

        }        

        public void default_status()  //預設控制元
        {
            //※在這個方法裡面不能使用fun.ProductDB_ds(fun.Query_DB)的方法※
            //※SYS_log不能放在這※
            this.Text = SYS_TXT;
            
            init_toolStrip_UID_Value.Text = UID;
            init_sys_status.Text = Service_ENV;             //取得DB連線名稱
            fun.ServiceName = init_sys_status.Text.Trim();           //設定DB連線伺服器名稱
            initfun.ServiceName = init_sys_status.Text.Trim();       //設定DB連線伺服器名稱
            fun.ReMAC(init_toolStrip_MAC_Value, init_toolStrip_IP_Value);         //取得本機MAC及IP

            #region panel元件<顯示>or<隱藏>
            init_panel.Visible = true;          //init_panel顯示
            init_DT_panel.Visible = false;      //init_DT_panel隱藏
            fun.Disabled_Panel_Tab(init_panel);         //關閉<init_panel>Tab
            fun.Disabled_Panel_Tab(seller_panel);       //關閉<seller_panel>Tab
            fun.Disabled_Panel_Tab(init_PDF_panel);     //關閉<init_PDF_panel>Tab
            fun.Login_check_ToF("Asset_ADD", true, AssetDS.SLS_Asset_LOGIN, init_Add_button);        //<新增>-依登入的使用者設定button顯示狀態
            fun.Login_check_ToF("Asset_Modify", true, AssetDS.SLS_Asset_LOGIN, init_Modify_button);  //<修改>-依登入的使用者設定button顯示狀態
            fun.Login_check_ToF("Asset_Del", true, AssetDS.SLS_Asset_LOGIN, init_Del_button);        //<刪除>-依登入的使用者設定button顯示狀態
            fun.Login_check_ToF("Asset_ADD", true, AssetDS.SLS_Asset_LOGIN, 廠商資料維護ToolStripMenuItem);        //依登入的使用者設定ToolStripMenuItem顯示狀態
            fun.Login_check_ToF("Asset_ROOT", true, AssetDS.SLS_Asset_LOGIN, 下拉選項設定ToolStripMenuItem);        //依登入的使用者設定ToolStripMenuItem顯示狀態
            fun.Login_check_ToF("Asset_ROOT", true, AssetDS.SLS_Asset_LOGIN, 系統設定ToolStripMenuItem);        //依登入的使用者設定ToolStripMenuItem顯示狀態
            #endregion

            #region 表頭控制項<啟動>or<關閉>
            cb_StoredLoc.Text = "";
            tb_Asset_ID.ReadOnly = true;         //<財產編號>的textbox唯讀
            btn_Asset_ID_input.Enabled = false;     //<導入財產編號>的Button
            tb_Asset_NM.ReadOnly = true;         //<財產名稱>的textbox唯讀
            cb_StoredLoc.Enabled = false;        //<存放地點>的combobox不啟動
            tb_OwnerID.ReadOnly = true;          //<保管人員編>的textbox唯讀
            tb_Owner_NM.ReadOnly = true;         //<保管人姓名>的textbox唯讀            
            tb_OwnerDept_Lv1.ReadOnly = true;    //<部門>的textbox唯讀
            btn_QueryEmpID.Enabled = false;      //<保管人員編查詢>關閉

            init_Add_button.Enabled = true;      //<新增>啟動
            init_Modify_button.Enabled = true;   //<修改>啟動
            init_QueryAsset_button.Enabled = true;      //<查詢>啟動
            init_Del_button.Enabled = true;         //<刪除>啟動
            init_Cancel_button.Enabled = false;     //<取消>不啟動
            init_Save_button.Enabled = false;    //<儲存>不啟動
            init_Cancel_button.Visible = false;     //<取消>隱藏
            init_Save_button.Visible = false;    //<儲存>隱藏
            #endregion

            #region 表身控制項<啟動>or<關閉>

            init_tabControl.Enabled = true;      //<分頁>可以切換
            cb_AssetATR.Enabled = false;         //<資產屬性>的combobox不啟動
            cb_AssetATR.Text = "";
            cb_AssetClass.Enabled = false;       //<資產類別>的combobox不啟動
            cb_AssetClass.Text = "";
            tb_FileCrDT.ReadOnly = true;         //<建檔日期>的textbox唯讀
            cb_AssetStat.Enabled = false;        //<使用狀態>的combobox不啟動
            cb_AssetStat.Text = "";            
            tb_Asset_Brand.ReadOnly = true;      //<品牌>的textbox唯讀     
            tb_Asset_MD.ReadOnly = true;         //<型號>的textbox唯讀
            tb_Asset_SN.ReadOnly = true;         //<產品序號>的textbox唯讀
            tb_Asset_Warranty.ReadOnly = true;   //<保固期限>的textbox唯讀 
            tb_Asset_ExpDT.ReadOnly = true;      //<使用年限>的textbox唯讀
            tb_Asset_Spec.ReadOnly = true;       //<規格明細>的textbox唯讀            
            tb_Asset_REMARK.ReadOnly = true;     //<備註>的textbox唯讀
            tb_QTY.ReadOnly = true;              //<數量>的textbox唯讀
            cb_Unit.Enabled = false;             //<單位>的combobox不啟動
            tb_PR_NO.ReadOnly = true;            //<請購單號>的textbox唯讀
            tb_Asset_GetDate.ReadOnly = true;    //<取得日期>的textbox唯讀
            dTP_Asset_GetDate_input.Enabled = false;    
            tb_Asset_Cost.ReadOnly = true;       //<購入金額>的textbox唯讀
            tb_addValue.ReadOnly = true;         //<附加價值>的textbox唯讀
            tb_Asset_ExpDT.ReadOnly = true;      //<使用年限>的textbox唯讀
            ck_Asset_PhaseOut.Enabled = false;    //<已逾年限>

            tb_retail_ID.ReadOnly = true;          //<廠商ID>的textbox唯讀
            tb_retailNM.ReadOnly = true;           //<廠商名稱>的textbox唯讀
            tb_retail_Contact_NM.ReadOnly = true;  //<廠商聯絡人>的textbox唯讀
            tb_retailEmail.ReadOnly = true;        //<廠商EMAIL>的textbox唯讀
            tb_retail_Phone.ReadOnly = true;       //<廠商電話>textbox唯讀
            tb_retailACC.ReadOnly = true;
            init_RetailData.Enabled = false;       //<廠商資料>按鈕關閉
            init_GenAssetCard.Enabled = false;     //<產生財產卡>按鈕關閉
            //tb_Asset_TrDate.Enabled = false;        //<保管人異動日期>的textbox唯讀
            DTP_Asset_TrDate.Enabled = false;       //<日期選擇器>按鈕關閉

            #endregion

            #region other
            fun.check_OK = false;            //check TexkBox全部有值就為True~不然就會false
            tb_PDFPosition.ReadOnly = true;         //<PDF檔案位置>唯讀
            tb_PDFmodify_Position.ReadOnly = true;      //<修改記錄存檔位置>唯讀
            tb_PDFPosition_view.ReadOnly = true;         //<PDF檔案位置>唯讀
            tb_PDFmodify_Position_view.ReadOnly = true;      //<修改記錄存檔位置>唯讀
            init_PDFbutton.Enabled = false;         //<檔案>關閉
            init_PDFmodify_button.Enabled = false;      //<檔案>關閉
            init_CheckData_button.Visible = false;      //<核准>關閉
            ck_Asset_CheckData_YES.Enabled = false;     //已審核=>唯讀
            ck_Asset_CheckData_NO.Enabled = false;      //未審核=>唯讀           
            this.tb_PDFPosition.AllowDrop = false;           //關閉<tb_PDFPosition>拖移功能
            this.tb_PDFmodify_Position.AllowDrop = false;    //關閉<tb_PDFmodify_Position>拖移功能            
            
            
            功能ToolStripMenuItem.Visible = true;
            功能ToolStripMenuItem.Enabled = true;
            

            #endregion
        }

        public void default_other_btn()     //其他-預設控制元件的初始狀態
        {            
            init_QueryOwnerAsset_button.Enabled = true;
            init_Save2_button.Enabled = false;
            init_Modify2_button.Enabled = false;
            init_Cancel2_button.Enabled = true;

            init_Save2_button.Visible = false;
            init_Modify2_button.Visible = false;

        }

        public void default_value()  //預設Textbox清空
        {            
            #region 表頭控制項<值>清空
            //================================================================
            #region panel1清空
            fun.clearAir(panel1);  
            #endregion

            #region panel2清空
            fun.clearAir(panel2);             
            #endregion
            //================================================================
            #endregion

            #region 表身控制項<值>清空 (資產明細)
            fun.clearAir(init_AssetDetail_panel);
            fun.clearAir(seller_panel);
            #endregion          

            #region other
            fun.clearAir(init_PDF_panel);
            
            #endregion
        }

        public void sub_()  //TestBOX與DB欄位的對應
        {
            try
            {
                #region 內容
                tb_Asset_ID.Text = fun.ds_index.Tables[0].Rows[0]["資產編號"].ToString();
                tb_Asset_NM.Text = fun.ds_index.Tables[0].Rows[0]["資產名稱"].ToString();                
                cb_StoredLoc.Text = fun.ds_index.Tables[0].Rows[0]["存放位置"].ToString();
                cb_AssetATR.Text = fun.ds_index.Tables[0].Rows[0]["屬性"].ToString();
                cb_AssetClass.Text = fun.ds_index.Tables[0].Rows[0]["類別"].ToString();
                tb_FileCrDT.Text = fun.ds_index.Tables[0].Rows[0]["建檔日期"].ToString();
                cb_AssetStat.Text = fun.ds_index.Tables[0].Rows[0]["資產狀態"].ToString();
                tb_Asset_Brand.Text = fun.ds_index.Tables[0].Rows[0]["品牌"].ToString();
                tb_Asset_MD.Text = fun.ds_index.Tables[0].Rows[0]["型號"].ToString();
                tb_Asset_SN.Text = fun.ds_index.Tables[0].Rows[0]["產品序號"].ToString();
                tb_Asset_Warranty.Text = fun.ds_index.Tables[0].Rows[0]["保固期限"].ToString();
                tb_Asset_Spec.Text = fun.ds_index.Tables[0].Rows[0]["規格明細"].ToString();
                tb_Asset_REMARK.Text = fun.ds_index.Tables[0].Rows[0]["備註"].ToString();
                tb_QTY.Text = fun.ds_index.Tables[0].Rows[0]["數量"].ToString();
                cb_Unit.Text = fun.ds_index.Tables[0].Rows[0]["單位"].ToString();
                tb_PR_NO.Text = fun.ds_index.Tables[0].Rows[0]["請購單號"].ToString();
                tb_Asset_GetDate.Text = fun.ds_index.Tables[0].Rows[0]["購入日期"].ToString();
                tb_Asset_Cost.Text = fun.ds_index.Tables[0].Rows[0]["購入金額"].ToString();
                tb_addValue.Text = fun.ds_index.Tables[0].Rows[0]["附加價值"].ToString();
                tb_Asset_ExpDT.Text = fun.ds_index.Tables[0].Rows[0]["耐用年限"].ToString();
                tb_retail_ID.Text = fun.ds_index.Tables[0].Rows[0]["廠商ID"].ToString();
                tb_retailNM.Text = fun.ds_index.Tables[0].Rows[0]["廠商名稱"].ToString();
                tb_retail_Contact_NM.Text = fun.ds_index.Tables[0].Rows[0]["廠商連絡人"].ToString();
                tb_retail_Phone.Text = fun.ds_index.Tables[0].Rows[0]["廠商電話"].ToString();
                tb_retailEmail.Text = fun.ds_index.Tables[0].Rows[0]["EMAIL"].ToString();
                tb_retailACC.Text = fun.ds_index.Tables[0].Rows[0]["地址"].ToString();
                tb_OwnerID.Text = fun.ds_index.Tables[0].Rows[0]["保管人員編"].ToString();
                tb_Owner_NM.Text = fun.ds_index.Tables[0].Rows[0]["保管人"].ToString();
                tb_OwnerDept_Lv1.Text = fun.ds_index.Tables[0].Rows[0]["保管部門"].ToString();
                tb_PDFPosition_view.Text = fun.ds_index.Tables[0].Rows[0]["PDF檔案位置"].ToString();
                tb_PDFmodify_Position_view.Text = fun.ds_index.Tables[0].Rows[0]["異動記錄檔"].ToString();
                //tb_Asset_TrDate.Text = fun.ds_index.Tables[0].Rows[0]["保管人異動日期"].ToString();
                DTP_Asset_TrDate.Text = fun.ds_index.Tables[0].Rows[0]["保管人異動日期"].ToString();
                checkdata = fun.ds_index.Tables[0].Rows[0]["審核資料"].ToString();
                if (checkdata == "Y")
                {
                    ck_Asset_CheckData_YES.Checked = true;
                    ck_Asset_CheckData_NO.Checked = false;                    
                    Login_check_ToF("Asset_CheckData", false, init_CheckData_button);      //核准-依登入的使用者設定button顯示狀態
                }
                else
                {
                    ck_Asset_CheckData_YES.Checked = false;
                    ck_Asset_CheckData_NO.Checked = true;
                    Login_check_ToF("Asset_CheckData", true, init_CheckData_button);      //核准-依登入的使用者設定button顯示狀態
                }
                
                #endregion

            }
            catch (Exception z)
            {
                System.Windows.Forms.MessageBox.Show(z.Message);
            }
            
        }

        public void combobox_set()  //combobox設定
        {
            Query_DB_AssetATR = @"SELECT [SN_NAME] FROM [dbo].[SLS_AssetSN] where [SN_ID] = 'A'";
            fun.ComboboxDB(Query_DB_AssetATR, "SN_NAME", cb_AssetATR);
            Query_DB_AssetATR = @"SELECT [SN_NAME], [SN_NAME]+'-'+[SN_DES] AS SName FROM [dbo].[SLS_AssetSN] where [SN_ID] = 'B'";
            fun.ComboboxDB(Query_DB_AssetATR, "SN_NAME", cb_AssetClass);
            Query_DB_AssetATR = @"SELECT [SN_NAME] FROM [dbo].[SLS_AssetSN] where [SN_ID] = 'C'";
            fun.ComboboxDB(Query_DB_AssetATR, "SN_NAME", cb_AssetStat);
            Query_DB_AssetATR = @"SELECT [SN_NAME] FROM [dbo].[SLS_AssetSN] where [SN_ID] = 'U'";
            fun.ComboboxDB(Query_DB_AssetATR, "SN_NAME", cb_Unit);
            Query_DB_AssetATR = @"SELECT [SN_NAME] FROM [dbo].[SLS_AssetSN] where [SN_ID] = 'OT'";
            fun.ComboboxDB(Query_DB_AssetATR, "SN_NAME", cb_StoredLoc);

        }                 

        public void check_key()         //check TexkBox是否有填入值
        {
            fun.check_OK = false;
            fun.check_str = "";            
            fun.save_method(tb_Asset_ID, "資產編號: \n");
            fun.save_method(tb_Asset_NM, "資產名稱: \n");
            fun.save_method(cb_StoredLoc, "存放地點: \n");
            fun.save_method(tb_OwnerID, "保管人員編: \n");
            fun.save_method(cb_AssetATR, "資產屬性: \n");            
            fun.save_method(cb_AssetClass, "資產類別: \n");
            fun.save_method(cb_AssetStat, "使用狀態: \n");
            fun.save_method(tb_QTY, "數    量: \n");
            fun.save_method(cb_Unit, "單    位: \n");
            fun.save_method(tb_PR_NO, "請購單號: \n");
            fun.save_method(tb_Asset_Cost, "購入金額: \n");
            fun.save_method(tb_addValue, "附加價值: \n");
            fun.save_method(tb_Asset_ExpDT, "使用年限: \n");
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

        private void start_status(System.Windows.Forms.Button ss)    //各按鈕影響資料的初始狀態
        {
            
            if (ss == init_Add_button)     //新增
            {
                #region 新增
                fun.Enabled_Panel(panel1);                   //開啟panel1
                fun.Enabled_Panel(init_AssetDetail_panel);             //開啟init_AssetDetail_panel
                fun.openCT(init_tabPage1);             //開啟init_tabPage1
                tb_Asset_ID.ReadOnly = true;            //<財產編號>的textbox唯讀
                btn_Asset_ID_input.Enabled = true;     //<導入財產編號>的Button
                init_Add_button.Enabled = false;      //<新增>不啟動
                init_Modify_button.Enabled = false;   //<修改>不啟動
                init_QueryAsset_button.Enabled = false;      //<查詢>不啟動
                init_Del_button.Enabled = false;         //<刪除>不啟動

                init_Cancel_button.Visible = true;     //<取消>顯示
                init_Save_button.Visible = true;    //<儲存>顯示
                init_Cancel_button.Enabled = true;     //<取消>啟動
                init_Save_button.Enabled = true;    //<儲存>啟動
                init_RetailData.Enabled = true;       //<廠商資料>按鈕關閉
                btn_QueryEmpID.Enabled = true;      //<員工編號查詢>啟動
                dTP_Asset_GetDate_input.Enabled = true;     //<取得日期>
                

                this.tb_PDFPosition.AllowDrop = true;           //開啟<tb_PDFPosition>拖移功能
                this.tb_PDFmodify_Position.AllowDrop = true;    //開啟<tb_PDFmodify_Position>拖移功能

                tb_QTY.Text = "1";                
                tb_FileCrDT.ReadOnly = true;           //<建檔日期>textbox唯讀
                init_PDFbutton.Enabled = true;         //<檔案>啟動
                tb_Asset_ID.Focus();
                
                #endregion

            }
            else if (ss == init_Modify_button)     //修改
            {
                #region  修改
                fun.Enabled_Panel(panel1);             //開啟panel1
                fun.Enabled_Panel(init_AssetDetail_panel);             //開啟init_AssetDetail_panel
                fun.openCT(init_tabPage1);          //開啟init_tabPage1


                
                tb_Asset_ID.ReadOnly = true;    //<資產編號>唯讀
                tb_FileCrDT.ReadOnly = true;    //<建檔日期>唯讀
                init_Add_button.Enabled = false;      //<新增>不啟動
                init_Modify_button.Enabled = false;   //<修改>不啟動
                init_QueryAsset_button.Enabled = false;      //<查詢>不啟動
                init_Del_button.Enabled = false;        //<刪除>不啟動
                init_Cancel_button.Visible = true;      //<取消>顯示
                init_Save_button.Visible = true;        //<儲存>顯示
                init_Cancel_button.Enabled = true;      //<取消>啟動
                init_Save_button.Enabled = true;        //<儲存>啟動
                init_RetailData.Enabled = true;         //<廠商資料>啟動
                init_PDFbutton.Enabled = true;          //<檔案>啟動-PDF主檔
                init_PDFmodify_button.Enabled = true;          //<檔案>啟動-修改記錄
                btn_QueryEmpID.Enabled = true;          //<保管人員編查詢>啟動
                DTP_Asset_TrDate.Enabled = true;        //<保管人異動日期>啟動
                dTP_Asset_GetDate_input.Enabled = true;     //<取得日期>

                this.tb_PDFPosition.AllowDrop = true;           //開啟<tb_PDFPosition>拖移功能
                this.tb_PDFmodify_Position.AllowDrop = true;    //開啟<tb_PDFmodify_Position>拖移功能

                #endregion
            }
            else if (ss == init_Del_button)        //刪除
            {
                #region 刪除
                #endregion

            }
            else if (ss == init_QueryAsset_button)     //查詢
            {
                #region 查詢
                tb_Asset_ID.Text = "";
                init_Add_button.Enabled = false;
                init_Modify_button.Enabled = false;
                init_Del_button.Enabled = false;
                tb_Asset_ID.ReadOnly = false;
                init_QueryAsset_button.Enabled = false;
                init_Cancel_button.Visible = true;     //<取消>顯示
                init_Save_button.Visible = false;    //<儲存>顯示
                init_Cancel_button.Enabled = true;     //<取消>顯示
                init_GenAssetCard.Enabled = false;     //<產生財產卡>按鈕關閉
                fun.clearAir(init_tabPage1);           //清空Textbox及Combobox內容
                fun.clearAir(panel1);                  //清空Textbox及Combobox內容
                fun.clearAir(init_PDF_panel);          //清空Textbox及Combobox內容
                //axAcroPDF1.LoadFile(fun.File_Position);
                tb_Asset_ID.Focus();        //游標移到這個TextBox
                init_PDFbutton.Enabled = false;         //<檔案>關閉
                init_PDFmodify_button.Enabled = false;      //<檔案>關閉

                #endregion

            }
            else if (ss == init_Save_button)       //儲存
            {

            }
            else if (ss == init_Cancel_button)     //取消
            {
                #region 取消
                if (this.Text == SYS_TXT + "-[新增]")
                {
                    default_value(); //清空值
                    default_status();  //預設值
                }
                else if (this.Text == SYS_TXT + "-[修改]")
                {
                    this.Text = SYS_TXT;
                    fun.Enabled_Panel_btn(init_panel);
                    fun.Disabled_Panel(panel1);
                    fun.Disabled_TabPage(init_tabPage1);
                    fun.Disabled_TabPage_btn(init_tabPage1);
                    fun.Disabled_Panel(init_AssetDetail_panel);

                    init_Save_button.Enabled = false;
                    init_Cancel_button.Enabled = false;
                    init_RetailData.Enabled = false;
                    init_PDFbutton.Enabled = false;
                    init_PDFmodify_button.Enabled = false;
                    btn_QueryEmpID.Enabled = false;
                    dTP_Asset_GetDate_input.Enabled = false;
                    DTP_Asset_TrDate.Enabled = false;

                    this.tb_PDFPosition.AllowDrop = false;           //關閉<tb_PDFPosition>拖移功能
                    this.tb_PDFmodify_Position.AllowDrop = false;    //關閉<tb_PDFmodify_Position>拖移功能
                    fun.clear_method(init_AssetDetail_panel);
                    tb_PDFPosition.Text = "";
                    tb_PDFmodify_Position.Text = "";

                }
                else if (this.Text == SYS_TXT + "-[查詢]")
                {
                    default_value(); //清空值
                    default_status();  //預設值
                }
                dataGridView1.Columns.Clear();          //清除dataGridView1的資料
                dataGridView2.Columns.Clear();          //清除dataGridView2的資料

                #endregion
            }
            else if (ss == init_QueryOwnerAsset_button)         //其他-查詢
            {
                #region 其他-查詢
                //btn_QueryEmpID.Enabled = true;
                //init_QueryOwnerAsset_button.Enabled = false;
                //init_Save2_button.Enabled = false;
                //init_Modify2_button.Enabled = false;
                //init_Cancel2_button.Enabled = true;

                #endregion
            }
            else if (ss == init_Modify2_button)             //其他-修改
            {


            }
            else if (ss == init_Cancel2_button)         //其他-取消
            {
                default_other_btn();                
            }

        }

        private void SYS_log(string x)        //在DB記錄登入狀態
        {
            
            int N = init_toolStrip_IP_Value.Text.LastIndexOf(".");
            int Q = init_toolStrip_IP_Value.Text.Length;
            initfun.Local_ID = init_toolStrip_UID_Value.Text;
            initfun.Local_SYS = this.Text;
            initfun.Local_PROC_NAME = x;
            initfun.Local_MYIP = init_toolStrip_IP_Value.Text.Substring(N,Q-N);            
            initfun.Local_MYMAC = "";
            initfun.Local_HOST_NAME = init_sys_status.Text;
            initfun.Local_PCNAME = Environment.MachineName;
            initfun.Local_USERNAME = Environment.UserName;
            initfun.Login_log();
        }

        private void 登出Button()             //登出
        {
            #region 內容
            SYS_log("登出");
            LOGIN_UI Login = new LOGIN_UI();
            this.Hide();
            Login.ShowDialog();
            this.Close();
            #endregion
        }

        public void File_SAccress_Get()            //取得檔案存放位置及傳送檔案
        {
            string GetDB_DTime = @"SELECT CONVERT(nvarchar(6), getdate(), 12)+replace(CONVERT(nvarchar(6),GETDATE(),108),':','') AS NO";
            fun.ProductDB_ds(GetDB_DTime);
            string GetDB_DTime_Value = fun.ds_index.Tables[0].Rows[0]["NO"].ToString();

            #region 系統狀態
            //Sys_AssetCard_Accress =x
            //Sys_Asset_ModifyInfo =y
            if (init_sys_status.Text == "PRD")
            {
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper(); 
            }
            else if (init_sys_status.Text == "QAS")
            {
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper(); ;
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
                

            }
            else if (init_sys_status.Text == "DEV")
            {
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();

            }
            #endregion
            #region 傳檔內容
            //把<資產主檔>及<異動記錄>的檔案上傳的方法同時更新資料庫
            if (tb_PDFPosition.Text != "")      //<資產主檔>
            {
                fun.SAUkey_tb_PDFPosition = tb_Asset_ID.Text.ToUpper() + "-" + Path.GetFileName(tb_PDFPosition.Text);
                fun.File_UAccress(tb_PDFPosition.Text, Sys_AssetCard_Accress, tb_Asset_ID.Text, "《資產主檔》", null, null); //<資產主檔>檔案傳送到預設位置

            }
            else
            {
                fun.SAUkey_tb_PDFPosition = tb_PDFPosition_view.Text;
            }

            if (tb_PDFmodify_Position.Text != "")       //<異動記錄>
            {
                fun.SAUkey_tb_PDFmodify_Position = tb_OwnerID.Text + Path.GetFileNameWithoutExtension(tb_PDFmodify_Position.Text) + "_" + GetDB_DTime_Value + Path.GetExtension(tb_PDFmodify_Position.Text) ;
                fun.File_UAccress(tb_PDFmodify_Position.Text, Sys_Asset_ModifyInfo, tb_Asset_ID.Text, "《異動記錄》", tb_OwnerID.Text, GetDB_DTime_Value);//<異動記錄>檔案傳送到預設位置
            }
            else
            {
                fun.SAUkey_tb_PDFmodify_Position = tb_PDFmodify_Position_view.Text;
            }
            #endregion  

        }

        public void File_SAccress_T()           //辨別系統環境=>取得檔案位置
        {
            #region 系統狀態
            if (init_sys_status.Text == "PRD")
            {               
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() +"\\" + tb_Asset_ID.Text.ToUpper();
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = '002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
            }
            else if (init_sys_status.Text == "QAS")
            {
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'QAS002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
            }
            else if (init_sys_status.Text == "DEV")
            {
                fun.Query_DB = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV001'";
                fun.Asset_AccressDB_ds(fun.Query_DB);
                Sys_AssetCard_Accress = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();
                fun.Query_DB_01 = @"SELECT [info_2] as 檔案位置  FROM [dbo].[SLS_AssetInfo]  where [info_1] = 'DEV002'";
                fun.Asset_AccressDB_ds(fun.Query_DB_01);
                Sys_Asset_ModifyInfo = fun.ds_index.Tables[0].Rows[0]["檔案位置"].ToString() + "\\" + tb_Asset_ID.Text.ToUpper();

            }
            #endregion
        }

        public void Other_QueryCondition()          //其他查詢條件
        {
            #region 內容
            QueryDB_Condition = "";          //其他查詢條件=>SQL語法的變數
            if (tb_Asset_ID.Text != "")
            {
                #region 資產編號
                QueryDB_Condition += @"and [Asset_ID] like '" + tb_Asset_ID.Text + "%'";
                #endregion

            }
            if (tb_Asset_NM.Text != "")
            {
                #region 資產名稱
                QueryDB_Condition += @"and [Asset_NAME] like '" + tb_Asset_NM.Text + "%'";
                #endregion
            }
            if (cb_StoredLoc.Text != "")
            {
                #region 存放位置
                QueryDB_Condition += @"and [Asset_LOCATION] like '" + cb_StoredLoc.Text + "%'";
                #endregion

            }
            if (tb_OwnerID.Text != "")
            {
                #region 保管人ID
                QueryDB_Condition += @"and [EMP_ID] like '" + tb_OwnerID.Text + "%'";
                #endregion

            }
            if (tb_Owner_NM.Text != "")
            {
                #region 保管人姓名
                QueryDB_Condition += @"and [EMP_Name] like '%" + tb_Owner_NM.Text + "%'";
                #endregion

            }
            if (tb_OwnerDept_Lv1.Text != "")
            {
                #region 保管人部門
                QueryDB_Condition += @"and [Dept] like '%" + tb_OwnerDept_Lv1.Text + "%'";
                #endregion
            }

            QueryDB_Condition += @"order by 1";
            #endregion
        }

        public void SYS_系統設定ToolStripMenuItem_Status_Key()      //判斷是否顯示<系統設定ToolStripMenuItem>
        {
            fun.Query_DB = @"SELECT [DMS_ROOT] FROM [dbo].[SLS_Employees] where [EMP_ID] = '" + init_toolStrip_UID_Value.Text + "' and [Asset_ROOT] = 'Y'";
            fun.ProductDB_ds(fun.Query_DB);
            if (fun.ds_index.Tables[0].Rows.Count == 0)
            {
                系統設定ToolStripMenuItem.Visible = false;
                系統設定ToolStripMenuItem.Enabled = false;
            }
            else
            {
                系統設定ToolStripMenuItem.Visible = true;
                系統設定ToolStripMenuItem.Enabled = true;
            }
        }

        private void Login_check_init_CheckData_Button(bool x)        //核准-依登入的使用者設定button顯示狀態
        {
            DataView DViewLOGIN = new DataView(AssetDS.SLS_Asset_LOGIN);
            DViewLOGIN.RowFilter = "Asset_CheckData = 'Y'";
            init_CheckData_button.Visible = DViewLOGIN.Count == 1 ? x /*<核准>打開*/ : false /*<核准>關閉*/;            
        }

        private void Login_check_ToF(string sx, bool y, System.Windows.Forms.Button Bonx)        //依登入的使用者設定button顯示狀態
        {
            //sx=>查詢條件(欄位)
            //y =>true OR false(當查詢有資料時~Button是否要顯示)
            //bonx =>設定button
            DataView DViewLOGIN = new DataView(AssetDS.SLS_Asset_LOGIN);
            DViewLOGIN.RowFilter = sx +" = 'Y'";
            Bonx.Visible = DViewLOGIN.Count == 1 ? y /*<核准>打開*/ : false /*<核准>關閉*/;  
        }

        private void Login_check_ToF(string sx, bool y, System.Windows.Forms.ToolStripMenuItem Bonx)        //依登入的使用者設定ToolStripMenuItem顯示狀態
        {
            //sx=>查詢條件(欄位)
            //y =>true OR false(當查詢有資料時~Button是否要顯示)
            //bonx =>設定button
            DataView DViewLOGIN = new DataView(AssetDS.SLS_Asset_LOGIN);
            DViewLOGIN.RowFilter = sx + " = 'Y'";
            Bonx.Visible = DViewLOGIN.Count == 1 ? y /*<核准>打開*/ : false /*<核准>關閉*/;
        }

        private void 廠商資料維護_FUN(bool x)
        {
            #region 內容
            //fun.DoubleClick_Enable = false;
            init_Seller inSea = new init_Seller(this, x);
            //設定init_Staff 新視窗的相對位置#############
            inSea.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //############################################
            //inSea.DoubleClick_Enable = false;
            inSea.ServiceName = this.init_sys_status.Text;
            inSea.USERID = this.init_toolStrip_UID_Value.Text;
            inSea.ShowDialog();
            #endregion
        }

        //======================================================================================
        #endregion

        #region button
        //============================================================================================================
        private void init_Add_button_Click(object sender, EventArgs e)  //新增
        {
            fun.Page_Name(this,this.Text,"-[新增]");
            default_value();        //清空值
            start_status(init_Add_button);                                               

            fun.Query_DB = @"SELECT CONVERT(VARCHAR(10),GETDATE(),111)";
            fun.ProductDB_ds(fun.Query_DB);             //建檔日期取值
            tb_FileCrDT.Text = fun.ds_index.Tables[0].Rows[0][0].ToString();       //建檔日期填入TextBox

        }

        private void init_Modify_button_Click(object sender, EventArgs e)    //修改
        {

            if (tb_Asset_ID.Text == "" && tb_OwnerID.Text == "")
            {
                MessageBox.Show("請使用查詢帶入欲修改之資料!", SYS_TXT);                
            }
            else
            {
                fun.Page_Name(this, this.Text, "-[修改]");  
                start_status(init_Modify_button);
            }

        }

        private void init_QueryAsset_button_Click(object sender, EventArgs e)  //查詢
        {
            fun.Page_Name(this,this.Text,"-[查詢]");
            start_status(init_QueryAsset_button);
        }

        private void btn_QueryEmpID2_Click(object sender, EventArgs e)    //前保管人
        {
            init_Staff inSt = new init_Staff(this);
            inSt.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            inSt.Location = new System.Drawing.Point(this.Left + btn_QueryEmpID.Right, this.Top + btn_QueryEmpID.Bottom);
            inSt.ShowDialog();

        }

        private void init_Del_button_Click(object sender, EventArgs e)      //刪除
        {
            MessageBox.Show("目前無權限刪除!!!!!", SYS_TXT);
        }

        private void btn_QueryEmpID_Click(object sender, EventArgs e)   //保管人
        {

            init_Staff inSt = new init_Staff(this);
            //設定init_Staff 新視窗的相對位置#############
            inSt.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            inSt.Location = new System.Drawing.Point(this.Left + btn_QueryEmpID.Right, this.Top + btn_QueryEmpID.Bottom);
            //############################################
            inSt.init_Staff_ServerName = Service_ENV;
            inSt.ShowDialog();

        }

        private void init_Save_button_Click(object sender, EventArgs e)  //儲存
        {

            if (this.Text == SYS_TXT+"-[新增]")
            {
                #region  財產管理系統-[新增]
                if (MessageBox.Show("確定要新增？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Get_SQL("查詢", tb_Asset_ID.Text.Trim(), null);
                    fun.ProductDB_ds(fun.Query_DB);
                    if (fun.ds_index.Tables[0].Rows.Count != 0)
                    {
                        MessageBox.Show("資產編號:" + tb_Asset_ID.Text.Trim() + "已存在!!!", SYS_TXT);

                    }
                    else
                    {
                        check_key();                //check TextBox是否有填入值
                        if (fun.check_OK == true)
                        {
                            //File_SAccress_Get(Sys_AssetCard_Accress, Sys_Asset_ModifyInfo);                //檔案存放位置取得的方法                            
                            File_SAccress_Get();                //檔案存放位置取得的方法 
                            Get_SQL("新增-儲存", null, null);
                            fun.DB_PJ_insert(fun.Query_DB, null, "新增成功", SYS_TXT);
                            //SYS_log("新增");
                            default_value(); //清空值
                            default_status();  //預設值
                        }

                    }

                }

                #endregion
            }
            else if (this.Text == SYS_TXT+"-[修改]")
            {
                #region 財產管理系統-[修改]
                if (MessageBox.Show("確定要修改？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    check_key();                //check TexkBox是否有填入值
                    if (fun.check_OK == true)
                    {
                        //File_SAccress_Get(Sys_AssetCard_Accress, Sys_Asset_ModifyInfo);                //檔案存放位置取得的方法
                        File_SAccress_Get();            //檔案存放位置取得的方法
                        Get_SQL("修改-儲存", null, null);
                        fun.DB_PJ_insert(fun.Query_DB, null, "修改成功", SYS_TXT);
                        default_value(); //清空值
                        default_status();  //預設值

                    }

                }

                #endregion
            }

        }

        private void init_Cancel_button_Click(object sender, EventArgs e)  //取消
        {

            if (MessageBox.Show("確定要取消？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                start_status(init_Cancel_button);
            }
        }

        private void init_RetailData_Click(object sender, EventArgs e)      //廠商資料
        { 
            廠商資料維護_FUN(true);
        }

        private void btn_Asset_ID_input_Click(object sender, EventArgs e)           //<導入財產編號>的按鈕
        {
            init_Asset_ID_Set iAI = new init_Asset_ID_Set(this);
            //設定init_Staff 新視窗的相對位置#############
            iAI.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //############################################
            iAI.init_Asset_ID_Set_ServerName = Service_ENV;
            iAI.ShowDialog();
        }

        private void 廠商資料維護ToolStripMenuItem_Click(object sender, EventArgs e)     //廠商資料維護按鈕
        {
            廠商資料維護_FUN(false);
        }

        private void 下拉選項設定ToolStripMenuItem_Click(object sender, EventArgs e)          //下拉選項設定
        {
            init_ComboBox_Option inCBO = new init_ComboBox_Option();
            //設定init_Staff 新視窗的相對位置#############
            inCBO.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //############################################
            inCBO.init_ComboBox_Option_ServerName = Service_ENV;
            inCBO.Text = 下拉選項設定ToolStripMenuItem.Text;
            inCBO.ShowDialog();
        }

        private void 維修記錄ToolStripMenuItem_Click(object sender, EventArgs e)            //維修記錄
        {
            init_Asset_ID_Set iAI = new init_Asset_ID_Set(this);
            //設定init_Staff 新視窗的相對位置#############
            iAI.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //############################################
            iAI.ShowDialog();
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)          //登出
        {
            登出Button();         //登出            
        }

        private void init_GenAssetCard_Click(object sender, EventArgs e)            //產生財產卡
        {
            try
            {
                // 讀取檔案 (*.docx)               
                string Docx_acc = @"\\192.168.100.210\exe_update\Asset\PRDX86\財產卡_空白.docx";
                DocX WDocument = DocX.Load(Docx_acc);
                SaveFileDialog sfd = new SaveFileDialog(); //通過SaveFileDialog類彈出一個保存對話方塊
                sfd.Filter = "Word 文件|*.docx";                
                sfd.FileName = tb_Asset_ID.Text.Trim();
                //sfd.FileName = DateTime.Now.ToString("yyyyMMddhhmmss"); //預設檔案名稱 
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfd.FileName;
                    // 取代
                    WDocument.ReplaceText("[$Asset_ID$]", tb_Asset_ID.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$Asset_NM$]", tb_Asset_NM.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$AssetATR$]", cb_AssetATR.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$Asset_ExpDT$]", tb_Asset_ExpDT.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$Asset_Cost$]", tb_Asset_Cost.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    //WDocument.ReplaceText("[$Asset_Spec$]", tb_Asset_Spec.Text.Replace(" ", "").Replace("\r\n", "　"), false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$Asset_Brand$]", tb_Asset_Brand.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$retailNM$]", tb_retailNM.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$retail_Contact_NM$]", tb_retail_Contact_NM.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$retail_Phone$]", tb_retail_Phone.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$tb_retailACC$]", tb_retailACC.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$tb_Asset_MD$]", tb_Asset_MD.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$tb_PR_NO$]", tb_PR_NO.Text, false, System.Text.RegularExpressions.RegexOptions.None);
                    WDocument.ReplaceText("[$Asset_GetDate$]", tb_Asset_GetDate.Text, false, System.Text.RegularExpressions.RegexOptions.None);

                    // 儲存檔案
                    WDocument.SaveAs(filename);
                    //wordDocument.SaveAs(@"D:\liang\資產管理系統資料\財產卡123.docx");        //存放到指定位置
                    MessageBox.Show("財產卡產生成功!", SYS_TXT);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void init_CheckData_button_Click(object sender, EventArgs e)            //核准
        {
            if (tb_Asset_ID.Text != "")
            {
                if (MessageBox.Show("確定要核准？", SYS_TXT, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Get_SQL("審核資料", null, null);
                    fun.CheckData_UPDB(fun.Query_DB);
                    ck_Asset_CheckData_YES.Checked = true;
                    ck_Asset_CheckData_NO.Checked = false;
                    init_CheckData_button.Visible = false;
                    tb_Asset_NM.Focus();
                }
            }

        }

        private void init_QueryOwnerAsset_button_Click(object sender, EventArgs e)      //其他-查詢
        {
            start_status(init_QueryOwnerAsset_button);
            if (init_tabControl.SelectedIndex == 2)
            {
                default_other_btn();
                Get_SQL("保管人資產統計", tb_OwnerID.Text, null);
                fun.xxx_DB(fun.Query_DB, this.dataGridView2);
            }
            else if (init_tabControl.SelectedIndex == 3)
            {
                default_other_btn();
                if (this.Text == SYS_TXT)
                {
                    if (tb_OwnerID.Text != "")
                    {
                        Get_SQL("保管人所有資產", tb_OwnerID.Text, null);
                        fun.xxx_DB(fun.Query_DB, this.dataGridView3);
                    }
                }
            }
            else if (init_tabControl.SelectedIndex == 4)
            {
                if (this.Text == SYS_TXT)
                {
                    default_other_btn();
                    Get_SQL("其他查詢", null, null);
                    Other_QueryCondition();          //其他查詢條件
                    fun.xxx_DB(fun.Query_DB + QueryDB_Condition, this.dataGridView4);
                }
            }

        }

        private void init_Cancel2_button_Click(object sender, EventArgs e)      //其他-取消
        {
            if (MessageBox.Show("確定要取消？", "警告!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                #region 內容
                if (this.Text == SYS_TXT)
                {
                    tb_Asset_ID.Text = "";
                    tb_Asset_NM.Text = "";
                    cb_StoredLoc.SelectedIndex = -1;
                    default_value();  //預設Textbox清空
                }
                #region 依目前的分項-清除dataGridView的內容
                if (init_tabControl.SelectedIndex == 2)
                {
                    dataGridView2.Columns.Clear();          //清除dataGridView2的資料
                }
                else if (init_tabControl.SelectedIndex == 3)
                {
                    dataGridView3.Columns.Clear();          //清除dataGridView3的資料
                }
                else if (init_tabControl.SelectedIndex == 4)
                {
                    dataGridView4.Columns.Clear();          //清除dataGridView4的資料
                }
                #endregion

                default_other_btn();
                start_status(init_Cancel2_button);
                tb_OwnerID.Text = "";
                tb_Owner_NM.Text = "";
                tb_OwnerDept_Lv1.Text = "";
                #endregion
            }

        }

        private void init_Modify2_button_Click(object sender, EventArgs e)      //其他-修改
        {

        }

        private void init_PDFbutton_Click(object sender, EventArgs e)           //PDF-主檔案
        {
            fun.MYPDF(null, tb_PDFPosition);
        }

        private void init_PDFmodify_button_Click(object sender, EventArgs e)        //PDF-修改記錄
        {
            fun.MYPDF(null, tb_PDFmodify_Position);
        }        
        
        public void refashDT(string x)
        {
            if (tb_OwnerID.Text != "")
            {
                Get_SQL("資產異動紀錄", tb_Asset_ID.Text, null);
                fun.xxx_DB(fun.Query_DB, this.dataGridView1);
                Get_SQL("保管人所有資產", tb_OwnerID.Text, null);
                fun.xxx_DB(fun.Query_DB, this.dataGridView3);

                Get_SQL("查詢", x, null);    //語法丟進fun.Query_DB
                fun.ProductDB_ds(fun.Query_DB);         //連接DB-執行DB指令
                sub_();         //TestBOX與DB欄位的對應
            }

        }

        //==============================================================================================================
        #endregion

        #region 事件

        private void tb_PDFPosition_view_DoubleClick(object sender, EventArgs e)        //財產卡檔名DoubleClick事件
        {
            if (tb_PDFPosition_view.Text != "")
            {
                try
                {
                    File_SAccress_T();          //依系統狀態取得設定的檔案存放位置
                    fun.openPdf(Sys_AssetCard_Accress + "\\" + tb_PDFPosition_view.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("異動資料有問題!! 無法開始檔案!!");
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("異動記錄沒有資料");
            }

        }

        private void tb_PDFmodify_Position_view_DoubleClick(object sender, EventArgs e)     //異動記錄檔名DoubleClick事件
        {
            if (tb_PDFmodify_Position_view.Text != "")
            {
                try
                {
                    File_SAccress_T();          //依系統狀態取得設定的檔案存放位置
                    fun.openPdf(Sys_Asset_ModifyInfo + "\\" + tb_PDFmodify_Position_view.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("異動資料有問題!! 無法開始檔案!!", SYS_TXT);
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("異動記錄沒有資料", SYS_TXT);
            }

        }

        private void tb_Asset_ID_KeyDown(object sender, KeyEventArgs e)  //資產編號按Enter要處理的事
        {            
            if (this.Text == SYS_TXT + "-[查詢]")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    #region  按Enter之後執行
                    Get_SQL("查詢", tb_Asset_ID.Text, null);    //語法丟進fun.Query_DB
                    fun.ProductDB_ds(fun.Query_DB);         //連接DB-執行DB指令
                    if (fun.ds_index.Tables[0].Rows.Count != 0)         //辨別是否有資產編號
                    {                        
                        default_status();  //預設值
                        sub_();         //TestBOX與DB欄位的對應
                        init_Add_button.Enabled = true;      //<新增>啟動
                        init_Modify_button.Enabled = true;   //<修改>啟動
                        init_QueryAsset_button.Enabled = true;      //<查詢>啟動
                        init_Del_button.Enabled = true;         //<刪除>啟動
                        init_Cancel_button.Enabled = false;     //<取消>不啟動
                        init_Save_button.Enabled = false;    //<儲存>不啟動
                        init_GenAssetCard.Enabled = true;     //<產生財產卡>按鈕開啟
                    }
                    else
                    {
                        MessageBox.Show("查無此編號!!!", SYS_TXT);
                    }                    
                    #endregion                   
                }
            }
            
        }
        
        private void init_tabControl_SelectedIndexChanged(object sender, EventArgs e)  //init_tabControl切換分頁設定
        {
            try
            {
                #region 內容
                if (init_tabControl.SelectedIndex == 0)         //分頁<資產明細>
                {
                    #region 分頁代號0
                    
                    init_panel.Visible = true;          //init_panel顯示
                    init_DT_panel.Visible = false;      //init_DT_panel隱藏
                    
                    if (this.Text == SYS_TXT)
                    {
                        btn_QueryEmpID.Enabled = false;
                        fun.EoD_Panel_txt(panel1, true);
                        fun.EoD_Panel_txt(panel2, true);
                        fun.EoD_Panel_ComboBox(panel1, false);
                    }
                    else if (this.Text == SYS_TXT+"-[查詢]")
                    {
                        btn_QueryEmpID.Enabled = false;
                    }
                    else 
                    {
                        btn_QueryEmpID.Enabled = true;
                    }
                    

                    if (tb_Asset_ID.Text != "")
                    {
                        Get_SQL("查詢", tb_Asset_ID.Text, null);    //語法丟進fun.Query_DB
                        fun.ProductDB_ds(fun.Query_DB);         //連接DB-執行DB指令
                        if (fun.ds_index.Tables[0].Rows.Count == 1)
                        {

                            sub_();         //TestBOX與DB欄位的對應
                        }
                        else
                        {
                            MessageBox.Show("查無此編號!!!", SYS_TXT);
                        }
                        
                    }
                    #endregion
                }
                else if (init_tabControl.SelectedIndex == 1)            //分頁<資產異動記錄>
                {
                    #region 分頁代號1
                    if (this.Text == SYS_TXT)
                    {
                        fun.EoD_Panel_txt(panel1, true);
                        fun.EoD_Panel_txt(panel2, true);
                        fun.EoD_Panel_ComboBox(panel1, false);
                    }
                    
                    init_panel.Visible = false;         //init_panel隱藏                    
                    init_DT_panel.Visible = false;       //init_DT_panel顯示
                    btn_QueryEmpID.Enabled = false; 
                    default_other_btn();                //其他-預設控制元件的初始狀態

                    if (tb_Asset_ID.Text != "")
                    {
                        Get_SQL("資產異動紀錄", tb_Asset_ID.Text, null);
                        fun.xxx_DB(fun.Query_DB, this.dataGridView1);
                    }
                    
                    #endregion
                }
                else if (init_tabControl.SelectedIndex == 2)            //分頁<保管人資產統計>
                {
                    #region 分頁代號2
                    //dataGridView2.Columns.Clear();      //清除dataGridView2的資料
                    if (this.Text == SYS_TXT)
                    {
                        btn_QueryEmpID.Enabled = true;
                        fun.EoD_Panel_txt(panel1, true);
                        fun.EoD_Panel_txt(panel2, true);
                        fun.EoD_Panel_ComboBox(panel1, false);

                    }
                    else
                    {
                        btn_QueryEmpID.Enabled = false;
                    }                    
                    init_panel.Visible = false;         //init_panel隱藏
                    init_DT_panel.Visible = true;       //init_DT_panel顯示                    
                    default_other_btn();                //其他-預設控制元件的初始狀態
                    #endregion
                }
                else if (init_tabControl.SelectedIndex == 3)            //分頁<保管人所有資產>
                {
                    #region 分頁代號3
                    //dataGridView3.Columns.Clear();      //清除dataGridView3的資料
                    if (this.Text == SYS_TXT)
                    {
                        btn_QueryEmpID.Enabled = true;
                        fun.EoD_Panel_txt(panel1, true);
                        fun.EoD_Panel_txt(panel2, true);
                        fun.EoD_Panel_ComboBox(panel1, false);
                        
                    }
                    else
                    {
                        btn_QueryEmpID.Enabled = false;
                    }
                    init_panel.Visible = false;         //init_panel隱藏
                    init_DT_panel.Visible = true;       //init_DT_panel顯示                    
                    default_other_btn();                //其他-預設控制元件的初始狀態
                    if (tb_OwnerID.Text != "")
                    {
                        Get_SQL("保管人所有資產", tb_OwnerID.Text, null);
                        fun.xxx_DB(fun.Query_DB, this.dataGridView3);
                    }
                    
                    #endregion
                }
                else if (init_tabControl.SelectedIndex == 4)            //分頁<其他查詢>
                {
                    #region 分頁代號4
                    if (this.Text == SYS_TXT)
                    {
                        btn_QueryEmpID.Enabled = true;
                        fun.EoD_Panel_txt(panel1, false);
                        fun.EoD_Panel_txt(panel2, false);
                        fun.EoD_Panel_ComboBox(panel1, true);
                    }
                    else
                    {
                        btn_QueryEmpID.Enabled = false;
                    }
                    init_panel.Visible = false;         //init_panel隱藏
                    init_DT_panel.Visible = true;       //init_DT_panel顯示
                    default_other_btn();                //其他-預設控制元件的初始狀態                    

                    #endregion
                }
                #endregion
            }
            catch (Exception x)
            {
                System.Windows.Forms.MessageBox.Show(x.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)      //dataGridView1左鍵二下
        {
            if (e.RowIndex >= 0)
            {
                string seller_ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();                
                string seller_ID_EID = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string seller_ID1 = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                string seller_ID2 = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                string QD = @"exec [dbo].[SLS_Asset_Transcation_Query] '" + seller_ID + seller_ID_EID + seller_ID1 + seller_ID2 + "'";
                            
                fun.ProductDB_ds(QD);         //連接DB-執行DB指令                
                string Transaction_FileName = fun.ds_index.Tables[0].Rows[0]["Asset_Position"].ToString();
                
                if (tb_PDFmodify_Position_view.Text != "")
                {
                    try
                    {
                        File_SAccress_T();          //依系統狀態取得設定的檔案存放位置                        
                        fun.openPdf(Sys_Asset_ModifyInfo + "\\" + Transaction_FileName);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("異動資料有問題!! 無法開始檔案!!");
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("異動記錄沒有資料");
                }

            }

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)         //dataGridView2左鍵二下
        {
            if (e.RowIndex >= 0)
            {
                if (this.Text == SYS_TXT)
                {
                    string seller_ID = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                    fun.Query_DB = @"SELECT [EMP_ID]			AS 員工編號
            	                    ,[EMP_Name]		AS 員工姓名                                    
            	                    ,[Dept_No]		AS 部門代碼
                                    ,[Dept]			AS 部門
                                    FROM [dbo].[SLS_AssetEmployees]
                                    where [EMP_ID] = '" + seller_ID + "'";
                    fun.ProductDB_ds(fun.Query_DB);
                    tb_OwnerID.Text = fun.ds_index.Tables[0].Rows[0]["員工編號"].ToString();
                    tb_Owner_NM.Text = fun.ds_index.Tables[0].Rows[0]["員工姓名"].ToString();
                    tb_OwnerDept_Lv1.Text = fun.ds_index.Tables[0].Rows[0]["部門"].ToString();                                             
                    init_tabControl.SelectedIndex = 3;

                }
                

            }
           
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)          //dateGridView3左鍵二下
        {
            if (e.RowIndex >= 0)
            {
                if (this.Text == SYS_TXT)
                {
                    string seller_ID = dataGridView3.CurrentRow.Cells[3].Value.ToString();                    
                    Get_SQL("查詢", seller_ID, null);    //語法丟進fun.Query_DB
                    fun.ProductDB_ds(fun.Query_DB);         //連接DB-執行DB指令
                    sub_();         //TestBOX與DB欄位的對應
                    init_GenAssetCard.Enabled = true;       //<產生財產卡>啟動
                    init_tabControl.SelectedIndex = 0;
                }
            }

        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)          //dateGridView3左鍵一下
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    if (this.Text == SYS_TXT)
                    {
                        string seller_ID = dataGridView3.CurrentRow.Cells["資產編號"].Value.ToString();
                        fun.Query_DB = @"exec [dbo].[SLS_Asset_Query] '" + seller_ID + @"'";
                        fun.ProductDB_ds(fun.Query_DB);
                        tb_Asset_ID.Text = fun.ds_index.Tables[0].Rows[0]["資產編號"].ToString();
                        tb_Asset_NM.Text = fun.ds_index.Tables[0].Rows[0]["資產名稱"].ToString();
                        cb_StoredLoc.Text = fun.ds_index.Tables[0].Rows[0]["存放位置"].ToString();                        
                    }                    

                }
            }
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)          //dateGridView4左鍵二下
        {
            if (e.RowIndex >= 0)
            {
                if (this.Text == SYS_TXT)
                {
                    string seller_ID = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                    tb_Asset_ID.Text = dataGridView4.CurrentRow.Cells[3].Value.ToString();
                    Get_SQL("查詢", seller_ID, null);    //語法丟進fun.Query_DB
                    fun.ProductDB_ds(fun.Query_DB);         //連接DB-執行DB指令
                    sub_();         //TestBOX與DB欄位的對應
                    init_GenAssetCard.Enabled = true;       //<產生財產卡>啟動
                    init_tabControl.SelectedIndex = 0;
                }
            }
        }

        #endregion
        
        #region 檔案拖放的方法
        //================================================================================================================
        private void tb_PDFPosition_DragDrop(object sender, DragEventArgs e)            //檔案拖到tb_PDFPosition後的事件
        {           
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                tb_PDFPosition.Text = file;     //取得完整路徑
                
                //tb_PDFPosition.Text = si.System_tb_01.Text +"\\"+ Path.GetFileName(file);       //取得檔名及副檔案
            }          

        }

        private void tb_PDFPosition_DragEnter(object sender, DragEventArgs e)           //檔案拖到tb_PDFPosition上方的事件
        {
            // 確定使用者抓進來的是檔案
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // 允許拖拉動作繼續 (這時滑鼠游標應該會顯示 +)
                e.Effect = DragDropEffects.All;
            }
        }

        private void tb_PDFmodify_Position_DragDrop(object sender, DragEventArgs e)         //檔案拖到tb_PDFmodify_Position後的事件
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                tb_PDFmodify_Position.Text = file;     //取得完整路徑                
            } 

        }

        private void tb_PDFmodify_Position_DragEnter(object sender, DragEventArgs e)        //檔案拖到tb_PDFmodify_Position上方的事件
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // 允許拖拉動作繼續 (這時滑鼠游標應該會顯示 +)
                e.Effect = DragDropEffects.All;
            }

        }


        //================================================================================================================
        #endregion
        
        #region 檔案存放設定相關
        //================================================================================================================
        private void 檔案存放設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            init_index si = new init_index(this);
            //設定init_Staff 新視窗的相對位置#############
            si.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //############################################
            si.init_index_ServerName = Service_ENV;
            si.Text = "檔案路徑資訊";                                
            si.ShowDialog();           

        }

        private void 系統設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dTP_Asset_GetDate_input_ValueChanged(object sender, EventArgs e)
        {
            tb_Asset_GetDate.Text = dTP_Asset_GetDate_input.Text;
        }
        
        //================================================================================================================
        #endregion
    }
}
