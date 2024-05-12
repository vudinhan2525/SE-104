using QUANLYDAILI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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
using System.Collections.ObjectModel;
using System.Security.Policy;

namespace QUANLYDAILI.Pages
{


    /// <summary>
    /// Interaction logic for GoodsImport.xaml
    /// </summary>
    public partial class GoodsImport : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        public ObservableCollection<YourDataModel> YourDataItems { get; set; } = new ObservableCollection<YourDataModel>();
        public event EventHandler DataSavedEvent;
        public GoodsImport()
        {
            InitializeComponent();
            GoodImportedDataGrid.ItemsSource = YourDataItems;

        }
      
     
       
        private void SaveDataToDatabase(string soPhieu, DateTime ngayLapPhieu)
        {
            dbConnector.OpenConnection();
            try
            {
                foreach (YourDataModel item in YourDataItems)
            {
                string query = "INSERT INTO PhieuNhap (MaPhieuNhap, MaMatHang, DonGia, DonViTinh, NgayLapPhieu, SoLuong, ThanhTien)" +
                               "VALUES (@MaPhieuNhap,@MaMatHang, @DonGia, @DonViTinh, @NgayLapPhieu, @SoLuong, @ThanhTien)";
                SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                command.Parameters.AddWithValue("@MaPhieuNhap", soPhieu);
                command.Parameters.AddWithValue("@NgayLapPhieu", ngayLapPhieu);
                command.Parameters.AddWithValue("@MaMatHang", item.MaMatHang);
                command.Parameters.AddWithValue("@DonGia", item.DonGia);
                command.Parameters.AddWithValue("@DonViTinh", item.DonViTinh);
                command.Parameters.AddWithValue("@SoLuong", item.SoLuong);
              
                command.Parameters.AddWithValue("@ThanhTien", item.ThanhTien);
                command.ExecuteNonQuery();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu dữ liệu: " + ex.Message);
            }
            
            finally
            {
                dbConnector.CloseConnection();
            }
            
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string soPhieu = SoPhieuTextBox.Text; // Giả sử SoPhieuTextBox là TextBox chứa số phiếu
            DateTime ngayLapPhieu = NgayLapPhieuDatePicker.SelectedDate ?? DateTime.Now;
            SaveDataToDatabase(soPhieu, ngayLapPhieu);
            OnDataSaved();
        }
        protected virtual void OnDataSaved()
        {
            // Kiểm tra xem có người đăng ký sự kiện hay không
            DataSavedEvent?.Invoke(this, EventArgs.Empty);
        }

       

    }
}
