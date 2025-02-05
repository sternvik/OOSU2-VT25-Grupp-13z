using AffärsLager;
using DataLager;
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
    /// Interaction logic for RegistrerautrustningWindow.xaml
    /// </summary>
    public partial class RegistrerautrustningWindow : Window
    {
        private readonly UtrustningController _utrustningController;
        private readonly UnitOfWork _unitOfWork;

        public RegistrerautrustningWindow(UtrustningController utrustningController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _utrustningController = utrustningController;
            _unitOfWork = unitOfWork;
            LaddaSkick();
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

        private void TillgängligaTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void RegistreraButton_Click(object sender, RoutedEventArgs e)
        {
            string namn = NamnTextbox.Text;
            string kategori = KategoriComboBox.Text;
            string skick = SkickComboBox.Text;

            if (!int.TryParse(TillgängligaTextBox.Text, out int tillgängliga) || tillgängliga < 0)
            {
                MessageBox.Show("Ange ett giltigt antal (heltal, minst 0)!");
                return;
            }

            if (string.IsNullOrWhiteSpace(namn) || string.IsNullOrWhiteSpace(kategori) || string.IsNullOrWhiteSpace(skick))
            {
                MessageBox.Show("Alla fält måste fyllas i!");
                return;
            }

            _utrustningController.RegistreraUtrustning(namn, kategori, skick, tillgängliga);
            MessageBox.Show("Utrustning registrerad!");
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            UtrustningshanteringWindow utrustningshanteringWindow = new UtrustningshanteringWindow(_utrustningController, _unitOfWork);
            utrustningshanteringWindow.Show();
            this.Close();
        }
    }
}
