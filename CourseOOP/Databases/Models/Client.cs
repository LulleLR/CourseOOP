using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseOOP.Databases.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public Client(string login, string password, bool isAdmin)
        {
            Login = login;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}
