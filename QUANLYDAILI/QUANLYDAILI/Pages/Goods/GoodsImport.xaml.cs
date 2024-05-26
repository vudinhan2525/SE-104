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
using QUANLYDAILI.Class;
using System.Collections.Specialized;

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

        // Phương thức tính toán tổng tiền từ dữ liệu trong YourDataItems
        private void CalculateTotal()
        {
            double totalAmount = YourDataItems.Sum(item => (double)item.ThanhTien);
            TotalAmountTextBox.Text = totalAmount.ToString();
        }

        // Sự kiện xảy ra khi dữ liệu trong YourDataItems thay đổi
        private void YourDataItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
        public GoodsImport()
        {
            InitializeComponent();
            GoodImportedDataGrid.ItemsSource = YourDataItems;

            //Thay đổi thành tiền
            GoodImportedDataGrid.CellEditEnding += GoodImportedDataGrid_CellEditEnding;

            // Đăng ký sự kiện CollectionChanged để theo dõi sự thay đổi trong YourDataItems
            YourDataItems.CollectionChanged += YourDataItems_CollectionChanged;

            NgayLapPhieuDatePicker.SelectedDate = DateTime.Now;
        }
        private void GoodImportedDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Tính toán lại cột "ThanhTien" khi chỉnh sửa dữ liệu trong cột "SoLuong" hoặc "DonGia"
            var item = (YourDataModel)e.Row.Item;
            item.CalculateTotal();

            CalculateTotal();
        }




        private void SaveDataToDatabase(string soPhieu, DateTime ngayLapPhieu)
        {
            dbConnector.OpenConnection();
            try
            {
                foreach (YourDataModel item in YourDataItems)
                {
                    decimal thanhtien = item.DonGia * item.SoLuong;
                    string query = "INSERT INTO PhieuNhap (MaPhieuNhap, MaMatHang, DonGia,  NgayLapPhieu, SoLuong, ThanhTien)" +
                                   "VALUES (@MaPhieuNhap,@MaMatHang, @DonGia,  @NgayLapPhieu, @SoLuong, @ThanhTien)";
                    SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                    command.Parameters.AddWithValue("@MaPhieuNhap", soPhieu);
                    command.Parameters.AddWithValue("@NgayLapPhieu", ngayLapPhieu);
                    command.Parameters.AddWithValue("@MaMatHang", item.MaMatHang); 
                    command.Parameters.AddWithValue("@DonGia", item.DonGia);
                    command.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    command.Parameters.AddWithValue("@ThanhTien", thanhtien);
                    int row = command.ExecuteNonQuery();
                   
                }
               
                    MessageBox.Show("Thêm phiếu xuất thành công");
                
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
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbConnector.OpenConnection();
                foreach (YourDataModel item in YourDataItems)
                {
                    // Retrieve the price (DonGia) from the database for each item
                    string queryDonGia = "SELECT Gia FROM MatHang WHERE MaMatHang = @MaMatHang";
                    SqlCommand commandDonGia = new SqlCommand(queryDonGia, dbConnector.sqlCon);
                    commandDonGia.Parameters.AddWithValue("@MaMatHang", item.MaMatHang);
                    decimal donGia = (decimal)commandDonGia.ExecuteScalar();

                    // Set the DonGia property of the item
                    item.DonGia = donGia;

                    // Calculate the total price (ThanhTien) for the item
                    decimal thanhTien = item.DonGia * item.SoLuong;
                    item.ThanhTien = thanhTien;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            CalculateTotal();


        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        protected virtual void OnDataSaved()
        {
            // Kiểm tra xem có người đăng ký sự kiện hay không
            DataSavedEvent?.Invoke(this, EventArgs.Empty);
        }


    }
    
}
