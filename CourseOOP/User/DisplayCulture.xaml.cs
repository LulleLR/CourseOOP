using CourseOOP.Databases;
using CourseOOP.Databases.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CourseOOP.User
{
    /// <summary>
    /// Interaction logic for DisplayCulture.xaml
    /// </summary>
    public partial class DisplayCulture : Window
    {
        private Culture _culture { get; set; }
        private int _userId { get; set; }
        public DisplayCulture(Culture culture, Client user)
        {
            InitializeComponent();
            lblCultureNameOut.Content = culture.CultureName;
            lblAuthorOut.Content = culture.AuthorName;
            lblParentOut.Content = culture.ParentVariety;
            lblProductivityOut.Content = culture.Productivity;
            txtBxSpecification.Text = culture.Specification;
            lblFrostResistanceOut.Content = culture.FrostResistance;
            lblImmunityOut.Content = culture.Immunity;
            lblFundOut.Content = culture.SelectionFund;
            _culture = culture;
            _userId = user.Id;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnWriteToFile_Click(object sender, RoutedEventArgs e)
        {
            string folder = Environment.CurrentDirectory;
            using (StreamWriter sw = new($"{folder}" + $"{lblCultureNameOut.Content}.txt", false, Encoding.UTF8))
            {
                sw.WriteLine($"Culture name: {lblCultureNameOut.Content}\n" +
                             $"Productivity: {lblProductivityOut.Content}\n" +
                             $"Frost resistance: {lblFrostResistanceOut.Content}\n" +
                             $"Immunity: {lblImmunityOut.Content}");
                if (lblAuthorOut.Content != null)
                {
                    sw.WriteLine($"Author: {lblAuthorOut.Content}");
                }
                if (lblParentOut.Content != null)
                {
                    sw.WriteLine($"Parent variety: {lblParentOut.Content}");
                }
                if (lblFundOut.Content != null)
                {
                    sw.WriteLine($"Selection fund: {lblFundOut.Content}");
                }
                if (lblSpecification.Content != null)
                {
                    sw.WriteLine($"Specification: {txtBxSpecification.Text}");
                }
            }
            MessageBox.Show($"You can find your file at {folder}" + $"{lblCultureNameOut.Content}.txt");
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.F1:
                    MessageBox.Show("Press \"Enter\" to close culture\n" +
                                    "Press \"W\" to write culture to file\n" +
                                    "Press \"F2\" to add a culture to favourites\n" +
                                    "Press \"F3\" to remove the culture from favourites", 
                                    "Help");
                    break;
                case Key.Enter:
                    Close();
                    break;
                case Key.W:
                    btnWriteToFile_Click(sender, e);
                    break;
                case Key.F2:
                    btnAddToFavourites_Click(sender, e);
                    break;
                case Key.F3:
                    btnDeleteFromFavourites_Click(sender, e);
                    break;

            }
        }

        private void btnAddToFavourites_Click(object sender, RoutedEventArgs e)
        {
            using (FavouritesDbContext db = new())
            {
                Favorite[] favourites = db.Favorites.Where(f => f.UserId == _userId).ToArray();
                Favorite? culture = favourites.FirstOrDefault(f => f.CultureId == _culture.Id);
                if (culture != null)
                {
                    MessageBox.Show("This culture is already in your favourites");
                    return;
                }
                db.Favorites.Add(new Favorite(_userId, _culture.Id));
                db.SaveChanges();
            }
            MessageBox.Show("Your culture was added to favourites");
        }

        private void btnDeleteFromFavourites_Click(object sender, RoutedEventArgs e)
        {
            using (FavouritesDbContext db = new())
            {
                Favorite[] favourites = db.Favorites.Where(f => f.UserId == _userId).ToArray();
                Favorite? culture = favourites.FirstOrDefault(f => f.CultureId == _culture.Id);
                if (culture == null)
                {
                    MessageBox.Show("This culture is not in your favourites");
                    return;
                }
                db.Favorites.Remove(culture);
                db.SaveChanges();
            }
            MessageBox.Show("This culture has been removed from your favourites");
        }
    }
}
