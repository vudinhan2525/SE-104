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
    }

}
