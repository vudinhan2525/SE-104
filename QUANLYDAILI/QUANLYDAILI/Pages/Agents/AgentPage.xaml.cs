using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using QUANLYDAILI.Pages.Agents;
using QUANLYDAILI.Utils;

namespace QUANLYDAILI.Pages
{
    /// <summary>
    /// Interaction logic for AgentPage.xaml
    /// </summary>

public partial class AgentPage : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private Frame _menuFrame; 
        private List<Agent> agents = new List<Agent>();
        public AgentPage(Frame menuFrame)
        {
            InitializeComponent();
            getAllAgents();
            _menuFrame = menuFrame;
        }
        private Image addImage(string url)
        {
            var image = new Image();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
            return image;
        }
        private void getAllAgents()
        {
            string query = $"SELECT * FROM DaiLy";
            try
            {
                dbConnector.OpenConnection();

                SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Agent agent = new Agent(
                        reader.GetInt32(0),       
                        reader.GetString(1),      
                        reader.GetString(2),      
                        reader.GetString(3),  
                        reader.GetString(4),      
                        reader.GetString(5),     
                        reader.GetByte(6),       
                        reader.GetDateTime(7).ToString("yyyy-MM-dd"),    
                        reader.GetDecimal(8),
                        reader.GetString(9) 
                    );
                    agents.Add(agent);
                }
                reader.Close();
                for(int i = 0; i < agents.Count; i++)
                {
                    // Create Border element
                    Border border = new Border();
                    border.CornerRadius = new CornerRadius(12, 12, 0, 0);
                    border.Margin = new Thickness(0, 20, 0, 0);
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cccccc"));
                    border.BorderThickness = new Thickness(1);
                    border.Height = 200;
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(agents[i].Avatar)));
                    border.Background = imageBrush;
                    border.Cursor = Cursors.Hand;
                    border.Tag = i;
                    border.MouseDown += HandleEditStore;
                    // Create inner Border element
                    Border innerBorder = new Border();
                    innerBorder.CornerRadius = new CornerRadius(0, 0, 12, 12);
                    innerBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cccccc"));
                    innerBorder.BorderThickness = new Thickness(1);
                    innerBorder.Background = new SolidColorBrush(Colors.White);
                    innerBorder.Padding = new Thickness(20, 12, 20, 12);
                    innerBorder.Cursor = Cursors.Hand;
                    innerBorder.Tag = i;
                    innerBorder.MouseDown += HandleEditStore;
                    // Create StackPanel
                    StackPanel stackPanel = new StackPanel();

                    // Add TextBlocks
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = agents[i].TenDaiLy;
                    textBlock1.FontSize = 20;
                    textBlock1.FontWeight = FontWeights.Bold;
                    stackPanel.Children.Add(textBlock1);

                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Text = agents[i].Quan + ", " + agents[i].DiaChi;
                    stackPanel.Children.Add(textBlock2);

                    TextBlock textBlock4 = new TextBlock();
                    textBlock4.Text = "SDT: " + agents[i].SoDienThoai;
                    textBlock4.Margin = new Thickness(0, 5, 0, 0);
                    stackPanel.Children.Add(textBlock4);

                    TextBlock textBlock3 = new TextBlock();
                    textBlock3.Text = "Khoản nợ: " + agents[i].KhoanNo.ToString("C", CultureInfo.GetCultureInfo("vi-VN"));
                    textBlock3.Margin = new Thickness(0, 7, 0, 4);
                    textBlock3.FontSize = 14;
                    textBlock3.FontWeight = FontWeights.SemiBold;
                    stackPanel.Children.Add(textBlock3);

                    // Add StackPanel to inner Border
                    innerBorder.Child = stackPanel;
                    
                    if (i % 3 == 0)
                    {
                        stPanel1.Children.Add(border);
                        stPanel1.Children.Add(innerBorder);
                    }
                    else if(i % 3 == 1)
                    {
                        stPanel2.Children.Add(border);
                        stPanel2.Children.Add(innerBorder);
                    }
                    else if(i %3 == 2)
                    {
                        stPanel3.Children.Add(border);
                        stPanel3.Children.Add(innerBorder);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dbConnector.CloseConnection();
            }

        }
        private void HandleEditStore(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                // Retrieve the value of the Tag property
                int tagValue = (int)border.Tag;
                _menuFrame.Content = new AddAgentPage(_menuFrame, agents[tagValue]);
            }
        }
        private void handleShowAddForm(object sender, RoutedEventArgs e)
        {
            Agent a = new Agent(0, "", "", "", "", "", 0, "", 0, "");
            _menuFrame.Content = new AddAgentPage(_menuFrame, a);
        }
    }
}
