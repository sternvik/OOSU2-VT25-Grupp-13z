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
    /// Interaction logic for SkapaträningspassWindow.xaml
    /// </summary>
    public partial class SkapaträningspassWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemController _medlemController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private List<Tränare> _tränares;
        private Tränare _valdtränare;

        public SkapaträningspassWindow(TräningspassController träningspassController, UnitOfWork unitOfWork, TränareController tränareController, MedlemController medlemController, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _medlemController = medlemController;
            _träningspassController = träningspassController;
            _medlemTräningspassController = medlemTräningspassController;
            LaddaSpecialiseringFilter();
            LaddaTränare();
            LaddaPlats();
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

        private void LaddaTränare()
        {
            try
            {
                _tränares = _tränareController.HämtaAllaTränare().ToList();
                string valdSpecialisering = SpecialiseringComboBox.SelectedItem?.ToString() ?? "";

                if (valdSpecialisering == "Alla")
                {
                    TränareDataGrid.ItemsSource = _tränares;
                }
                else
                {
                    TränareDataGrid.ItemsSource = _tränares
                        .Where(t => t.Specialisering != null && t.Specialisering.Equals(valdSpecialisering, StringComparison.OrdinalIgnoreCase))
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
            LaddaTränare();
        }

        private void LaddaPlats()
        {
            string valdAktivitet = AktivitetTextBox.Text;

            var lokaler = _träningspassController.HämtaLokalerFörAktivitet(valdAktivitet);
            if (lokaler.Any())
            {
                PlatsComboBox.ItemsSource = lokaler;
            }
            else
            {
                PlatsComboBox.ItemsSource = new List<string> { "Inga lokaler tillgängliga för denna aktivitet." };
            }
        }

        private void TränareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TränareDataGrid.SelectedItem is Tränare valdTränare)
            {
                NamnTextBox.Text = valdTränare.Namn;
                AktivitetTextBox.Text = valdTränare.Specialisering;
                LaddaPlats();
            }
        }

        private void LaddaTider()
        {
            var tider = _träningspassController.HämtaTiderFörAktivitet();
            StarttidComboBox.ItemsSource = tider;
        }

        private void SparaButton_Click(object sender, RoutedEventArgs e)
        {
            if (TränareDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vänligen välj en tränare.");
                return;
            }

            if (StarttidComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vänligen välj en starttid.");
                return;
            }

            if (PlatsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vänligen välj en plats.");
                return;
            }

            var valdTränare = (Tränare)TränareDataGrid.SelectedItem;
            var aktivitet = valdTränare.Specialisering;
            var starttid = StarttidComboBox.SelectedItem.ToString();
            var datum = StarttidDatePicker.SelectedDate ?? DateTime.Now;
            var plats = PlatsComboBox.SelectedItem.ToString();

            var startHour = int.Parse(starttid.Split(':')[0]);
            var startDateTime = new DateTime(datum.Year, datum.Month, datum.Day, startHour, 0, 0);

            try
            {
                _träningspassController.SkapaTräningspass(aktivitet, datum, startDateTime.TimeOfDay, plats, valdTränare.TränareID);
                MessageBox.Show("Träningspasset har skapats.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel vid skapande av träningspass: {ex.Message}");
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
