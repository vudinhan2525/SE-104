using QUANLYDAILI.Utils;
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

namespace QUANLYDAILI.Pages.Statistics
{
    /// <summary>
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        List<DoanhThuItem> doanhThuItems;
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private bool firstLoad { get; set; }
        private decimal sum = 0;
        public StatisticsPage()
        {
            InitializeComponent();
            firstLoad = true;
            for (int i = 1; i <= 12; i++)
            {
                string r = "Tháng " + i;
                MonthCombobox.Items.Add(r);
            }
            MonthCombobox.SelectedIndex = 0;
            for(int i = 2000;i <= 2024; i++)
            {
                YearCombobox.Items.Add(i.ToString());
            }
            YearCombobox.SelectedIndex = 24;


        }
        private void calcMonthly()
        {          
            dbConnector.OpenConnection();
            string fQuery = "SELECT SUM(ThanhTien) as TongTriGia FROM PhieuXuat WHERE YEAR(NgayLapPhieu) = @yearInp AND MONTH(NgayLapPhieu) = @monthInp";
            SqlCommand command2 = new SqlCommand(fQuery, dbConnector.sqlCon);
            command2.Parameters.AddWithValue("@yearInp", YearCombobox.SelectedItem.ToString());
            command2.Parameters.AddWithValue("@monthInp", MonthCombobox.SelectedIndex + 1);
            SqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                if (!reader2.IsDBNull(0))
                {
                    decimal s = reader2.GetDecimal(0);
                    sum = s;
                }
            }
            reader2.Close();
            string query = "SELECT DaiLy.TenDaiLy,COUNT(*) AS SoPhieuXuat,SUM(ThanhTien) AS TongTriGia FROM PhieuXuat INNER JOIN DaiLy ON DaiLy.MaDaiLy = PhieuXuat.MaDaiLy WHERE YEAR(NgayLapPhieu) = @yearInp AND MONTH(NgayLapPhieu) = @monthInp GROUP BY PhieuXuat.MaDaiLy,DaiLy.TenDaiLy";
            SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
            command.Parameters.AddWithValue("@yearInp", YearCombobox.SelectedItem.ToString());
            command.Parameters.AddWithValue("@monthInp",MonthCombobox.SelectedIndex + 1);
            SqlDataReader reader = command.ExecuteReader();
            DoanhThuTable.Items.Clear();
            int i = 1;
            while (reader.Read())
            {
                string s = reader.GetString(reader.GetOrdinal("TenDaiLy"));
                int c = reader.GetInt32(reader.GetOrdinal("SoPhieuXuat"));
                decimal d = reader.GetDecimal(reader.GetOrdinal("TongTriGia"));
                decimal percent = d * 100 / sum;
                DoanhThuItem item = new DoanhThuItem(i, s, c, d, percent);
                i++;
                DoanhThuTable.Items.Add(item);
            }
            reader.Close();
            dbConnector.CloseConnection();
        }
        private void MonthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int month = MonthCombobox.SelectedIndex + 1;
            if (!firstLoad)
            {
                calcMonthly();
            }
            else
            {
                firstLoad = false;
            }
        }
        private void YearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!firstLoad)
            {
                calcMonthly();
            }
            else
            {
                firstLoad = false;
            }
        }
    }
}
