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

namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Brush originalColor;
        
        public MainPage()
        {
            InitializeComponent();
            originalColor = HomeBtn.Background; 
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            // Thay đổi màu của nút khi focus
            Button button = sender as Button;
            button.Background = Brushes.Green;
        }

        // Hàm xử lý sự kiện khi nút mất focus
        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            // Đặt lại màu gốc của nút khi mất focus
            Button button = sender as Button;
            button.Background = originalColor;
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
