using System;
using System.Data.SqlClient;

namespace QUANLYDAILI.Utils
{
    public class DatabaseConnector
    {
        // Đổi = connection string của mỗi người
        //An
        //private readonly string cntString = @"Data Source=MSI\AN;Initial Catalog=QUANLYDAILI;Integrated Security=True";
        //Khoa
        private readonly string cntString = @"Data Source=KHOAPC;Initial Catalog=DaiLy;Integrated Security=True";
        public SqlConnection sqlCon = null;

        public void OpenConnection()
        {
            try
            {
                if (sqlCon == null)
                {
                    sqlCon = new SqlConnection(cntString);
                }
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                    //MessageBox.Show("Connect to database success");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (sqlCon != null && sqlCon.State == System.Data.ConnectionState.Open)
                {
                    sqlCon.Close();
                    //MessageBox.Show("Disconnect to database success");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
    }
}
