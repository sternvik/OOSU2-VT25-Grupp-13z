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

namespace Presentationslager.Personal.Utrustningshantering
{
    /// <summary>
    /// Interaction logic for UppdaterautrustningWindow.xaml
    /// </summary>
    public partial class UppdaterautrustningWindow : Window
    {
        private readonly UtrustningController _utrustningController;
        private readonly UnitOfWork _unitOfWork;
        private Utrustning _valdUtrustning;

        public UppdaterautrustningWindow(UtrustningController utrustningController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _utrustningController = utrustningController;
            LaddaSkick();
            LaddaKategoriFilter();
            LaddaKategori();
        }


        private void LaddaSkick()
        {
            SkickComboBox.ItemsSource = _utrustningController.HämtaSkick();
            SkickComboBox.SelectedIndex = 0;
        }

        private void LaddaKategori()
        {
            KategoriComboBox.ItemsSource = _utrustningController.HämtaKategorier();
            KategoriComboBox.SelectedIndex = 0;
        }

        private void LaddaKategoriFilter()
        {
            var kategorier = _utrustningController.HämtaKategorier();
            kategorier.Insert(0, "Alla");
            KategorifilterComboBox.ItemsSource = kategorier;
            KategorifilterComboBox.SelectedIndex = 0;
        }
        private void TillgängligaTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void KategorifilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valdKategori = KategorifilterComboBox.SelectedItem?.ToString() ?? "";

            if (valdKategori == "Alla")
            {
                LaddaUtrustning();
            }
            else
            {
                LaddaUtrustning(valdKategori);
            }
        }

        private void LaddaUtrustning(string valdKategori = null)
        {
            try
            {
                List<Utrustning> utrustningfiltrerad = _utrustningController.HämtaAllUtrustning().ToList();

                if (!string.IsNullOrEmpty(valdKategori) && valdKategori != "Alla")
                {
                    utrustningfiltrerad = utrustningfiltrerad
                        .Where(u => u.Kategori != null && u.Kategori.Equals(valdKategori, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                UtrustningDataGrid.ItemsSource = utrustningfiltrerad;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel inträffade vid laddning av utrustning: {ex.Message}");
            }
        }

        private void UtrustningDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UtrustningDataGrid.SelectedItem is Utrustning valdUtrustning)
            {
                _valdUtrustning = valdUtrustning;

                NamnTextBox.Text = _valdUtrustning.Namn;
                KategoriComboBox.SelectedItem = _valdUtrustning.Kategori;
                SkickComboBox.SelectedItem = _valdUtrustning.Skick;
                TillgängligaTextBox.Text = _valdUtrustning.Tillgängliga.ToString();
            }
            else
            {
                MessageBox.Show("Välj en utrustning att redigera.");
                return;
            }
        }

        private void SparaButton_Click(object sender, RoutedEventArgs e)
        {
            _valdUtrustning.Namn = string.IsNullOrWhiteSpace(NamnTextBox.Text) ? _valdUtrustning.Namn : NamnTextBox.Text;
            _valdUtrustning.Kategori = string.IsNullOrWhiteSpace(KategoriComboBox.Text) ? _valdUtrustning.Kategori : KategoriComboBox.Text;
            _valdUtrustning.Skick = string.IsNullOrWhiteSpace(SkickComboBox.Text) ? _valdUtrustning.Skick : SkickComboBox.Text;

            if (int.TryParse(TillgängligaTextBox.Text, out int tillgängliga))
            {
                _valdUtrustning.Tillgängliga = tillgängliga;
            }

            _utrustningController.UppdateraUtrustning(_valdUtrustning.UtrustningID, _valdUtrustning);
            MessageBox.Show("Utrustning uppdaterad!");

            LaddaUtrustning();

        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            UtrustningshanteringWindow utrustningshanteringWindow = new UtrustningshanteringWindow(_utrustningController, _unitOfWork);
            utrustningshanteringWindow.Show();
            this.Close();
        }
    }
}
