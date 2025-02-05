using AffärsLager;
using DataLager;
using EntitetsLager;
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

namespace Presentationslager.Personal.Träningspasshantering
{
    /// <summary>
    /// Interaction logic for RedigeraträningspassWindow.xaml
    /// </summary>
    public partial class RedigeraträningspassWindow : Window
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemController _medlemController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private List<Tränare> _tränares;
        private List<Träningspass> _träningspass;
        private Träningspass _valdträningspass;

        public RedigeraträningspassWindow(TräningspassController träningspassController, UnitOfWork unitOfWork, TränareController tränareController, MedlemController medlemController, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _medlemController = medlemController;
            _träningspassController = träningspassController;
            _medlemTräningspassController = medlemTräningspassController;
            LaddaSpecialiseringFilter();
            LaddaTräningspass();
            LaddaTider();
            _medlemTräningspassController = medlemTräningspassController;
        }
        private void LaddaSpecialiseringFilter()
        {
            var specialiseringar = _tränareController.HämtaSpecialisering();
            specialiseringar.Insert(0, "Alla");
            SpecialiseringComboBox.ItemsSource = specialiseringar;
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void LaddaTräningspass()
        {
            try
            {
                _träningspass = _träningspassController.HämtaAllaTräningspass().ToList();
                string valdSpecialisering = SpecialiseringComboBox.SelectedItem?.ToString() ?? "";

                if (valdSpecialisering == "Alla")
                {
                    TräningspassDataGrid.ItemsSource = _träningspass;
                }
                else
                {
                    TräningspassDataGrid.ItemsSource = _träningspass
                        .Where(t => t.Aktivitet != null && t.Aktivitet.Equals(valdSpecialisering, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel inträffade vid laddning av tränare");
            }
        }

        private void SpecialiseringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaddaTräningspass();
        }

        private void LaddaTider()
        {
            var tider = _träningspassController.HämtaTiderFörAktivitet();
            TidComboBox.ItemsSource = tider;
        }

        private void TräningspassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TräningspassDataGrid.SelectedItem is Träningspass valdTräningspass)
            {
                _valdträningspass = valdTräningspass;
                string aktivitet = valdTräningspass.Aktivitet;
                var filtreradeTränare = _tränareController.HämtaAllaTränare().Where(T => T.Specialisering != null && T.Specialisering.Equals(aktivitet, StringComparison.OrdinalIgnoreCase)).ToList();
                TränareComboBox.ItemsSource = filtreradeTränare;
                TränareComboBox.DisplayMemberPath = "Namn";
                TränareComboBox.SelectedValuePath = "TränareID";
                TränareComboBox.SelectedValue = valdTräningspass.TränareID;

                var tillgängligaplatser = _träningspassController.HämtaLokalerFörAktivitet(aktivitet);
                PlatsComboBox.ItemsSource = tillgängligaplatser;

                Datumpicker.SelectedDate = valdTräningspass.Datum;
                TidComboBox.SelectedItem = valdTräningspass.Tid.ToString(@"hh\:mm");
            }
        }

        private void SparaButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdträningspass == null)
            {
                MessageBox.Show("Vänligen välj ett träningspass att redigera");
            }

            try
            {
                int nyTränareID = (int)TränareComboBox.SelectedValue;
                string nyPlats = PlatsComboBox.SelectedItem?.ToString();
                DateTime nyttDatum = Datumpicker.SelectedDate ?? _valdträningspass.Datum;
                TimeSpan nyTid = TimeSpan.Parse(TidComboBox.SelectedItem.ToString());

                if (_valdträningspass.TränareID != nyTränareID || _valdträningspass.Datum != nyttDatum || _valdträningspass.Tid != nyTid)
                {
                    if (!_träningspassController.ÄrTränareTillgänglig(nyTränareID, nyttDatum, nyTid))
                    {
                        MessageBox.Show("Den valda tränaren är inte tillgänglig vid denna tid.");
                        return;
                    }
                }
                if (_valdträningspass.Plats != nyPlats || _valdträningspass.Datum != nyttDatum || _valdträningspass.Tid != nyTid)
                {
                    if (!_träningspassController.ÄrPlatsTillgänglig(nyPlats, nyttDatum, nyTid))
                    {
                        MessageBox.Show("Den valda platsen är redan bokad vid denna tid.");
                        return;
                    }
                }

                var uppdateratTräningspass = new Träningspass
                {
                    TräningspassID = _valdträningspass.TräningspassID,
                    TränareID = nyTränareID,
                    Aktivitet = _valdträningspass.Aktivitet,
                    Datum = nyttDatum,
                    Tid = nyTid,
                    Plats = nyPlats
                };

                _träningspassController.RedigeraTräningspass(_valdträningspass.TräningspassID, uppdateratTräningspass);
                MessageBox.Show("Träningspasset har uppdaterats!");
                LaddaTräningspass();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod vid uppdateringen: {ex.Message}");
            }
        }


        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TräningspasshanteringWindow träningspasshanteringWindow = new TräningspasshanteringWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            träningspasshanteringWindow.Show();
            this.Close();
        }
    }
}
