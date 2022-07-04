using CourseOOP.Databases;
using CourseOOP.Databases.Models;
using CourseOOP.User;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseOOP
{
    /// <summary>
    /// Interaction logic for AdminUserPanel.xaml
    /// </summary>
    public partial class AdminUserPanel : Window
    {
        private readonly Dictionary<int, string> dict = new()
        {
            [0] = "Author",
            [1] = "Parent variety",
            [2] = "Productivity",
            [3] = "Specification",
            [4] = "Frost resistance",
            [5] = "Immunity",
            [6] = "Selection fund",
        };
        private Client _user { get; set; }
        public AdminUserPanel(Client client)
        {
            InitializeComponent();
            DisplayCultures();
            DisplayHistory();
            _user = client;
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtBxSearchFeature.Text = dict[comboBox.SelectedIndex];
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            using (MyDbContext db = new())
            {
                Culture[] cultures = db.Cultures.Where(c => c.CultureName.Contains(txtBxSearchName.Text)).ToArray();
                txtBxCultures.Text = "";
                foreach (Culture culture in cultures)
                {
                    txtBxCultures.Text += $"{culture.CultureName}\n";
                }
            }
            if (txtBxSearchName.Text == "")
            {
                return;
            }
            using (HistoryDbContext db = new())
            {
                db.Requests.Add(new Request(txtBxSearchName.Text));
                db.SaveChanges();
            }
        }

        private void btnSearchFeature_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxSearchFeature.Text == "")
            {
                MessageBox.Show("You must fill entry field");
                return;
            }
            using (MyDbContext db = new())
            {
                Culture[] cultures = db.Cultures.ToArray();
                if (comboBox.SelectedIndex == 2 || comboBox.SelectedIndex == 4 || comboBox.SelectedIndex == 5)
                {
                    IntFeatures(cultures);
                }
                else
                {
                    StringFeatures(cultures);
                }
            }
            using (HistoryDbContext db = new())
            {
                db.Requests.Add(new Request($"{dict[comboBox.SelectedIndex]}: {txtBxSearchFeature.Text}"));
                db.SaveChanges();
            }
        }
        private void FeatureNotFound()
        {
            txtBxCultures.Text = "Cultures with these features have not been found";
        }

        private static void NotANumber()
        {
            MessageBox.Show("You must enter a number");
        }

        private static void UnknownComparisonOperation()
        {
            MessageBox.Show("Unknown comparison operation");
        }

        private void StringFeatures(Culture[] cultures)
        {
            Culture[]? outputCultures = null;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    outputCultures = cultures.Where(c => (c.AuthorName ?? "").Contains(txtBxSearchFeature.Text)).ToArray();
                    break;
                case 1:
                    outputCultures = cultures.Where(c => (c.ParentVariety ?? "").Contains(txtBxSearchFeature.Text)).ToArray();
                    break;
                case 3:
                    outputCultures = cultures.Where(c => (c.Specification ?? "").Contains(txtBxSearchFeature.Text)).ToArray();
                    break;
                case 6:
                    outputCultures = cultures.Where(c => (c.SelectionFund ?? "").Contains(txtBxSearchFeature.Text)).ToArray();
                    break;
            }
            if (outputCultures == null)
            {
                FeatureNotFound();
            }
            else
            {
                PrintFeatures(outputCultures);
            }
        }

        private void IntFeatures(Culture[] cultures)
        {
            string[] terms = txtBxSearchFeature.Text.Split(' ');
            if (terms.Length > 3)
            {
                MessageBox.Show("Too much spaces in entry field");
                return;
            }
            if (terms.Length == 1)
            {
                if (int.TryParse(terms[0], out int feature))
                {
                    SplitResultOne(cultures, feature);
                    return;
                }
                else
                {
                    NotANumber();
                    return;
                }
            }
            if (terms.Length == 2)
            {
                SplitResultTwo(terms, cultures);
            }
        }

        private void SplitResultOne(Culture[] cultures, int feature)
        {
            Culture[]? outputCultures = null;
            switch (comboBox.SelectedIndex)
            {
                case 2:
                    outputCultures = cultures.Where(c => c.Productivity == feature).ToArray();
                    break;
                case 4:
                    outputCultures = cultures.Where(c => c.FrostResistance == feature).ToArray();
                    break;
                case 5:
                    outputCultures = cultures.Where(c => c.Immunity == feature).ToArray();
                    break;
            }
            if (outputCultures == null)
            {
                FeatureNotFound();
            }
            else
            {
                PrintFeatures(outputCultures);
            }
        }

        private void SplitResultTwo(string[] terms, Culture[] cultures)
        {
            if (int.TryParse(terms[0], out int feature))
            {
                if (terms[1] != ">" && terms[1] != ">=" && terms[1] != "<" && terms[1] != "<=")
                {
                    UnknownComparisonOperation();
                    return;
                }
                SelectionFeature(cultures, feature, terms[1]);
            }
            else if (int.TryParse(terms[1], out feature))
            {
                if (terms[0] != ">" && terms[0] != ">=" && terms[0] != "<" && terms[0] != "<=")
                {
                    UnknownComparisonOperation();
                    return;
                }
                SelectionFeature(cultures, feature, terms[0]);
            }
            else
            {
                NotANumber();
            }
        }

        private void SelectionFeature(Culture[] cultures, int feature, string comparison)
        {
            switch (comboBox.SelectedIndex)
            {
                case 2:
                    SplitTwoProductivityFeature(cultures, feature, comparison);
                    break;
                case 4:
                    SplitTwoFrostResistanceFeature(cultures, feature, comparison);
                    break;
                case 5:
                    SplitTwoImmunityFeature(cultures, feature, comparison);
                    break;
            }
        }

        private void SplitTwoProductivityFeature(Culture[] cultures, int feature, string comparison)
        {
            Culture[] outputCultures = comparison switch
            {
                ">" => cultures.Where(c => c.Productivity > feature).ToArray(),
                ">=" => cultures.Where(c => c.Productivity >= feature).ToArray(),
                "<" => cultures.Where(c => c.Productivity < feature).ToArray(),
                _ => cultures.Where(c => c.Productivity <= feature).ToArray(),
            };
            PrintProductivityFeature(outputCultures);
        }

        private void SplitTwoFrostResistanceFeature(Culture[] cultures, int feature, string comparison)
        {
            Culture[] outputCultures = comparison switch
            {
                ">" => cultures.Where(c => c.FrostResistance > feature).ToArray(),
                ">=" => cultures.Where(c => c.FrostResistance >= feature).ToArray(),
                "<" => cultures.Where(c => c.FrostResistance < feature).ToArray(),
                _ => cultures.Where(c => c.FrostResistance <= feature).ToArray(),
            };
            PrintFrostResistanceFeature(outputCultures);
        }

        private void SplitTwoImmunityFeature(Culture[] cultures, int feature, string comparison)
        {
            Culture[] outputCultures = comparison switch
            {
                ">" => cultures.Where(c => c.Immunity > feature).ToArray(),
                ">=" => cultures.Where(c => c.Immunity >= feature).ToArray(),
                "<" => cultures.Where(c => c.Immunity < feature).ToArray(),
                _ => cultures.Where(c => c.Immunity <= feature).ToArray(),
            };
            PrintImmunityFeature(outputCultures);
        }

        private void PrintFeatures(Culture[] cultures)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    PrintAuthorNameFeature(cultures);
                    break;
                case 1:
                    PrintParentVarietyFeature(cultures);
                    break;
                case 2:
                    PrintProductivityFeature(cultures);
                    break;
                case 3:
                    PrintSpecificationFeature(cultures);
                    break;
                case 4:
                    PrintFrostResistanceFeature(cultures);
                    break;
                case 5:
                    PrintImmunityFeature(cultures);
                    break;
                case 6:
                    PrintSelectionFundFeature(cultures);
                    break;
            }
        }

        private void PrintAuthorNameFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Author name: {culture.AuthorName}\n\n";
            }
        }

        private void PrintParentVarietyFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Parent variety: {culture.ParentVariety}\n\n";
            }
        }

        private void PrintProductivityFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Productivity: {culture.Productivity}\n\n";
            }
        }

        private void PrintSpecificationFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Specification: {culture.Specification}\n\n";
            }
        }
        private void PrintFrostResistanceFeature(Culture[] cultures)
        {

            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Frost resistance: {culture.FrostResistance}\n\n";
            }
        }
        private void PrintImmunityFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Immunity: {culture.Immunity}\n\n";
            }
        }

        private void PrintSelectionFundFeature(Culture[] cultures)
        {
            txtBxCultures.Text = "";
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += $"{culture.CultureName}\n" +
                                      $"Selection fund: {culture.SelectionFund}\n\n";
            }
        }

        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            using (MyDbContext db = new())
            {
                Culture? culture = db.Cultures.FirstOrDefault(c => c.CultureName == txtBxDisplay.Text);
                if (culture == null)
                {
                    MessageBox.Show("Unknown culture");
                    return;
                }
                DisplayCulture dc = new(culture, _user);
                dc.Show();
            }
        }

        private void btnToAdmin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AdminPanel ap = new(_user);
            ap.Show();
            Close();
        }

        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (txtBxRequest.Text != "" || txtBxRequest.Text != "Your request")
            {
                using (AddRequestsDbContext db = new())
                {
                    db.Requests.Add(new AddRequest(txtBxRequest.Text));
                    db.SaveChanges();
                }
                MessageBox.Show("Your request has been sent");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    btnToAdmin_Click(sender, e);
                    break;
                case Key.F1:
                    MessageBox.Show("Press \"F2\" to search by name\n" +
                                    "Press \"F3\" to search by feature\n" +
                                    "Press \"F4\" to display culture\n" +
                                    "Press \"F5\" to send request\n" +
                                    "Press \"F6\" to display favourites\n" +
                                    "Press \"Escape\" to close the program",
                                    "Help");
                    break;
                case Key.F2:
                    btnSearch_Click(sender, e);
                    break;
                case Key.F3:
                    btnSearchFeature_Click(sender, e);
                    break;
                case Key.F4:
                    btnDisplay_Click(sender, e);
                    break;
                case Key.F5:
                    btnSendRequest_Click(sender, e);
                    break;
                case Key.F6:
                    btnFavourites_Click(sender, e);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void btnFavourites_Click(object sender, RoutedEventArgs e)
        {
            txtBxCultures.Text = "";
            Favorite[] favourites;
            using (FavouritesDbContext db = new())
            {
                favourites = db.Favorites.Where(f => f.UserId == _user.Id).ToArray();
            }
            List<Culture> cultures = new();
            using (MyDbContext db = new())
            {
                for (int i = 0; i < favourites.Length; i++)
                {
                    Culture? culture = db.Cultures.FirstOrDefault(c => c.Id == favourites[i].CultureId);
                    if (culture != null)
                    {
                        cultures.Add(culture);
                    }
                }
            }
            foreach (Culture culture in cultures)
            {
                txtBxCultures.Text += culture.CultureName + "\n";
            }
        }
    }
}
