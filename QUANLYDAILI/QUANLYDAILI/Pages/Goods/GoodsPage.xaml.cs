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
using QUANLYDAILI.Class;
using System.Collections.ObjectModel;
using System.Reflection.PortableExecutable;

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        public ObservableCollection<Goods> GoodsCollection { get; set; }
        public GoodsPage()
        {
            InitializeComponent();
            // Khởi tạo ObservableCollection
            GoodsCollection = new ObservableCollection<Goods>();

            // Thiết lập nguồn dữ liệu cho DataGrid
            GoodsDataGrid.ItemsSource = GoodsCollection;
            LoadGoodsData();
        }
        public void UpdateGoodsData(DataTable importedData)
        {
            // Cập nhật dữ liệu từ trang Import vào cơ sở dữ liệu của trang Goods
            // Sau đó, tải lại dữ liệu trên trang Goods
        }
        private void LoadGoodsData()
        {
            try
            {

                dbConnector.OpenConnection();

                // Tạo câu truy vấn SQL để lấy dữ liệu từ bảng hoặc chế độ xem trong cơ sở dữ liệu của bạn
                string query = "SELECT * FROM MatHang";

                // Thực hiện truy vấn SQL để lấy dữ liệu từ cơ sở dữ liệu
                using (SqlCommand command = new SqlCommand(query, dbConnector.sqlCon))
                {
                    // Sử dụng SqlDataReader để đọc dữ liệu từ cơ sở dữ liệu
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa dữ liệu cũ trong GoodsCollection
                        //GoodsCollection.Clear();

                        // Đọc dữ liệu từ SqlDataReader và thêm vào GoodsCollection
                        while (reader.Read())
                        {
                            Goods newItem = new Goods
                            {
                                MaMatHang = reader["MaMatHang"].ToString(),
                                TenMatHang = reader["TenMatHang"].ToString(),
                                Gia = Convert.ToInt32(reader["Gia"]),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                DonViTinh = reader["DonViTinh"].ToString(),
                                IsChecked = false
                            };
                            GoodsCollection.Add(newItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi khi tai du lieu hang hoa: " + ex.Message);
            }
            finally
            {
                if (dbConnector.sqlCon.State == ConnectionState.Open)
                {
                    dbConnector.CloseConnection();
                }
            }

        }
        private void AddGoods_Click(object sender, RoutedEventArgs e)
        {
            int gia, soLuong;

            // Kiểm tra xem dữ liệu nhập vào từ TextBox có phải là số nguyên hay không
            if (!int.TryParse(DonGiaTxt.Text, out gia) || !int.TryParse(SoLuongTxt.Text, out soLuong))
            {
                // Nếu dữ liệu không phải là số nguyên, hiển thị một thông báo lỗi hoặc thực hiện xử lý phù hợp
                MessageBox.Show("Giá và số lượng phải là số nguyên.");
                return;
            }
            // Tạo một đối tượng mới từ dữ liệu được nhập từ các TextBox và ComboBox
            Goods newItem = new Goods
            {
                MaMatHang = MaMatHangTxt.Text,
                TenMatHang = TenMatHangTxt.Text,
                Gia = gia,
                SoLuong = soLuong,
                DonViTinh = ((ComboBoxItem)DVTTxt.SelectedItem).Content.ToString() // Lấy giá trị từ ComboBox
            };




            // Làm mới DataGrid để hiển thị các thay đổi
            GoodsDataGrid.Items.Refresh();

            // Thêm vào cơ sở dữ liệu
            AddGoodsToDatabase(newItem);
        }
        private void AddGoodsToDatabase(Goods newItem)
        {
            try
            {
                dbConnector.OpenConnection();

                string query = "INSERT INTO MatHang (MaMatHang, TenMatHang, Gia, SoLuong, DonViTinh) " +
                               "VALUES (@MaMatHang, @TenMatHang, @Gia, @SoLuong, @DonViTinh)";
                using (SqlCommand command = new SqlCommand(query, dbConnector.sqlCon))
                {
                    command.Parameters.AddWithValue("@MaMatHang", newItem.MaMatHang);
                    command.Parameters.AddWithValue("@TenMatHang", newItem.TenMatHang);
                    command.Parameters.AddWithValue("@Gia", newItem.Gia);
                    command.Parameters.AddWithValue("@SoLuong", newItem.SoLuong);
                    command.Parameters.AddWithValue("@DonViTinh", newItem.DonViTinh);
                    command.ExecuteNonQuery();
                }

                // Sau khi thêm thành công vào cơ sở dữ liệu, thêm vào GoodsCollection
                GoodsCollection.Add(newItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hóa vào cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                if (dbConnector.sqlCon.State == ConnectionState.Open)
                {
                    dbConnector.CloseConnection();
                }
            }
        }
        private void DeleteSelectedItems()
        {
            try
            {
                // Mở kết nối đến cơ sở dữ liệu
                dbConnector.OpenConnection();

                // Tạo danh sách tạm thời để chứa các mục cần xóa
                List<Goods> itemsToRemove = new List<Goods>();

                // Lặp qua mỗi hàng trong GoodsCollection
                foreach (Goods item in GoodsCollection)
                {
                    // Kiểm tra xem CheckBox tương ứng của hàng có được chọn không
                    if (item.IsChecked)
                    {
                        // Lấy thông tin cần thiết để xóa dòng từ cơ sở dữ liệu
                        string maMatHang = item.MaMatHang;

                        // Thực hiện truy vấn SQL hoặc gọi phương thức xóa từ cơ sở dữ liệu
                        string query = "DELETE FROM MatHang WHERE MaMatHang = @MaMatHang";
                        using (SqlCommand command = new SqlCommand(query, dbConnector.sqlCon))
                        {
                            command.Parameters.AddWithValue("@MaMatHang", maMatHang);
                            command.ExecuteNonQuery();
                        }

                        // Thêm vào danh sách các mục cần xóa
                        itemsToRemove.Add(item);
                    }
                }

                // Xóa các mục đã được chọn từ GoodsCollection
                foreach (var itemToRemove in itemsToRemove)
                {
                    GoodsCollection.Remove(itemToRemove);
                }
                GoodsDataGrid.Items.Refresh();

                // Thông báo xóa thành công
                MessageBox.Show("Đã xóa các mặt hàng được chọn thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa hàng loạt: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối đến cơ sở dữ liệu
                if (dbConnector.sqlCon.State == ConnectionState.Open)
                {
                    dbConnector.CloseConnection();
                }
            }
        }

        // Sự kiện được gọi khi người dùng nhấn nút xóa hàng loạt
        private void DeleteGoodsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Gọi phương thức để xóa các mục được chọn
            DeleteSelectedItems();

        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
