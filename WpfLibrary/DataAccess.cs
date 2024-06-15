using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WpfLibrary
{
    public class DataAccess : IDataAccess
    {
        private List<User> Users = new List<User>()
        {
            new User() {Id = 1, Username = "thodang", Password = "tho123456", Fullname = "Tho Dang"},
            new User() {Id = 2, Username = "dev", Password = "dev123456", Fullname = "Nguyen Van An"},
            new User() {Id = 3, Username = "qc", Password = "qc123456", Fullname = "Le Thi Hai"},
        };
        private List<Book> Books = new List<Book>()
        {
            new Book() {Id = 1, Name = "David Copperfield", Expiry = 10, Credit = 20},
            new Book() {Id = 2, Name = "My Name Is Asher Lev", Expiry = 3, Credit = 10},
            new Book() {Id = 3, Name = "Song of Solomon", Expiry = 5, Credit = 15},
            new Book() {Id = 4, Name = "Drowning Ruth", Expiry = 3, Credit = 35},
            new Book() {Id = 5, Name = "The Rachel Papers", Expiry = 7, Credit = 15},
            new Book() {Id = 6, Name = "Abel's Island", Expiry = 10, Credit = 15},
            new Book() {Id = 7, Name = "Ethan Frome", Expiry = 3, Credit = 40},
            new Book() {Id = 8, Name = "Daniel Deronda", Expiry = 11, Credit = 20},
            new Book() {Id = 9, Name = "Joseph Andrews", Expiry = 20, Credit = 10},
            new Book() {Id = 10, Name = "Ahab's Wife, or The Star-Gazer", Expiry = 15, Credit = 10},
            new Book() {Id = 11, Name = "Back to Methuselah", Expiry = 4, Credit = 20},
        };
        private List<Transaction> Transactions = new List<Transaction>()
        {
            new Transaction() {Id = "30da5522-8dae-4070-9a70-dcd72dd33559", BookId = 1, CostCredit = 20, CreatedTime = new DateTime(2024, 6, 1, 10, 0, 0), ExpiryTime = new DateTime(2024, 6, 11, 10, 0, 0), UserId = 1},
            new Transaction() {Id = "8ae2383a-5d83-44ad-bd24-11b45ac7e42e", BookId = 2, CostCredit = 10, CreatedTime = new DateTime(2024, 6, 1, 08, 20, 0), ExpiryTime = new DateTime(2024, 6, 4, 08, 20, 0), UserId = 1},
            new Transaction() {Id = "d7466277-e897-4ca4-8d5d-520869dc1d1c", BookId = 3, CostCredit = 15, CreatedTime = new DateTime(2024, 6, 15, 14, 15, 0), ExpiryTime = new DateTime(2024, 6, 20, 14, 15, 0), UserId = 2}
        };
        public DataAccess()
        {
        }
        public List<User> GetUsers()
        {
            return Users;
        }

        public List<Book> GetBooks()
        {
            return Books;
        }

        public List<Transaction> GetTransactions()
        {
            return Transactions;
        }
    }
}
