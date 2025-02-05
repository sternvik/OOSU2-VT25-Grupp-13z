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

namespace Presentationslager.Personal.Tränarhantering
{
    /// <summary>
    /// Interaction logic for LäggtilltränareWindow.xaml
    /// </summary>
    public partial class LäggtilltränareWindow : Window
    {
        private readonly TränareController _tränareController;
        private readonly UnitOfWork _unitOfWork;
        private readonly SäkerhetsController _säkerhetsController;
        public LäggtilltränareWindow(TränareController tränareController, SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _säkerhetsController = säkerhetsController;
            LaddaSpecialisering();
        }
        private void LaddaSpecialisering()
        {
            SpecialiseringComboBox.ItemsSource = _tränareController.HämtaSpecialisering();
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TränarhanteringWindow tränarhanteringWindow = new TränarhanteringWindow(_tränareController, _säkerhetsController, _unitOfWork);
            tränarhanteringWindow.Show();
            this.Close();
        }

        private void LäggtilltränareButton_Click(object sender, RoutedEventArgs e)
        {
            string namn = NamnTextbox.Text;
            string lösenord = LösenordBox.Password;
            string bekräftalösenord = BekräftaLösenordBox.Password;
            string specialisering = SpecialiseringComboBox.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(namn) || string.IsNullOrWhiteSpace(lösenord) || string.IsNullOrWhiteSpace(bekräftalösenord))
            {
                MessageBox.Show("Alla fält måste fyllas i.");
                return;
            }

            if (lösenord != bekräftalösenord)
            {
                MessageBox.Show("Lösenorden matchar inte. Kontrollera att du har skrivit samma lösenord i båda fälten.");
                return;
            }

            _säkerhetsController.SkapaTränare(namn, lösenord, specialisering);
            MessageBox.Show("Konto har skapats!");
        }
    }
}
