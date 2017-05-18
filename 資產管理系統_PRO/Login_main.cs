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
    public partial class Login_main : Form
    {

        init_function fun = new init_function();


        public Login_main()
        {
            InitializeComponent();
        }

        public string App_LoginPW 
        {
            set;
            get;
        }

        public string App_LoginNewPW
        {
            set;
            get;
        }        

        private void Login_main_Load(object sender, EventArgs e)
        {
            this.Text = "Login System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;//視窗在中央打開
            Login_ServerCB.Items.Add("PRD");
            Login_ServerCB.Items.Add("QAS");
            Login_ServerCB.Items.Add("DEV");           
            LoginMOD_ServerCB.Items.Add("PRD");
            LoginMOD_ServerCB.Items.Add("QAS");
            LoginMOD_ServerCB.Items.Add("DEV");
        }

        private void Login_Button_Click(object sender, EventArgs e)     //登入
        {
            if (Login_ServerCB.Text != "")
            {
                #region 內容
                if (Login_ServerCB.Text == "PRD")
                {
                    #region 內容
                    App_LoginPW = fun.desEncrypt_A(Login_PWD_tb.Text, "naturalbiokeyLogin");
                    fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_Asset_Login] '" +
                                        Login_ID_tb.Text +
                                        @"','" + App_LoginPW + "'";
                    fun.ProductDB_ds(fun.Query_DB);
                    if (fun.ds_index.Tables[0].Rows.Count != 0)
                    {
                        //MessageBox.Show("登入成功!!");
                        init_PRO iPO = new init_PRO();
                        iPO.Service_ENV = Login_ServerCB.Text;       //server
                        iPO.UID = Login_ID_tb.Text;          //使用者ID
                        this.Hide();
                        iPO.ShowDialog(this);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("帳密不正確!!", this.Text);
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("伺服器目前沒開放!!\n請選擇其他伺服器", this.Text);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("請選擇伺服器!!", this.Text);
            } 
        }

        private void Login_Cancel_Click(object sender, EventArgs e)     //取消
        {
            this.Close();
        }

        private void LoginMOD_Button_Click(object sender, EventArgs e)      //修改密碼<確定>
        {
            if (LoginMOD_ServerCB.Text != "")
            {                
                App_LoginPW = fun.desEncrypt_A(LoginOLD_PWD_tb.Text, "naturalbiokeyLogin");
                fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_Asset_Login] '" +
                                    LoginMOD_ID_tb.Text +
                                    @"','" + App_LoginPW + "'";
                fun.ProductDB_ds(fun.Query_DB);
                if (fun.ds_index.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("帳密不正確!!", this.Text);
                }
                else
                {
                    if (MessageBox.Show("確定要修改密碼？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        App_LoginNewPW = fun.desEncrypt_A(LoginNEW_PWD_tb.Text, "naturalbiokeyLogin");
                        fun.Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_Asset_Login_ModifyPWD] '" +
                                        LoginMOD_ID_tb.Text +@"','" + App_LoginNewPW + "'";
                        fun.DMS_modify(fun.Query_DB);
                        if (!fun.Check_error)
                        {
                            MessageBox.Show("密碼修改成功!!", this.Text);
                            fun.clearAir(Login_Modify_panel);
                            Login_tabControl.SelectedIndex = 0;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("請選擇伺服器!!", this.Text);
            }
        }

        private void LoginMOD_Cancel_Button_Click(object sender, EventArgs e)       //修改密碼<取消>
        {
            fun.clearAir(Login_Modify_panel);            
            Login_tabControl.SelectedIndex = 0;
        }
    }    
    
}
