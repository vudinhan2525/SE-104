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
namespace QUANLYDAILI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String cntString = @"Data Source=MSI\AN;Initial Catalog=QUANLYDAILI;Integrated Security=True";
        SqlConnection sqlCon = null;
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

        private void OpenConnection()
        {
            try
            {   
                if(sqlCon == null)
                {
                    sqlCon = new SqlConnection(cntString);
                }
                if(sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                    //MessageBox.Show("Connect to database success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CloseConnection()
        {
            try
            {
                if(sqlCon != null && sqlCon.State == System.Data.ConnectionState.Open)
                {
                    sqlCon.Close();
                    //MessageBox.Show("Disconnect to database success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Trim() != "" && passwordBox.Password.Trim() != "")
            {
                string query = $"SELECT COUNT(*) FROM [USERS] WHERE Username = '{usernameTextBox.Text.Trim()}' AND Password = '{passwordBox.Password.Trim()}'";
                try
                {
                    OpenConnection();

                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Connection = sqlCon;

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
                    CloseConnection();
                }
            }
        }
    }
}
