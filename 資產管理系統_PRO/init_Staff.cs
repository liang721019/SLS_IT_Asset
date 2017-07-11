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
    public partial class init_Staff : Form
    {
        Asset_init_function fun = new Asset_init_function();        
        init_PRO iPO = null;
        public init_Staff(init_PRO YY)
        {
            InitializeComponent();            
            iPO = YY;
        }

        private void Staff_Query_fun()
        {
            fun.Query_DB = @"SELECT [EMP_ID]			AS 員工編號
	                               ,[EMP_Name]		AS 員工姓名
                              FROM [dbo].[SLS_AssetEmployees]
                              where [EMP_ID] like '" + tb_EmpName.Text.Trim() + 
                              @"%' or [EMP_Name] like '" + tb_EmpName.Text.Trim() +
                              "%' order by 1,2" ;
            fun.xxx_DB(fun.Query_DB, this.dataGridView1);

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)  //datagridview1資料點二下要做的事情
        {
            if(e.RowIndex >= 0)
            {
                //iPO.dataGridView3.Columns.Clear();
                string UID1 = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

                fun.Query_DB = @"SELECT [EMP_ID]			AS 員工編號
            	                    ,[EMP_Name]		AS 員工姓名                                    
            	                    ,[Dept_No]		AS 部門代碼
                                    ,[Dept]			AS 部門
                                    FROM [dbo].[SLS_AssetEmployees]
                                    where [EMP_ID] = '" + UID1 + "'";
                fun.ProductDB_ds(fun.Query_DB);
                iPO.tb_OwnerID.Text = fun.ds_index.Tables[0].Rows[0]["員工編號"].ToString();
                iPO.tb_Owner_NM.Text = fun.ds_index.Tables[0].Rows[0]["員工姓名"].ToString(); 
                iPO.tb_OwnerDept_Lv1.Text = fun.ds_index.Tables[0].Rows[0]["部門"].ToString();                

                this.Close();

            }
        }

        private void Staff_Query_Click(object sender, EventArgs e)    //查詢
        {
            Staff_Query_fun();
        }

        private void tb_EmpName_KeyDown(object sender, KeyEventArgs e)      //員工姓名按Enter要處理的事
        {
            if(e.KeyCode ==Keys.Enter)
            {
                Staff_Query_fun();
            }
            


        }
    }
}
