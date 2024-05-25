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
using QUANLYDAILI.Utils;

namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class GlobalVariables
    {
        public static int PhanTram { get; set; }
        public static List<string> Districts { get; set; } = new List<string>
        {
            "Quận 1",
            "Quận 2",
            "Quận 3",
            "Quận 4",
            "Quận 5",
            "Quận 6",
            "Quận 7",
            "Quận 8",
            "Quận 9",
            "Quận 10",
            "Quận 11",
            "Quận 12",
            "Quận 13",
            "Quận 14",
            "Quận 15",
            "Quận 16",
            "Quận 17",
            "Quận 18",
            "Quận 19",
            "Quận 20"
        };
        public static int maxAgentPerDistrict { get; set; }
        public static int numberOfTypeAgent { get; set; }

        public static Dictionary<string, int> typeAgent = new Dictionary<string, int>();

        public static List<int> maxDebtOfAgent = new List<int>();
        public static void updateMap()
        {
            for (int i = 1; i <= numberOfTypeAgent; i++)
            {
                string s = "Loại " + i;
                typeAgent[s] = maxDebtOfAgent[i - 1];
            }
        }



    }
    public partial class MainWindow : Window
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        
        public Frame GetMainFrame()
        {
            return Main;
        }
        public MainWindow()
        {
            InitializeComponent();
            GlobalVariables.PhanTram = 102;
            GlobalVariables.maxAgentPerDistrict = 4;
            GlobalVariables.numberOfTypeAgent = 2;
            GlobalVariables.maxDebtOfAgent.Add(10000000);
            GlobalVariables.maxDebtOfAgent.Add(5000000);
            GlobalVariables.updateMap();
            var loginPage = new LoginPage(Main);
            Main.Content = loginPage;

        }
            

    }
    public class DoanhThuItem
    {
        public int STT { get; set; }
        public string TenDaiLy { get; set; }
        public int SoPhieuXuat { get; set; }
        public decimal TongTriGia { get; set; }
        public decimal TyLe { get; set; }
        public DoanhThuItem(int _STT,string _TenDaiLy, int _SoPhieuXuat, decimal _TongTriGia, decimal _TyLe)
        {
            STT = _STT;
            TenDaiLy = _TenDaiLy;
            SoPhieuXuat = _SoPhieuXuat;
            TongTriGia = _TongTriGia;
            TyLe = _TyLe;
        }
    }
}
