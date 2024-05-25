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
        private Dictionary<string, TextBox> textBoxDictionary;
        public SettingPage()
        {
            InitializeComponent();
            NumberAgentInp.Text = GlobalVariables.maxAgentPerDistrict.ToString();
            NumberOfTypeInp.Text = GlobalVariables.numberOfTypeAgent.ToString();
            renderType();
        }
        private void renderType()
        {
            TypeName.Children.Clear();
            TypeDebt.Children.Clear();
            textBoxDictionary = new Dictionary<string, TextBox>();
            for (int i = 0; i < GlobalVariables.numberOfTypeAgent; i++)
            {
                TextBox textBox = new TextBox();
                textBox.FontSize = 14;
                textBox.IsReadOnly = true;
                if (i != 0)
                {
                    textBox.Margin = new Thickness(0, 10, 0, 0);
                }
                string s = "Loại " + (i + 1);
                textBox.Text = s;
                textBox.Padding = new Thickness(20, 10, 20, 10);

                // Create style for the border
                Style borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(8)));

                textBox.Resources.Add(typeof(Border), borderStyle);
                TypeName.Children.Add(textBox);


                TextBox textBox2 = new TextBox();
                textBox2.FontSize = 14;
                if (i != 0)
                {
                    textBox2.Margin = new Thickness(0, 10, 0, 0);
                }
                textBox2.Padding = new Thickness(20, 10, 20, 10);
                textBox2.Text = GlobalVariables.maxDebtOfAgent[i].ToString();
                // Create style for the border
                Style borderStyle2 = new Style(typeof(Border));
                borderStyle2.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(8)));

                // Apply style to the TextBox's resources
                textBox2.Resources.Add(typeof(Border), borderStyle2);
                textBoxDictionary.Add(i.ToString(), textBox2);
                // Add TextBox to the main window or any container
                // For example, assuming a StackPanel named 'stackPanel' exists in XAML
                TypeDebt.Children.Add(textBox2);
            }
        }
        private void ChangeNumberAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.maxAgentPerDistrict = Int32.Parse(NumberAgentInp.Text);
            MessageBox.Show("Cập nhật số lượng đại lý mỗi quận thành công.");
        }

        private void ChangeNumberOfTypeBtn_Click(object sender, RoutedEventArgs e)
        {   
            if(Int32.Parse(NumberOfTypeInp.Text) > GlobalVariables.numberOfTypeAgent)
            {
                for(int i = GlobalVariables.numberOfTypeAgent;i < Int32.Parse(NumberOfTypeInp.Text); i++)
                {
                    GlobalVariables.maxDebtOfAgent.Add(10000000);
                }
            }
            else if(Int32.Parse(NumberOfTypeInp.Text) < GlobalVariables.numberOfTypeAgent)
            {
                for (int i = Int32.Parse(NumberOfTypeInp.Text); i < GlobalVariables.numberOfTypeAgent; i++)
                {
                    GlobalVariables.maxDebtOfAgent.RemoveAt(GlobalVariables.maxDebtOfAgent.Count - 1);
                }
            }
            else
            {

            }
            GlobalVariables.numberOfTypeAgent = Int32.Parse(NumberOfTypeInp.Text);
            GlobalVariables.updateMap();
            MessageBox.Show("Cập nhật loại đại lí thành công.");
            
            renderType();
        }

        private void SaveMaxDebtBtn_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < GlobalVariables.numberOfTypeAgent; i++)
            {
                TextBox tmp = GetTextBoxByID(i.ToString());
                GlobalVariables.maxDebtOfAgent[i] = Int32.Parse(tmp.Text);
            }
            GlobalVariables.updateMap();
            MessageBox.Show("Cập nhật thành công.");
        }
        private TextBox GetTextBoxByID(string id)
        {
            TextBox textBox = null;
            textBoxDictionary.TryGetValue(id, out textBox);
            return textBox;
        }
    }
}
