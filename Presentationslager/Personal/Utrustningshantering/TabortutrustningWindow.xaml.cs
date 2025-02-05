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

    public partial class TabortutrustningWindow : Window
    {

        private readonly UtrustningController _utrustningController;
        private readonly UnitOfWork _unitOfWork;
        private Utrustning _valdUtrustning;

        public TabortutrustningWindow(UtrustningController utrustningController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _utrustningController = utrustningController;
            LaddaKategoriFilter();
        }

        private void LaddaKategoriFilter()
        {
            var kategorier = _utrustningController.HämtaKategorier();
            kategorier.Insert(0, "Alla");
            KategorifilterComboBox.ItemsSource = kategorier;
            KategorifilterComboBox.SelectedIndex = 0;
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
                ValdutrustningTextBox.Text = _valdUtrustning.Namn;
            }
        }

        private void TabortButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdUtrustning == null)
            {
                MessageBox.Show("Välj utrustning att ta bort!");
                return;
            }

            _utrustningController.TaBortUtrustning(_valdUtrustning.UtrustningID);
            MessageBox.Show("Utrustning borttagen!");

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
