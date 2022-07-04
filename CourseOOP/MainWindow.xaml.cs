using CourseOOP.Databases;
using CourseOOP.Databases.Models;
using CourseOOP.Login;
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

namespace CourseOOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void registrationLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Hide();
            Registration rg = new();
            rg.Show();
            Close();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            Client? client;
            using (UsersDbContext db = new())
            {
                client = db.Users.FirstOrDefault(u => u.Login == txtBxLogin.Text);
                if (client == null)
                {
                    MessageBox.Show("User with this login does not exists");
                    return;
                }
            }
            if (client.Password == txtBxPassword.Text)
            {
                if (client.IsAdmin == true)
                {
                    AdminPanel ap = new(client);
                    Hide();
                    ap.Show();
                    Close();
                }
                else
                {
                    UserPanel up = new(client);
                    Hide();
                    up.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Wrong password");
            }
        }

        private void firstWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    MessageBox.Show("Press \"Enter\" to enter the program\n" +
                                    "Press \"F2\" to register a new user\n" +
                                    "Press \"Escape\" to close the program",
                                    "Help");
                    break;
                case Key.Enter:
                    btnEnter_Click(sender, e);
                    break;
                case Key.F2:
                    registrationLabel_MouseDoubleClick(sender, null);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
