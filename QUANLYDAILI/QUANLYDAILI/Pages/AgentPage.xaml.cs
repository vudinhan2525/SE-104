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

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for AgentPage.xaml
    /// </summary>
    public struct Agent
    {
        public int MaDaiLy;
        public string TenDaiLy;
        public string SoDienThoai;
        public string Quan;
        public string Avatar;
        public string DiaChi;
        public int Loai;
        public string NgayTiepNhan;
        public decimal KhoanNo;
        public string Email;
        public Agent(int maDaiLy, string tenDaiLy, string soDienThoai, string quan, string avatar, string diaChi, int loai, string ngayTiepNhan, decimal khoanNo, string email)
        {
            MaDaiLy = maDaiLy;
            TenDaiLy = tenDaiLy;
            SoDienThoai = soDienThoai;
            Quan = quan;
            Avatar = avatar;
            DiaChi = diaChi;
            Loai = loai;
            NgayTiepNhan = ngayTiepNhan;
            KhoanNo = khoanNo;
            Email = email;
        }
    }

public partial class AgentPage : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        public AgentPage()
        {
            InitializeComponent();
            getAllAgents();
            
            //stPanel.Children.Add(addImage("https://shopcartimg2.blob.core.windows.net/shopcartctn/logo.webp"));
        }
        private Image addImage(string url)
        {
            var image = new Image();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
            return image;
        }
        private void getAllAgents()
        {
            string query = $"SELECT * FROM DaiLy";
            try
            {
                dbConnector.OpenConnection();

                SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                SqlDataReader reader = command.ExecuteReader();

                List<Agent> agents = new List<Agent>();

                while (reader.Read())
                {
                    Agent agent = new Agent(
                        reader.GetInt32(0),       
                        reader.GetString(1),      
                        reader.GetString(2),      
                        reader.GetString(3),  
                        reader.GetString(4),      
                        reader.GetString(5),     
                        reader.GetByte(6),       
                        reader.GetDateTime(7).ToString("yyyy-MM-dd"),    
                        reader.GetDecimal(8),
                        reader.GetString(9) 
                    );
                    agents.Add(agent);
                }
                reader.Close();

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
