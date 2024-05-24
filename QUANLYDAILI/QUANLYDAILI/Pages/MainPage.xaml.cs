using QUANLYDAILI.Pages;
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
using System.Collections.ObjectModel;
using FontAwesome.WPF;
using QUANLYDAILI.Pages.Bills;
using QUANLYDAILI.Pages.Agents.Export;
using QUANLYDAILI.Pages.Statistics;

namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string btnId;
        private Button tmpBtn;
        public MainPage()
        {
            InitializeComponent();
            tmpBtn = HomeBtn;
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Name != btnId)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f3f3f3"));
            }
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Name != btnId)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            }
        }
        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            // Thay đổi màu của nút khi focus
            Button button = sender as Button;
            if (button != null)
            {   
                if(button.Name != tmpBtn.Name)
                {
                    resetButton();
                }
                tmpBtn = button; 
                btnId = button.Name;
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0194F3"));
                button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                StackPanel stackPanel = button.Content as StackPanel;
                string res = "";
                if (btnId == "HomeBtn") res = "HomeIcon"; 
                if (btnId == "GoodsBtn") res = "GoodsIcon";
                if (btnId == "AgentBtn") res = "AgentIcon";
                if (btnId == "ImportBtn") res = "ImportIcon";
                if (btnId == "ExportBtn") res = "ExportIcon";
                if (btnId == "BillBtn") res = "BillIcon";
                if (stackPanel != null)
                {
               
                    ImageAwesome imageAwesome = stackPanel.FindName(res) as ImageAwesome;
                    if (imageAwesome != null)
                    {
                        imageAwesome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                    }
                }
            }
        }
        private void resetButton()
        {
            if (tmpBtn != null)
            {
                tmpBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                tmpBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                StackPanel stackPanel = tmpBtn.Content as StackPanel;
                string res = ""; ;
                if (btnId == "HomeBtn") res = "HomeIcon";
                if (btnId == "GoodsBtn") res = "GoodsIcon";
                if (btnId == "AgentBtn") res = "AgentIcon";
                if (btnId == "ImportBtn") res = "ImportIcon";
                if (btnId == "ExportBtn") res = "ExportIcon";
                if (btnId == "BillBtn") res = "BillIcon";
                if (stackPanel != null)
                {

                    ImageAwesome imageAwesome = stackPanel.FindName(res) as ImageAwesome;
                    if (imageAwesome != null)
                    {
                        imageAwesome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                    }
                }
            }

        }


        private void AgentBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new AgentPage(mainMenu);
        }

        private void GoodsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new GoodsPage();
            
        }
        
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new StatisticsPage();
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new ImportsHistory(); 
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new ExportHistory();
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new BillPage();
        }
        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new Report();
        }
    }
}
