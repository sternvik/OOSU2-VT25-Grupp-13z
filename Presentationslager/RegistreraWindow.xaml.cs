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

namespace Presentationslager
{
    /// <summary>
    /// Interaction logic for RegistreraWindow.xaml
    /// </summary>
    public partial class RegistreraWindow : Window
    {
        private readonly SäkerhetsController _säkerhetsController;
        private readonly UnitOfWork _unitOfWork;
        private readonly TränareController _tränareController;


        public RegistreraWindow(SäkerhetsController säkerhetsController, UnitOfWork unitOfWork, TränareController tränareController)
        {
            InitializeComponent();
            _säkerhetsController = säkerhetsController;
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            LaddaSpecialisering();
        }

        private void LaddaSpecialisering()
        {
            SpecialiseringComboBox.ItemsSource = _tränareController.HämtaSpecialisering();
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void RegistreraButton_Click(object sender, RoutedEventArgs e)
        {
            string namn = NamnTextBox.Text;
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

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
