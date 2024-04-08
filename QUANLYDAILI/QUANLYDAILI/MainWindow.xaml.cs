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
            
        }

        private void usernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(usernameTextBox.Text == "Username")
            {
                usernameTextBox.Text = "";
            }
            usernameTextBox.BorderBrush = Brushes.Gray;
            errorMessagelogin.Visibility = Visibility.Collapsed;
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "Password")
            {
                passwordBox.Password = "";
            }
            passwordBox.BorderBrush = Brushes.Gray;
            errorMessagelogin.Visibility = Visibility.Collapsed;
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Trim() != "" && passwordBox.Password.Trim() != "")
            {
                string query = $"SELECT COUNT(*) FROM [USERS] WHERE Username = '{usernameTextBox.Text.Trim()}' AND Password = '{passwordBox.Password.Trim()}'";
                try
                {
                    dbConnector.OpenConnection();

                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Connection = dbConnector.sqlCon;

                    int count = (int)sqlCmd.ExecuteScalar();
                    if(count > 0)
                    {
                        //Login success
                    }
                    else
                    {   
                        //Login failed
                        usernameTextBox.BorderBrush = Brushes.Red;
                        passwordBox.BorderBrush = Brushes.Red;
                        errorMessagelogin.Visibility = Visibility.Visible;
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
        }
    }
}
