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

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for AddAgentPage.xaml
    /// </summary>
    public partial class AddAgentPage : Page
    {
        private Frame _menuFrame;
        public AddAgentPage(Frame menuFrame)
        {
            InitializeComponent();
            _menuFrame = menuFrame;
            TypeInp.Items.Add("Loại 1");
            TypeInp.Items.Add("Loại 2");
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

    }
}
