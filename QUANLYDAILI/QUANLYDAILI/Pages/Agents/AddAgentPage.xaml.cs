using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
            for(int i = 0;i < GlobalVariables.numberOfTypeAgent; i++)
            {
                string s = "Loại " + (i + 1);
                TypeInp.Items.Add(s);
            }
            TypeInp.SelectedIndex = 0;
            for(int i = 0; i < GlobalVariables.Districts.Count; i++)
            {
                DistrictComboBox.Items.Add(GlobalVariables.Districts[i]);
            }
            DebtInp.Text = "0";
            DistrictComboBox.SelectedIndex = 0;
            if (a.MaDaiLy != 0)
            {
                agent = a;
                mainTitle.Text = "Cập nhật đại lý";
                isModified = true;
                NameInp.Text = a.TenDaiLy;
                SDTInp.Text = a.SoDienThoai;
                DebtInp.IsReadOnly = false;
                for (int j = 0;j < GlobalVariables.Districts.Count; j++)
                {
                    if (GlobalVariables.Districts[j] == a.Quan)
                    {
                        DistrictComboBox.SelectedIndex=j;
                        break;
                    }
                }
                CityInp.Text = a.DiaChi;
                AvatarInp.Text = a.Avatar;
                DebtInp.Text = a.KhoanNo.ToString();
                EmailInp.Text = a.Email;
                DeleteStoreBtn.Visibility = Visibility.Visible;
                GetMoneyStoreBtn.Visibility = Visibility.Visible;
                ExportGoodsBtn.Visibility = Visibility.Visible;
                if (a.Loai == 2)
                {
                    TypeInp.SelectedIndex = 1;
                }
                addStoreText.Text = "Lưu đại lý";
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
            if (NameInp.Text.Trim() == "" || TypeInp.Text.Trim() == "" || SDTInp.Text.Trim() == "" || DistrictComboBox.SelectedItem.ToString() == "" || CityInp.Text.Trim() == "" || AvatarInp.Text.Trim() == "" || DebtInp.Text.Trim() == "" || EmailInp.Text.Trim() == "")
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
            
            try
            {
                dbConnector.OpenConnection();
                string fQuery = "SELECT COUNT(*) as Soluong FROM DaiLy WHERE Quan = N'" + DistrictComboBox.SelectedItem.ToString() + "'";
                SqlCommand sql = new SqlCommand();
                sql.CommandText = fQuery;
                sql.Connection = dbConnector.sqlCon;
                SqlDataReader reader2 = sql.ExecuteReader();
                while (reader2.Read())
                {
                    int c = reader2.GetInt32(reader2.GetOrdinal("Soluong"));
                    if (c >= GlobalVariables.maxAgentPerDistrict)
                    {
                        MessageBox.Show("Quận này đã đủ " + GlobalVariables.maxAgentPerDistrict + " đại lý.");
                        return;
                    }
                }
                reader2.Close();
            }
            catch
            {

            }
            finally
            {
                dbConnector.CloseConnection();
            }
            
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
                    sqlCmd.Parameters.AddWithValue("@Quan", DistrictComboBox.SelectedItem.ToString());
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
                    sqlCmd.Parameters.AddWithValue("@Quan", DistrictComboBox.SelectedItem.ToString());
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
            if (GlobalVariables.typeAgent[s.Trim()] < debt)
            {
                MessageBox.Show("Đại lý loại " + typeNum + " chỉ được nợ tối đa "+ GlobalVariables.typeAgent[s.Trim()] + " đ.");
                DebtInp.Text = "";
                return false;
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
        private void ExportGoodsBtn_Click(Object sender, RoutedEventArgs e)
        {
            _menuFrame.Content = new ExportForm(_menuFrame, agent);
        }
       
        private void DeleteStoreBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            DeleteStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c53030"));
        }

        private void DeleteStoreBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e53e3e"));
        }

        private void GetMoneyStoreBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            GetMoneyStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1dbf69"));
        }

        private void GetMoneyStoreBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            GetMoneyStoreBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#21d375"));
        }

        private void GetMoneyStoreBtn_Click(object sender, RoutedEventArgs e)
        {
            _menuFrame.Content = new TakeMoneyForm(_menuFrame, agent);
        }
        private void ExportGoodsBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ExportGoodsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbc432"));
        }

        private void ExportGoodsBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ExportGoodsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebd234"));
        }

    }
}
