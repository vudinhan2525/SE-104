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
            var mainPage = new MainPage();
            Main.Content = mainPage;

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
