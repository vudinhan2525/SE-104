using System;
using System.Collections.Generic;
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
using QUANLYDAILI.Utils;

namespace QUANLYDAILI.Pages.Agents
{
    /// <summary>
    /// Interaction logic for TakeMoneyForm.xaml
    /// </summary>
    public partial class TakeMoneyForm : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private Frame _menuFrame;
        private Agent agent;
        public TakeMoneyForm(Frame menuFrame, Agent a)
        {
            InitializeComponent();
            if (a.MaDaiLy != 0)
            {
                agent = a;
                NameInp.Text = agent.TenDaiLy;
                SDTInp.Text = agent.SoDienThoai;
                AddressInp.Text = agent.Quan + "," + agent.DiaChi;
                EmailInp.Text = agent.Email;
            }

        }

        private void CancleBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TakeMoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TakeMoneyInp.Text.Trim() == "") return;
            decimal moneyDebt = agent.KhoanNo;
            decimal moneyTaken = Decimal.Parse(TakeMoneyInp.Text.Trim());
            decimal debtLeft = moneyDebt - moneyTaken;
            string query = "UPDATE DaiLy SET KhoanNo=@KhoanNo WHERE MaDaiLy=@MaDaiLy";
            try
            {
                dbConnector.OpenConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = query;
                sqlCmd.Connection = dbConnector.sqlCon;
                sqlCmd.Parameters.AddWithValue("@KhoanNo", debtLeft);
                sqlCmd.Parameters.AddWithValue("@MaDaiLy", agent.MaDaiLy);
                int rowsAffected = sqlCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {

                }
                else
                {
                    MessageBox.Show("Cập nhật đại lý thất bại.");
                }
                sqlCmd = new SqlCommand();
                string query2 = "INSERT INTO PhieuThu(MaDaiLy, TenDaiLy, Avatar, DiaChi, SoDienThoai, Email,SoTienThu,NgayThu) VALUES (@MaDaiLy, @TenDaiLy, @Avatar, @DiaChi, @SoDienThoai, @Email, @SoTienThu, @NgayThu)";
                sqlCmd.CommandText = query2;
                sqlCmd.Connection = dbConnector.sqlCon;
                sqlCmd.Parameters.AddWithValue("@MaDaiLy", agent.MaDaiLy);
                sqlCmd.Parameters.AddWithValue("@TenDaiLy", agent.TenDaiLy);
                sqlCmd.Parameters.AddWithValue("@Avatar", agent.Avatar);
                sqlCmd.Parameters.AddWithValue("@DiaChi", AddressInp.Text);
                sqlCmd.Parameters.AddWithValue("@SoDienThoai", agent.SoDienThoai);
                sqlCmd.Parameters.AddWithValue("@Email", agent.Email);
                sqlCmd.Parameters.AddWithValue("@SoTienThu", moneyTaken);
                sqlCmd.Parameters.AddWithValue("@NgayThu", DateTime.Today);
                int rowsAffected2 = sqlCmd.ExecuteNonQuery();
                if (rowsAffected2 > 0)
                {
                    MessageBox.Show("Thêm phiếu thu thành công.");
                }
                else
                {
                    MessageBox.Show("Thêm phiếu thu thất bại.");
                }

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
