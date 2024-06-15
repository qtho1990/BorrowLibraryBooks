using BorrowLibraryBooks.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BorrowLibraryBooks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly IAuthManagement _authManagement;
        public readonly TransactionManagement _transactionManagement;
        public MainWindow(IAuthManagement authManagement, TransactionManagement transactionManagement)
        {
            InitializeComponent();
            _authManagement = authManagement;
            _transactionManagement = transactionManagement;
            _transactionManagement.LogoutEvent += _transactionManagement_LogoutEvent;
            _transactionManagement.Closed += _transactionManagement_Closed;
            user_name_tb.Text = "thodang";
            pass_tb.Text = "tho123456";
        }

        private void _transactionManagement_Closed(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void _transactionManagement_LogoutEvent(object? sender, EventArgs e)
        {
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(user_name_tb.Text) || string.IsNullOrEmpty(pass_tb.Text))
            {
                MessageBox.Show("Please enter username and password", "Login Failed");
                return;
            }
            var userLogin = _authManagement.Login(user_name_tb.Text, pass_tb.Text);
            if (userLogin == null)
            {
                MessageBox.Show("Username or Password incorrect", "Login Failed");
                return;
            }
            _transactionManagement.LoadInitData();
            _transactionManagement.Show();
            this.Hide();
            //this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _transactionManagement.Close();
        }
    }
}