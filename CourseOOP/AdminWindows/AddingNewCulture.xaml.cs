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
    /// Interaction logic for AddingNewCulture.xaml
    /// </summary>
    public partial class AddingNewCulture : Window
    {
        Client? admin = null;
        public AddingNewCulture(Client? client)
        {
            InitializeComponent();
            if (client != null && client.Login == "admin")
            {
                admin = client;
            }
        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxName.Text == "" || txtBxProductivity.Text == "" ||
               txtBxFrost.Text == "" || txtBxImmunity.Text == "")
            {
                MessageBox.Show("You need to fill in all required parameters");
                return;
            }
            if (!(int.TryParse(txtBxProductivity.Text, out int _) &&
                int.TryParse(txtBxFrost.Text, out int _) &&
                int.TryParse(txtBxImmunity.Text, out int _)))
            {
                MessageBox.Show("Productivity, Frost resistance and Immunity must be a number");
                return;
            }
            
            using (MyDbContext db = new())
            {
                Culture? cultureTest = db.Cultures.FirstOrDefault(c => c.CultureName == txtBxName.Text);
                if (cultureTest != null)
                {
                    MessageBox.Show("Culture with this name is already exists");
                    return;
                }
                Culture culture = new(txtBxName.Text, 
                                      Convert.ToInt32(txtBxProductivity.Text), 
                                      Convert.ToInt32(txtBxFrost.Text),
                                      Convert.ToInt32(txtBxImmunity.Text));
                if (txtBxAuthor.Text != "")
                {
                    culture.AuthorName = txtBxAuthor.Text;
                }
                if (txtBxParent.Text != "")
                {
                    culture.ParentVariety = txtBxParent.Text;
                }
                if (txtBxSpecification.Text != "")
                {
                    culture.Specification = txtBxSpecification.Text;
                }
                if (txtBxFund.Text != "")
                {
                    culture.SelectionFund = txtBxFund.Text;
                }
                db.Cultures.Add(culture);
                db.SaveChanges();
            }
            CloseWindow();
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
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

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    btnSubmit_Click(sender, e);
                    break;
                case Key.F1:
                    MessageBox.Show("Press \"Enter\" to submit new culture\n" +
                                    "Press \"F2\" to cancel", 
                                    "Help");
                    break;
                case Key.F2:
                    btnCancel_Click(sender, e);
                    break;
            }
        }
    }
}
