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
using QUANLYDAILI.Pages.Agents;
using QUANLYDAILI.Utils;

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for AddAgentPage.xaml
    /// </summary>
    public partial class AddAgentPage : Page
    {
        private Frame _menuFrame;
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private bool isModified = false; 
        private Agent agent;
        public AddAgentPage(Frame menuFrame,Agent a)
        {
            InitializeComponent();
            _menuFrame = menuFrame;
            TypeInp.Items.Add("Loại 1");
            TypeInp.Items.Add("Loại 2");
            TypeInp.SelectedIndex = 0;
            if (a.MaDaiLy != 0)
            {
                agent = a;
                isModified = true;
                NameInp.Text = a.TenDaiLy;
                SDTInp.Text = a.SoDienThoai;
                DistrictInp.Text = a.Quan;
                CityInp.Text = a.DiaChi;
                AvatarInp.Text = a.Avatar;
                DebtInp.Text = a.KhoanNo.ToString();
                EmailInp.Text = a.Email;
                DeleteStoreBtn.Visibility = Visibility.Visible;
                if(a.Loai == 2)
                {
                    TypeInp.SelectedIndex = 1;
                }
                addStoreText.Text = "Save";
            }

        }

        private void BackBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            BackBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e7eb"));
        }

        private void BackBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            BackBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8FA"));
        }

        private void BackBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _menuFrame.Content = new AgentPage(_menuFrame);
        }

        private void AddStoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameInp.Text.Trim() == "" || TypeInp.Text.Trim() == "" || SDTInp.Text.Trim() == "" || DistrictInp.Text.Trim() == "" || CityInp.Text.Trim() == "" || AvatarInp.Text.Trim() == "" || DebtInp.Text.Trim() == "" || EmailInp.Text.Trim() == "")
            {
                return;
            }
            if (checkType(TypeInp.Text) == false)
            {
                return;
            }
            string[] word = TypeInp.Text.Trim().Split(' ');
            int typeNum = Int32.Parse(word[1]);
            string[] num = DebtInp.Text.Trim().Split('.');
            int debt = Int32.Parse(num[0]);

            if (!isModified)
            {
                string query = "INSERT INTO Daily(TenDaiLy, SoDienThoai, Quan, Avatar, DiaChi, Loai, NgayTiepNhan, KhoanNo, Email) VALUES (@TenDaiLy, @SoDienThoai, @Quan, @Avatar, @DiaChi, @Loai, @NgayTiepNhan, @KhoanNo, @Email)";
                try
                {
                    dbConnector.OpenConnection();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Connection = dbConnector.sqlCon;
                    sqlCmd.Parameters.AddWithValue("@TenDaiLy", NameInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@SoDienThoai", SDTInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Quan", DistrictInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Avatar", AvatarInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@DiaChi", CityInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Loai", typeNum); 
                    sqlCmd.Parameters.AddWithValue("@NgayTiepNhan", DateTime.Today);
                    sqlCmd.Parameters.AddWithValue("@KhoanNo", debt);
                    sqlCmd.Parameters.AddWithValue("@Email", EmailInp.Text.Trim());
                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm đại lý thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Thêm đại lý thất bại.");
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
            else
            {
                // Update in DaiLy Table with agent.MaDaiLy as primary key of this row 
                string query = "UPDATE Daily SET TenDaiLy=@TenDaiLy, SoDienThoai=@SoDienThoai, Quan=@Quan, Avatar=@Avatar, DiaChi=@DiaChi, Loai=@Loai, KhoanNo=@KhoanNo, Email=@Email WHERE MaDaiLy=@MaDaiLy";
                try
                {
                    dbConnector.OpenConnection();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Connection = dbConnector.sqlCon;
                    sqlCmd.Parameters.AddWithValue("@TenDaiLy", NameInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@SoDienThoai", SDTInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Quan", DistrictInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Avatar", AvatarInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@DiaChi", CityInp.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Loai", typeNum);
                    sqlCmd.Parameters.AddWithValue("@KhoanNo", debt);
                    sqlCmd.Parameters.AddWithValue("@Email", EmailInp.Text.Trim());
                    // Assuming MaDaiLy is a property in your class holding the primary key value
                    sqlCmd.Parameters.AddWithValue("@MaDaiLy", agent.MaDaiLy);
                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật đại lý thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật đại lý thất bại.");
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

        private void TypeInp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedType = comboBox.SelectedItem.ToString();
            checkType(selectedType);
        }
        private bool checkType(string s)
        {

            string[] word = s.Trim().Split(' ');
            if (word[0] == "" || DebtInp.Text.Trim() == "") return false;
            int typeNum = Convert.ToInt32(word[1]);
            string[] num = DebtInp.Text.Trim().Split('.');
            int debt = Convert.ToInt32(num[0]);
            if (typeNum == 1)
            {
                if (debt > 10000000)
                {
                    MessageBox.Show("Đại lý loại 1 chỉ được nợ tối đa 10.000.000đ.");
                    DebtInp.Text = "";
                    return false;
                }
            }
            if (typeNum == 2)
            {

                if (debt > 5000000)
                {
                    MessageBox.Show("Đại lý loại 2 chỉ được nợ tối đa 5.000.000đ.");
                    DebtInp.Text = "";
                    return false;
                }
            }            if (typeNum == 1)
            {
                if (debt > 10000000)
                {
                    MessageBox.Show("Đại lý loại 1 chỉ được nợ tối đa 10.000.000đ.");
                    DebtInp.Text = "";
                    return false;
                }
            }
            if (typeNum == 2)
            {

                if (debt > 5000000)
                {
                    MessageBox.Show("Đại lý loại 2 chỉ được nợ tối đa 5.000.000đ.");
                    DebtInp.Text = "";
                    return false;
                }
            }
            return true;
        }

        private void DeleteStoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isModified)
            {
                dbConnector.OpenConnection();
                string query = "DELETE FROM DaiLy WHERE MaDaiLy = @MaDaiLy";
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = query;
                sqlCmd.Connection = dbConnector.sqlCon;
                sqlCmd.Parameters.AddWithValue("@MaDaiLy", agent.MaDaiLy);
                try
                {
                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Store deleted successfully.");
                        _menuFrame.Content = new AgentPage(_menuFrame);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete store.");
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

        private void DeleteStoreBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            DeleteStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c53030"));
        }

        private void DeleteStoreBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e53e3e"));
        }
    }
}
