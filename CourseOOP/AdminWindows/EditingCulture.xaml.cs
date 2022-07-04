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
    /// Interaction logic for EditingCulture.xaml
    /// </summary>
    public partial class EditingCulture : Window
    {
        private Culture _culture;
        private Client? admin = null;
        public EditingCulture(Culture culture, Client? client)
        {
            InitializeComponent();
            lblCultureName.Content = culture.CultureName;
            txtBxAuthor.Text = culture.AuthorName;
            txtBxParent.Text = culture.ParentVariety;
            txtBxProductivity.Text = culture.Productivity.ToString();
            txtBxSpecification.Text = culture.Specification;
            txtBxFrost.Text = culture.FrostResistance.ToString();
            txtBxImmunity.Text = culture.Immunity.ToString();
            txtBxFund.Text = culture.SelectionFund;
            _culture = culture;
            if (client != null && client.Login == "admin")
            {
                admin = client;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                                  "Are you sure you want to delete the culture?", 
                                  "Deleting culture",
                                  MessageBoxButton.YesNo
                                  );
            if (result == MessageBoxResult.Yes)
            {
                using (MyDbContext db = new())
                {
                    db.Cultures.Remove(_culture);
                    db.SaveChanges();
                }
                CloseWindow();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxProductivity.Text == "" || txtBxFrost.Text == "" || txtBxImmunity.Text == "")
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
                Culture changedCulture = new((string)lblCultureName.Content, 
                                             int.Parse(txtBxProductivity.Text), 
                                             int.Parse(txtBxFrost.Text), 
                                             int.Parse(txtBxImmunity.Text));
                if (txtBxAuthor.Text != "")
                {
                    changedCulture.AuthorName = txtBxAuthor.Text;
                }
                if (txtBxParent.Text != "")
                {
                    changedCulture.ParentVariety = txtBxParent.Text;
                }
                if (txtBxSpecification.Text != "")
                {
                    changedCulture.Specification = txtBxSpecification.Text;
                }
                if (txtBxFund.Text != "")
                {
                    changedCulture.SelectionFund = txtBxFund.Text;
                }
                db.Cultures.Remove(_culture);
                db.Cultures.Add(changedCulture);
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
                case Key.F1:
                    MessageBox.Show("Press \"Enter\" to submit culture\n" +
                                    "Press \"F2\" to cancel editing\n" +
                                    "Press \"F3\" to delete culture", 
                                    "Help");
                    break;
                case Key.Enter:
                    btnSubmit_Click(sender, e);
                    break;
                case Key.F2:
                    btnCancel_Click(sender, e);
                    break;
                case Key.F3:
                    btnDelete_Click(sender, e);
                    break;
            }
        }
    }
}
