using BorrowLibraryBooks.Service;
using Model;
using Model.Enum;
using Model.Transfer;
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
using System.Windows.Shapes;
using WpfLibrary;

namespace BorrowLibraryBooks
{
    /// <summary>
    /// Interaction logic for TransactionManagement.xaml
    /// </summary>
    public partial class TransactionManagement : Window
    {
        private readonly IAuthManagement _authManagement;
        private readonly ITransactionService _transactionService;
        public event EventHandler LogoutEvent;
        public event EventHandler CloseEvent;
        public List<TransactionTransfer> transactions;
        private int CurrentCredit = 0;
        public TransactionManagement(IAuthManagement authManagement, ITransactionService transactionService)
        {
            InitializeComponent();
            _authManagement = authManagement;
            _transactionService = transactionService;
        }

        public void LoadBook()
        {
            // Load combo box book
            this.cb_books.Items.Clear();
            foreach(var book in _transactionService.GetBooks().OrderBy(m => m.Name))
            {
                this.cb_books.Items.Add(new KeyValuePair<Book, string>(book, $"{book.Name}"));
            }          
            
            // Load combo box filter
            this.cb_filter.Items.Clear();
            this.cb_filter.Items.Add(new KeyValuePair<int, string>(0, $"All"));
            this.cb_filter.Items.Add(new KeyValuePair<int, string>(1, $"Booking"));
            this.cb_filter.Items.Add(new KeyValuePair<int, string>(2, $"Near expiry"));
            this.cb_filter.Items.Add(new KeyValuePair<int, string>(3, $"Expiry"));
            this.cb_filter.SelectedValue = 0;
            this.cb_filter.SelectionChanged += Cb_filter_SelectionChanged;
        }

        private void Cb_filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadTransactionList();
        }

        private void bt_logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutEvent?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }

        private void bt_borrow_Click(object sender, RoutedEventArgs e)
        {
            var book = this.cb_books.SelectedValue as Book;
            if(book == null) 
            {
                MessageBox.Show("Please choose book", "warning");
                return;
            }
            var createTransaction = _transactionService.NewTransaction(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedTime = DateTime.Now,
                BookId = book.Id,
                UserId = _authManagement.GetUserLogin().Id,
            });
            switch (createTransaction)
            {
                case CreateTransactionStatusEnum.Success:
                    ReloadTransactionList();
                    break;
                case CreateTransactionStatusEnum.NotEnoughCredit:
                    MessageBox.Show("You don't have enough credit", "warning");
                    break;
                case CreateTransactionStatusEnum.MaximumCanBorrow:
                    MessageBox.Show("You had maximum books borrow", "warning");
                    break;
                case CreateTransactionStatusEnum.NoLongerBook:
                    MessageBox.Show("This book is no longer available", "warning");
                    break;
                default:
                    MessageBox.Show("Have some error, please call admin", "error");
                    break;
            }
        }

        public void LoadInitData()
        {
            CurrentCredit = 100;
            lb_welcome.Content = $"Welcome {_authManagement.GetUserLogin()?.Fullname}";
            LoadBook();
            ReloadTransactionList();
        }

        private void ReloadTransactionList()
        {
            try
            {
                var dataFilter = cb_filter.SelectedValue;
                int filter = 0;
                if (dataFilter != null)
                {
                    Int32.TryParse(dataFilter.ToString(), out filter);
                }
                transactions = _transactionService.GetTransactionShow((TransactionStatusEnum)Enum.ToObject(typeof(TransactionStatusEnum), filter));
                tb_transaction.ItemsSource = transactions;
                CurrentCredit = _transactionService.GetCurrentCredit();
                lb_credit.Content = $"CurrentCredit: {CurrentCredit}";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
