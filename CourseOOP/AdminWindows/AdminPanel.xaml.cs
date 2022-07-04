using CourseOOP.AdminWindows;
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

namespace CourseOOP
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        Client? admin = null;
        public AdminPanel(Client? client)
        {
            InitializeComponent();
            DisplayCultures();
            DisplayHistory();
            DisplayRequests();
            if (client != null && client.Login == "admin")
            {
                lblSuperAdmin.Content = "Super admin";
                admin = client;
            }
        }

        private void DisplayCultures()
        {
            Culture[] cultures;
            using (MyDbContext db = new())
            {
                cultures = db.Cultures.ToArray();
            }
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n";
            }
        }
        private void DisplayHistory()
        {
            Request[] requests;
            using (HistoryDbContext db = new())
            {
                requests = db.Requests.ToArray();
                
            }
            foreach (Request request in requests)
            {
                txtBxHistory.Text += $"{request.Text}\n";
            }
        }

        private void DisplayRequests()
        {
            AddRequest[] requests;
            using (AddRequestsDbContext db = new())
            {
                requests = db.Requests.ToArray();

            }
            foreach (AddRequest request in requests)
            {
                txtBxRequests.Text += $"{request.Text}\n";
            }
        }

        private void btnToUser_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AdminUserPanel aup;
            if (admin == null)
            {
                aup = new(null);
            }
            else
            {
                aup = new(admin);
            }
            aup.Show();
            Close();
        }
        private void btnOpUsers_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            OperateUsers ou;
            if (admin == null)
            {
                ou = new();
            }
            else
            {
                ou = new(admin);
            }
            ou.Show();
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddingNewCulture anc;
            if (admin == null)
            {
                anc = new(null);
            }
            else
            {
                anc = new(admin);
            }
            anc.Show();
            Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxEdit.Text != "" && txtBxEdit.Text != "Name of culture")
            {
                using (MyDbContext db = new())
                {
                    Culture? culture = db.Cultures.FirstOrDefault(c => c.CultureName == txtBxEdit.Text);
                    if (culture != null)
                    {
                        Hide();
                        EditingCulture ec;
                        if (admin == null)
                        {
                            ec = new(culture, null);
                        }
                        else
                        {
                            ec = new(culture, admin);
                        }
                        ec.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Culture with this name does not exists");
                    }
                }
            }
            else
            {
                MessageBox.Show("You must fill entry field");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    MessageBox.Show("Press \"F2\" to edit culture\n" +
                                    "Press \"F3\" to add new culture\n" +
                                    "Press \"F4\" to operate users\n" +
                                    "Press \"F5\" to switch to user view\n" +
                                    "Press \"Escape\" to close the program", 
                                    "Help");
                    break;
                case Key.F2:
                    btnEdit_Click(sender, e);
                    break;
                case Key.F3:
                    btnAdd_Click(sender, e);
                    break;
                case Key.F4:
                    btnOpUsers_Click(sender, e);
                    break;
                case Key.F5:
                    btnToUser_Click(sender, e);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
