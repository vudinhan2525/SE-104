using QUANLYDAILI.Class;
using QUANLYDAILI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace QUANLYDAILI.Pages.Agents
{
    /// <summary>
    /// Interaction logic for ExportForm.xaml
    /// </summary>
    /// 
    public partial class ExportForm : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private Frame _menuFrame;
        private Agent agent;
        public ObservableCollection<ExportData> YourDataItems { get; set; } = new ObservableCollection<ExportData>();
        public event EventHandler DataSavedEvent;
        public ExportForm(Frame menuFrame, Agent a)
        {
            InitializeComponent();
            
            if (a.MaDaiLy != 0)
            {

                agent = a;
                NameInp.Text = agent.TenDaiLy;
            }
            this.DataContext = this;

            // Initialize DataGrid ItemsSource
            ExportDataGrid.ItemsSource = YourDataItems;

            //Thay đổi thành tiền
            ExportDataGrid.CellEditEnding += ExportDataGrid_CellEditEnding;

           
        }
        private void ExportDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Tính toán lại cột "ThanhTien" khi chỉnh sửa dữ liệu trong cột "SoLuong" hoặc "DonGia"
            var item = (ExportData)e.Row.Item;
            item.CalculateTotal();
            // Recalculate the total after editing
            CalculateTotal();
        }
        private void CalculateTotal()
        {
            double total = YourDataItems.Sum(item => (double)item.ThanhTien);
            TotalInp.Text = total.ToString();
            UpdateRemain();
        }
        private void UpdateRemain()
        {
            if (double.TryParse(TotalInp.Text, out double total) && double.TryParse(PaymentInp.Text, out double payment))
            {
                double remain = total - payment;
                RemainInp.Text = remain.ToString();
            }
        }
        private void PaymentInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRemain();
        }

        private void SaveDataToDatabase(DateTime ngayLapPhieu)
        {
            dbConnector.OpenConnection();
            try
            {
                foreach (ExportData item in YourDataItems)
                {
                    decimal thanhtien = item.DonGia * item.SoLuong;
                    string query = "INSERT INTO PhieuXuat (MaMatHang, MaDaiLy, DonViTinh, NgayLapPhieu, SoLuong, DonGia, ThanhTien)" +
                                   "VALUES (@MaMatHang,@MaDaiLy,  @DonViTinh, @NgayLapPhieu, @SoLuong, @DonGia, @ThanhTien)";
                    SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                  
                    
                    command.Parameters.AddWithValue("@MaMatHang", item.MaMatHang);
                    command.Parameters.AddWithValue("@NgayLapPhieu", ngayLapPhieu);
                    command.Parameters.AddWithValue("@MaDaiLy", agent.MaDaiLy);
                    command.Parameters.AddWithValue("@DonViTinh", item.DonViTinh);
                    command.Parameters.AddWithValue("@DonGia", item.DonGia);
                    command.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    command.Parameters.AddWithValue("@ThanhTien", thanhtien);
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
        private void AddExportBtn_Click(object sender, EventArgs e)
        {
            
            DateTime ngayLapPhieu = DateInp.SelectedDate ?? DateTime.Now;
            SaveDataToDatabase(ngayLapPhieu);
            OnDataSaved();
        }
    
        private void UpdExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnector.OpenConnection();
                foreach (ExportData item in YourDataItems)
                {
                    // Retrieve the price (DonGia) and unit (DonViTinh) from the database for each item
                    string query = "SELECT Gia, DonViTinh, SoLuong FROM MatHang WHERE MaMatHang = @MaMatHang";
                    SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                    command.Parameters.AddWithValue("@MaMatHang", item.MaMatHang);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if(item.SoLuong > reader.GetInt32(reader.GetOrdinal("SoLuong")))
                            {
                                MessageBox.Show("Khong du mat hang de xuat!");
                                item.SoLuong = 0;

                                return;
                            }
                            // Ensure that the data types match the expected types in the database
                            item.DonGia = reader.GetDecimal(reader.GetOrdinal("Gia")); // Retrieve the decimal value from the Gia column
                            item.DonViTinh = reader.GetString(reader.GetOrdinal("DonViTinh")); // Retrieve the string value from the DonViTinh column
                        }
                    }


                    // Calculate the total price (ThanhTien) for the item
                    decimal thanhTien = item.DonGia * item.SoLuong;
                    item.ThanhTien = thanhTien;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                dbConnector.CloseConnection();
            }

            // Recalculate and update the total
            CalculateTotal();
            
        }
        protected virtual void OnDataSaved()
        {
            // Kiểm tra xem có người đăng ký sự kiện hay không
            DataSavedEvent?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
