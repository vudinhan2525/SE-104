using QUANLYDAILI.Class;
using QUANLYDAILI.Pages.Bills;
using QUANLYDAILI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QUANLYDAILI.Pages.Agents.Export
{
    /// <summary>
    /// Interaction logic for ExportHistory.xaml
    /// </summary>
    public partial class ExportHistory : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private List<ExportData> _exports = new List<ExportData>();
        private List<Agent> agents = new List<Agent>();
        public ExportHistory()
        {
            InitializeComponent();
            getExportList();
        }
        private void getExportList()
        {
            dbConnector.OpenConnection();
            string query = "SELECT * FROM PhieuXuat";
            using (SqlCommand command = new SqlCommand(query, dbConnector.sqlCon))
            {
                // Sử dụng SqlDataReader để đọc dữ liệu từ cơ sở dữ liệu
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Tạo một DataTable để chứa dữ liệu từ cơ sở dữ liệu
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    ExportHistoryDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            dbConnector.CloseConnection();

        }
        private void SearchBtn_Click(object sender, EventArgs e)
        {

            string query = $"SELECT * FROM DaiLy WHERE MaDaiLy LIKE @query";
            try
            {
                dbConnector.OpenConnection();

                SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                command.Parameters.AddWithValue("@query", "%" + SearchExp.Text.Trim() + "%");
                SqlDataReader reader = command.ExecuteReader();

                agents = new List<Agent>();
                while (reader.Read())
                {
                    Agent agent = new Agent(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetByte(6),
                        reader.GetDateTime(7).ToString("yyyy-MM-dd"),
                        reader.GetDecimal(8),
                        reader.GetString(9)
                    );
                    agents.Add(agent);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dbConnector.CloseConnection();
            }


        }
    }

}
