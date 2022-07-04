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

namespace CourseOOP.Login
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            using (UsersDbContext db = new())
            {
                Client? client = db.Users.FirstOrDefault(u => u.Login == txtBxLogin.Text);
                if (client != null)
                {
                    MessageBox.Show("User with this id is already exists");
                    return;
                }
                else
                {
                    if (txtBxLogin.Text != "" && txtBxPassword.Text != "")
                    {
                        db.Users.Add(new Client(txtBxLogin.Text, txtBxPassword.Text, false));
                        db.SaveChanges();
                        Hide();
                        MainWindow mw = new();
                        mw.Show();
                        Close();
                    }
                }
            }
        }

        private void RegistrationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    MessageBox.Show("Press \"Enter\" to register new user\n" +
                                    "Press \"Escape\" to close the program",
                                    "Help");
                    break;
                case Key.Enter:
                    btnRegister_Click(sender, e);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new();
            Hide();
            mw.Show();
            Close();
        }
    }
}
