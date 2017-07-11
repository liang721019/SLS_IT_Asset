using LOGIN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 財產管理系統
{
    class LOGIN_UI : Login_main
    {
        DataTable Asset_LOGIN_DT = new DataTable();

        public override void V_login_SetENV()      //設定LOGIN變數
        {
            base.V_login_SetENV();
            Query_DB = @"select * from [dbo].[SLS_Asset_LOGINTemp]('" + ID_Login + "','" + App_LoginPW + "')";
            LOD_DT = Asset_LOGIN_DT;
        }

        public override void V_login_open()      //開窗
        {
            DataView DV = new DataView(LOD_DT);
            DV.RowFilter = "Asset_Login = 'Y'";
            if (DV.Count == 1)
            {                
                //SLS_Asset_LOGIN
                init_PRO iPO = new init_PRO();
                //設定init_Staff 新視窗的相對位置#############
                iPO.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                //############################################                                
                DataRow iPODR = iPO.MYDS.SLS_Asset_LOGIN.NewRow();
                iPODR["EMP_ID"] = LOD_DT.Rows[0]["EMP_ID"];
                iPODR["EMP_Name"] = LOD_DT.Rows[0]["EMP_Name"];
                iPODR["Asset_Login"] = LOD_DT.Rows[0]["Asset_Login"];
                iPODR["Asset_ROOT"] = LOD_DT.Rows[0]["Asset_ROOT"];
                iPODR["Asset_ADD"] = LOD_DT.Rows[0]["Asset_ADD"];
                iPODR["Asset_Modify"] = LOD_DT.Rows[0]["Asset_Modify"];
                iPODR["Asset_Del"] = LOD_DT.Rows[0]["Asset_Del"];
                iPODR["Asset_Query"] = LOD_DT.Rows[0]["Asset_Query"];
                iPODR["Asset_Other"] = LOD_DT.Rows[0]["Asset_Other"];
                iPODR["Asset_CheckData"] = LOD_DT.Rows[0]["Asset_CheckData"];
                iPODR["Del_Flag"] = LOD_DT.Rows[0]["Del_Flag"];
                iPO.MYDS.SLS_Asset_LOGIN.Rows.Add(iPODR);
                iPO.MYDS.SLS_Asset_LOGIN.AcceptChanges();
                iPO.Service_ENV = GETServerName;       //server
                iPO.UID = UID;          //使用者ID
                this.Hide();
                iPO.ShowDialog(this);
                this.Close();
            }
            else
            {
                MessageBox.Show("您沒有權限登入!!\n請找資訊部門協助", this.Text);
            }
            
        }
    }
}
