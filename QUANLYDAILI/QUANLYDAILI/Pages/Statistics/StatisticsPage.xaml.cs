using QUANLYDAILI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private bool firstLoad { get; set; }
        private bool firstLoad2 { get; set; }
        private decimal sum = 0;
        public StatisticsPage()
        {
            InitializeComponent();
            firstLoad = true;
            firstLoad2 = true;
            for (int i = 1; i <= 12; i++)
            {
                string r = "Tháng " + i;
                MonthCombobox.Items.Add(r);
                Month2Combobox.Items.Add(r);
            }
            MonthCombobox.SelectedIndex = 4;
            Month2Combobox.SelectedIndex = 4;
            for (int i = 2000;i <= 2024; i++)
            {
                YearCombobox.Items.Add(i.ToString());
                Year2Combobox.Items.Add(i.ToString());
            }
            YearCombobox.SelectedIndex = 24;
            Year2Combobox.SelectedIndex = 24;



        }
        private void calcMonthlyDebt()
        {   
            List<int> ids = new List<int>();
            List<decimal> fDebts = new List<decimal>();
            List<decimal> cDebts = new List<decimal>();
            List<string> names = new List<string>();
            dbConnector.OpenConnection();
            string query = "SELECT DISTINCT * FROM DaiLy";
            SqlCommand cmd = new SqlCommand(query,dbConnector.sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("TenDaiLy"));
                int id = reader.GetInt32(reader.GetOrdinal("MaDaiLy"));
                ids.Add(id);
                names.Add(name);
            }
            reader.Close();
            for(int i = 0;i < ids.Count;i++)
            {
                int MONTH = Month2Combobox.SelectedIndex + 1;
                string query1 = "SELECT SUM(ThanhTien) as TongTien FROM PhieuXuat WHERE YEAR(NgayLapPhieu) < "+ Year2Combobox.SelectedItem.ToString() + " OR (YEAR(NgayLapPhieu) = "+ Year2Combobox.SelectedItem.ToString() + " AND MONTH(NgayLapPhieu) < "+ MONTH + ") AND MaDaiLy = "+ ids[i] + " ;";
                SqlCommand cmd1 = new SqlCommand(query1, dbConnector.sqlCon);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    if (!reader1.IsDBNull(0))
                    {
                        decimal s = reader1.GetDecimal(0);
                        fDebts.Add(s);
                    }
                    else
                    {
                        fDebts.Add(0);
                    }
                       
                }
                reader1.Close();

            }
            for (int i = 0; i < ids.Count; i++)
            {
                int MONTH = Month2Combobox.SelectedIndex + 1;
                string query1 = "SELECT SUM(ThanhTien) as TongTien FROM PhieuXuat WHERE YEAR(NgayLapPhieu) = "+ Year2Combobox.SelectedItem.ToString() + " AND MONTH(NgayLapPhieu) = "+ MONTH + " AND MaDaiLy = " + ids[i] + " ;";
                SqlCommand cmd1 = new SqlCommand(query1, dbConnector.sqlCon);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    if (!reader1.IsDBNull(0))
                    {
                        decimal s = reader1.GetDecimal(0);
                        cDebts.Add(s);
                    }
                    else
                    {
                        cDebts.Add(0);
                    }
                }
                reader1.Close();

            }
            dbConnector.CloseConnection();
            CongNoTable.Items.Clear();
            for (int i = 0; i < ids.Count; i++)
            { 
                CongNoItem item = new CongNoItem(i + 1, names[i], fDebts[i], cDebts[i], fDebts[i] + cDebts[i]);
                CongNoTable.Items.Add(item);
            }
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
                DoanhThuItem item = new DoanhThuItem(i, s, c, d, Math.Round(percent, 2));
                i++;
                DoanhThuTable.Items.Add(item);
            }
            reader.Close();
            dbConnector.CloseConnection();
        }
        private void MonthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void Month2Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!firstLoad2)
            {
                calcMonthlyDebt();
            }
            else
            {
                firstLoad2 = false;
            }
        }

        private void Year2Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!firstLoad2)
            {
                calcMonthlyDebt();
            }
            else
            {
                firstLoad2 = false;
            }
        }
    }
}
