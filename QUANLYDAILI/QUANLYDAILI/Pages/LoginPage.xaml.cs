using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QUANLYDAILI.Utils;

namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Frame _mainFrame;
        private DatabaseConnector dbConnector = new DatabaseConnector();
        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }
        private void usernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text == "Username")
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
                    if (count > 0)
                    {

                        //Login success
                        _mainFrame.Content = new MainPage();
                        loginPage.Visibility = Visibility.Collapsed;
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
