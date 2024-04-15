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
      
        public MainWindow()
        {
            InitializeComponent();
            var mainPage = new MainPage();
            Main.Content = mainPage;

        }

    }
}
