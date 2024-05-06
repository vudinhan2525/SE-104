using System;
using System.Data.SqlClient;

namespace QUANLYDAILI.Utils
{
    public class DatabaseConnector
    {
        // Đổi = connection string của mỗi người
        private readonly string cntString = @"Data Source=KHOAPC;Initial Catalog=DaiLy;Integrated Security=True; TrustServerCertificate=True";
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
