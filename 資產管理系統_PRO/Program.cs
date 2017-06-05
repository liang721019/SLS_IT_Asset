using LOGIN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 財產管理系統
{
    static class Program
    {

        private class AssetLOGIN : Login_main
        {
            public override void V_login_SetENV()      //設定LOGIN變數
            {
                base.V_login_SetENV();
                //Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_QS_Login] '" +
                //                ID_Login + @"','" + App_LoginPW + "'";
                Query_DB = @"exec [TEST_SLSYHI].[dbo].[SLS_Asset_Login] '" +
                                        ID_Login +
                                        @"','" + App_LoginPW + "'";
            }            

            public override void V_login_open()      //開窗
            {
                //SLS_Asset_LOGIN
                init_PRO iPO = new init_PRO();
                string Query_DB = @"select * from [dbo].[SLS_Asset_LOGINTemp]('"+UID+"')";
                iPO.AssetFUN.USER_INFO(Query_DB, iPO.MYDS.Tables["SLS_Asset_LOGIN"]);         //登入後載入使用者資訊至DS
                //DataRow Asset_dr = iPO.MYDS.Tables["SLS_Asset_LOGIN"].NewRow();
                //Asset_dr["Check"] = "0";               
                //iPO.MYDS.Tables["SLS_Asset_LOGIN"].Rows.Add(Asset_dr);
                iPO.Service_ENV = ServerName;       //server
                iPO.UID = UID;          //使用者ID
                this.Hide();
                iPO.ShowDialog(this);
                this.Close();
            }

        }
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AssetLOGIN());
        }
    }
}
