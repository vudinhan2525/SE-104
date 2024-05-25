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

namespace QUANLYDAILI.Pages.Setting
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            NumberAgentInp.Text = GlobalVariables.maxAgentPerDistrict.ToString();
        }

        private void ChangeNumberAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.maxAgentPerDistrict = Int32.Parse(NumberAgentInp.Text);
            MessageBox.Show("Cập nhật số lượng đại lý mỗi quận thành công.");
        }
    }
}
