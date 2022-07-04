using CourseOOP.Databases;
using CourseOOP.Databases.Models;
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

namespace CourseOOP.AdminWindows
{
    /// <summary>
    /// Interaction logic for OperateUsers.xaml
    /// </summary>
    public partial class OperateUsers : Window
    {
        Client? admin = null;
        public OperateUsers()
        {
            InitializeComponent();
            DisplayUsers();
        }
        public OperateUsers(Client client)
        {
            InitializeComponent();
            DisplayAllUsers();
            admin = client;
        }

        private void DisplayUsers()
        {
            Client[] clients;
            using (UsersDbContext db = new())
            {
                clients = db.Users.ToArray();
            }
            foreach (Client client in clients)
            {
                if (client.IsAdmin == true)
                {
                    continue;
                }
                txtBxUsers.Text += $"{client.Login}\n";
            }
        }

        private void DisplayAllUsers()
        {
            Client[] clients;
            using (UsersDbContext db = new())
            {
                clients = db.Users.ToArray();
            }
            foreach (Client client in clients)
            {
                if (client.IsAdmin == true)
                {
                    txtBxUsers.Text += $"Admin: {client.Login}\n";
                    continue;
                }
                txtBxUsers.Text += $"User: {client.Login}\n";
            }
        }

        private void btnMakeAdmin_Click(object sender, RoutedEventArgs e)
        {
            using (UsersDbContext db = new())
            {
                Client? client = db.Users.FirstOrDefault(u => u.Login == txtBxUser.Text);
                if (txtBxUser.Text == "" || client == null)
                {
                    MessageBox.Show("User with this login does not exists");
                    return;
                }
                else if (client.IsAdmin == true)
                {
                    MessageBox.Show("User with this login is already an admin");
                }
                else
                {
                    db.Users.Remove(client);
                    db.Users.Add(new Client(client.Login, client.Password, true));
                    db.SaveChanges();
                    MessageBox.Show("This user is now admin");
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AdminPanel ap;
            if (admin == null)
            {
                ap = new(null);
            }
            else
            { 
                ap = new(admin);
            }
            ap.Show();
            Close();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (admin == null)
            {
                MessageBox.Show("You do not have permission to perform this action");
                return;
            }
            using (UsersDbContext db = new())
            {
                Client? client = db.Users.FirstOrDefault(u => u.Login == txtBxUser.Text);
                if (txtBxUser.Text == "" || client == null)
                {
                    MessageBox.Show("User with this login does not exists");
                    return;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                                  $"Are you sure you want to delete user with login \"{client.Login}\"?",
                                  "Deleting culture",
                                  MessageBoxButton.YesNo
                                  );
                    if (result == MessageBoxResult.Yes)
                    {
                        db.Users.Remove(client);
                        db.SaveChanges();
                    }
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    MessageBox.Show("Press \"F2\" to make user admin\n" +
                                    "Press \"F3\" to delete the user\n" +
                                    "Press \"Enter\" to return to admin panel", 
                                    "Help");
                    break;
                case Key.F2:
                    btnMakeAdmin_Click(sender, e);
                    break;
                case Key.F3:
                    btnDeleteUser_Click(sender, e);
                    break;
                case Key.Enter:
                    btnClose_Click(sender, e);
                    break;
            }
        }
    }
}
