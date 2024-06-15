using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLibrary
{
    public interface IDataAccess
    {
        public List<User> GetUsers();
        public List<Book> GetBooks();
        public List<Transaction> GetTransactions();
    }
}
