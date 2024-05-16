using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using QUANLYDAILI.Utils;

namespace QUANLYDAILI.Pages.Bills
{
    /// <summary>
    /// Interaction logic for BillPage.xaml
    /// </summary>
    public partial class BillPage : Page
    {
        private DatabaseConnector dbConnector = new DatabaseConnector();
        private List<Bill> bills = new List<Bill>();
        public BillPage()
        {
            InitializeComponent();
            getBillList();
        }
        private void getBillList()
        {
            string query = "SELECT * FROM PhieuThu ORDER BY MaPhieuThu DESC";
            try
            {
                dbConnector.OpenConnection();

                SqlCommand command = new SqlCommand(query, dbConnector.sqlCon);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Bill b = new Bill(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetDecimal(7), reader.GetDateTime(8).ToString("yyyy-MM-dd"));
                    bills.Add(b);
                }

                for (int i = 0; i < bills.Count; i++)
                {
                    Border outerBorder = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Background = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cccccc")),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(10, 8, 10, 8),
                        Margin = new Thickness(20, 0, 80, 0)
                    };
                    if(i != 0)
                    {
                        outerBorder.Margin = new Thickness(20, 15, 80, 0);
                    }
                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    // Logo
                    Border logoBorder = new Border
                    {
                        CornerRadius = new CornerRadius(12),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cccccc")),
                        BorderThickness = new Thickness(1),
                        Height = 100,
                        Width = 100,
                        Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(bills[i].Avatar))
                        }
                    };
                    Grid.SetColumn(logoBorder, 0);
                    grid.Children.Add(logoBorder);

                    // Shop details
                    StackPanel detailsStackPanel = new StackPanel { Margin = new Thickness(15, 0, 0, 0) };
                    TextBlock shopNameTextBlock = new TextBlock
                    {
                        Text = bills[i].TenDaiLy,
                        Margin = new Thickness(0, 8, 0, 0),
                        FontSize = 22,
                        FontWeight = FontWeights.SemiBold
                    };
                    TextBlock locationTextBlock = new TextBlock
                    {
                        Text = bills[i].DiaChi,
                        Margin = new Thickness(0, 4, 0, 0),
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096"))
                    };
                    StackPanel contactStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 20, 0, 0)
                    };
                    TextBlock phoneTextBlock = new TextBlock
                    {
                        Text = "SDT : " + bills[i].SoDienThoai,
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096"))
                    };
                    TextBlock emailTextBlock = new TextBlock
                    {
                        Text = bills[i].Email,
                        FontSize = 14,
                        Margin = new Thickness(20, 0, 0, 0),
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096"))
                    };
                    contactStackPanel.Children.Add(phoneTextBlock);
                    contactStackPanel.Children.Add(emailTextBlock);
                    detailsStackPanel.Children.Add(shopNameTextBlock);
                    detailsStackPanel.Children.Add(locationTextBlock);
                    detailsStackPanel.Children.Add(contactStackPanel);
                    Grid.SetColumn(detailsStackPanel, 1);
                    grid.Children.Add(detailsStackPanel);

                    // Billing details
                    StackPanel billingStackPanel = new StackPanel { Margin = new Thickness(0, 0, 20, 0) };
                    TextBlock dateTextBlock = new TextBlock
                    {
                        Text = "Ngày thu: " + bills[i].NgayThu,
                        Margin = new Thickness(0, 10, 0, 0),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096")),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    TextBlock invoiceTextBlock = new TextBlock
                    {
                        Text = "Mã phiếu thu: " + bills[i].MaPhieuThu,
                        Margin = new Thickness(0, 4, 0, 0),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096")),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    StackPanel amountStackPanel = new StackPanel
                    {
                        Margin = new Thickness(0, 30, 0, 0),
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Label amountLabel = new Label
                    {
                        Content = "Số tiền: ",
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#718096")),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    TextBlock amountTextBlock = new TextBlock
                    {
                        Text = bills[i].SoTienThu.ToString("N0"),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd6b20")),
                        Margin = new Thickness(0, 0, 0, 1),
                        FontSize = 22,
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Label currencyLabel = new Label
                    {
                        Content = "VNĐ",
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    amountStackPanel.Children.Add(amountLabel);
                    amountStackPanel.Children.Add(amountTextBlock);
                    amountStackPanel.Children.Add(currencyLabel);
                    billingStackPanel.Children.Add(dateTextBlock);
                    billingStackPanel.Children.Add(invoiceTextBlock);
                    billingStackPanel.Children.Add(amountStackPanel);
                    Grid.SetColumn(billingStackPanel, 2);
                    grid.Children.Add(billingStackPanel);

                    outerBorder.Child = grid;
                    mainPanel.Children.Add(outerBorder);

                }
                  

            }
            catch (Exception ex)
            {

            }
            finally { dbConnector.CloseConnection(); }
            
        }
    }
}
