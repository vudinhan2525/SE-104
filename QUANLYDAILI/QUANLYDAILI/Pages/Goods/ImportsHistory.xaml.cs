using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using QUANLYDAILI.Utils;

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for ImportsHistory.xaml
    /// </summary>
    /// 
    public partial class ImportsHistory : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();

        public ImportsHistory()
        {
            InitializeComponent();
            LoadImportData();



        }
        private void ImportsGoodsBtn_Click(object sender, RoutedEventArgs e)
        {
         
            ImportFrame.Content = new GoodsImport();
            SendImportedDataToGoods();
        }
     
        private void LoadImportData()
        {
            dbConnector.OpenConnection();
            string query = "SELECT * FROM PhieuNhap";
            using (SqlCommand command = new SqlCommand(query, dbConnector.sqlCon))
            {
                // Sử dụng SqlDataReader để đọc dữ liệu từ cơ sở dữ liệu
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Tạo một DataTable để chứa dữ liệu từ cơ sở dữ liệu
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    // Gán nguồn dữ liệu DataTable cho DataGrid của bạn
                    GoodsDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            dbConnector.CloseConnection();
        }
        private void SendImportedDataToGoods()
        {
            // Lấy MainFrame từ MainWindow
            Frame mainFrame = ((MainWindow)Application.Current.MainWindow).GetMainFrame();

            // Tìm trang GoodsPage trong MainFrame
            if (mainFrame != null)
            {
                if (mainFrame.Content is GoodsPage goodsPage)
                {
                    // Nếu tìm thấy trang GoodsPage, gọi phương thức cập nhật dữ liệu
                    DataTable importedData = ((DataView)GoodsDataGrid.ItemsSource).ToTable();
                    goodsPage.UpdateGoodsData(importedData);
                }
            }
        }
    }
}
