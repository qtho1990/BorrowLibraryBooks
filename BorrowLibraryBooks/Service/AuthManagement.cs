using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfLibrary;

namespace BorrowLibraryBooks.Service
{
    public interface IAuthManagement
    {
        public User Login(string username, string password);
        public void Logout();
        public User GetUserLogin();
    }

    public class AuthManagement: IAuthManagement
    { 
        private List<User> _users = new List<User>();
        public User CurrentLogin = null;
        private readonly IDataAccess _dataAccess;
        public AuthManagement(IDataAccess dataAccess) 
        {
            _dataAccess = dataAccess;
            _users = _dataAccess.GetUsers();
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            CurrentLogin = _users.FirstOrDefault(m => m.Username == username && m.Password == password);
            return CurrentLogin;
        }

        public void Logout()
        {
            CurrentLogin = null;
        }

        public User GetUserLogin()
        {
            return CurrentLogin;
        }
    }
}
