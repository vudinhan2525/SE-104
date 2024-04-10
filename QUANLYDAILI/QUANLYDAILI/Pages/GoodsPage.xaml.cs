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

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage : Page
    {

        public GoodsPage()
        {
            InitializeComponent();
            LoadGoodsData();
        }
        private void LoadGoodsData()
        {
            string connectionString = @"Data Source=DESKTOP-PEGO4F4;Initial Catalog=DaiLy;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL để lấy dữ liệu từ bảng hoặc chế độ xem trong cơ sở dữ liệu của bạn
                string query = "SELECT * FROM MatHang";

                // Thực hiện truy vấn SQL để lấy dữ liệu từ cơ sở dữ liệu
                using (SqlCommand command = new SqlCommand(query, connection))
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
            }
        }
    }
}
