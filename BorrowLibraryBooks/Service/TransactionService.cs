using Microsoft.Extensions.Configuration;
using Model;
using Model.Enum;
using Model.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfLibrary;

namespace BorrowLibraryBooks.Service
{
    public interface ITransactionService
    {
        public List<TransactionTransfer> GetTransactionShow(TransactionStatusEnum status);
        public CreateTransactionStatusEnum NewTransaction(Transaction transaction);
        public int GetCurrentCredit();
        public List<Book> GetBooks();
    }

    public class TransactionService: ITransactionService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IAuthManagement _authManagement;
        private List<Transaction> _transactions;
        private List<Book> _books;
        private List<Transaction> CurrentTransaction = new List<Transaction>();
        private int CurrentCredit = 0;
        public TransactionService(IDataAccess dataAccess, IAuthManagement authManagement) 
        {
            _authManagement = authManagement;
            _dataAccess = dataAccess;
            LoadTransaction();
        }

        private void LoadTransaction()
        {
            var userLogin = _authManagement.GetUserLogin();
            _transactions = _dataAccess.GetTransactions().ToList();
            _books = _dataAccess.GetBooks();

            // Load quantity book from setting
            var quantityBookSetting = System.Configuration.ConfigurationManager.AppSettings.Get("QuantityBook");
            if (!string.IsNullOrEmpty(quantityBookSetting))
            {
                var quantityBook = Int32.Parse(quantityBookSetting);
                foreach (var book in _books)
                {
                    book.Quantity = quantityBook;
                }
                foreach(var transaction in _transactions)
                {
                    var book = _books.FirstOrDefault(m => m.Id == transaction.BookId);
                    if (book != null)
                        book.Quantity--;
                }
            }
        }

        public List<TransactionTransfer> GetTransactionShow(TransactionStatusEnum status)
        {
            var userLogin = _authManagement.GetUserLogin();
            CurrentTransaction = _transactions.Where(m => m.UserId == userLogin?.Id).ToList();
            var transactionsShow = CurrentTransaction.Select(m =>
            {
                var book = _books.FirstOrDefault(k => k.Id == m.BookId);
                return new TransactionTransfer()
                {
                    Id = m.Id,
                    BookName = book?.Name ?? "",
                    CreatedTime = m.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ExpiryTime = m.ExpiryTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = ConverStatus(m.ExpiryTime),
                    ExpiryCredit = CalExpiryCredit(m.ExpiryTime, m.CostCredit),
                    CostCredit = m.CostCredit,
                };
            }).ToList();
            CurrentCredit = (userLogin?.Credit ?? 0) - transactionsShow.Sum(m => m.CostCredit + m.ExpiryCredit);
            switch (status)
            {
                case TransactionStatusEnum.Booking:
                    return transactionsShow.Where(m => m.Status == TransactionStatusEnum.Booking || m.Status == TransactionStatusEnum.NearExpiry).ToList();
                case TransactionStatusEnum.NearExpiry:
                    return transactionsShow.Where(m => m.Status == TransactionStatusEnum.NearExpiry).ToList();
                case TransactionStatusEnum.Expiry:
                    return transactionsShow.Where(m => m.Status == TransactionStatusEnum.Expiry).ToList();
                default:
                    return transactionsShow;
            }
            
        }

        private TransactionStatusEnum ConverStatus(DateTime expiryTime)
        {
            var status = TransactionStatusEnum.Booking;
            var now = DateTime.Now;
            if (expiryTime <= now)
                status = TransactionStatusEnum.Expiry;
            else if (expiryTime.AddDays(-7) <= now)
                status = TransactionStatusEnum.NearExpiry;
            return status;
        }

        private int CalExpiryCredit(DateTime expiryTime, int credit)
        {
            var expiryCredit = 0;
            var expiry = (DateTime.Now - expiryTime).TotalDays;
            if (expiry > 0)
            {
                expiryCredit = (int)Math.Ceiling(expiry) * credit * 20 / 100;
            }
            return expiryCredit;
        }

        public CreateTransactionStatusEnum NewTransaction(Transaction transaction)
        {
            if(_transactions == null)
                _transactions = new List<Transaction>();
            var book = _books.FirstOrDefault(k => k.Id == transaction.BookId);
            if(book == null)
                return CreateTransactionStatusEnum.Other;
            if(book.Quantity <= 0)
            {
                return CreateTransactionStatusEnum.NoLongerBook;
            }
            if (CurrentTransaction.Count() >= 5)
            {
                return CreateTransactionStatusEnum.MaximumCanBorrow;
            }
            if (CurrentCredit - book.Credit < 0)
            {
                return CreateTransactionStatusEnum.NotEnoughCredit;
            }
            book.Quantity--;
            transaction.ExpiryTime = transaction.CreatedTime.AddDays(book?.Expiry ?? 0);
            transaction.CostCredit = book?.Credit ?? 0;
            _transactions.Add(transaction);
            return CreateTransactionStatusEnum.Success;
        }

        public int GetCurrentCredit()
        {
            return CurrentCredit;
        }

        public List<Book> GetBooks()
        {
            return _books;
        }
    }
}
