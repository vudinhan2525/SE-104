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
namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string btnId;
        public MainPage()
        {
            InitializeComponent(); 
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
                btnId = button.Name;
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0194F3"));
                button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                StackPanel stackPanel = button.Content as StackPanel;
                string res = "";
                if (btnId == "HomeBtn") res = "HomeIcon"; 
                if (btnId == "GoodsBtn") res = "GoodsIcon";
                if (btnId == "AgentBtn") res = "AgentIcon";
                if (btnId == "ImportBtn") res = "ImportIcon";
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

        // Hàm xử lý sự kiện khi nút mất focus
        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            // Đặt lại màu gốc của nút khi mất focus
            Button button = sender as Button;
            if (button != null)
            {   
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                StackPanel stackPanel = button.Content as StackPanel;
                string res = ""; ;
                if (btnId == "HomeBtn") res = "HomeIcon";
                if (btnId == "GoodsBtn") res = "GoodsIcon";
                if (btnId == "AgentBtn") res = "AgentIcon";
                if (btnId == "ImportBtn") res = "ImportIcon";
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
            mainMenu.Content = new AgentPage();
        }

        private void GoodsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new GoodsPage();
            
        }
        
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new MainPage();
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Content = new ImportsHistory(); 
        }
    }
}
