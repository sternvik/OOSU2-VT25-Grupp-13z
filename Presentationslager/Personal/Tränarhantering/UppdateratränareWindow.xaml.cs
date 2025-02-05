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
using EntitetsLager;

namespace Presentationslager.Personal.Tränarhantering
{
    /// <summary>
    /// Interaction logic for UppdateratränareWindow.xaml
    /// </summary>
    public partial class UppdateratränareWindow : Window
    {
        private readonly TränareController _tränareController;
        private readonly UnitOfWork _unitOfWork;
        private readonly SäkerhetsController _säkerhetsController;
        private List<Tränare> _tränares;
        private Tränare _valdtränare;

        public UppdateratränareWindow(TränareController tränareController, SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _säkerhetsController = säkerhetsController;
            LaddaSpecialisering();
            LaddaTränare();
        }

        private void LaddaTränare()
        {
            _tränares = _tränareController.HämtaAllaTränare().ToList();
            TränareDataGrid.ItemsSource = _tränares;
        }

        private void LaddaSpecialisering()
        {
            SpecialiseringComboBox.ItemsSource = _tränareController.HämtaSpecialisering();
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void TränareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TränareDataGrid.SelectedItem is Tränare valdTränare)
            {
                _valdtränare = valdTränare;
                NamnTextBox.Text = valdTränare.Namn;
                SpecialiseringComboBox.SelectedItem = valdTränare.Specialisering;
                LösenordBox.Password = valdTränare.Lösenord;
                BekräftaLösenordBox.Password = valdTränare.Lösenord;
            }
        }

        private void SparaButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdtränare == null)
            {
                MessageBox.Show("Vänligen välj en tränare att uppdatera.");
                return;
            }

            if (LösenordBox.Password != BekräftaLösenordBox.Password)
            {
                MessageBox.Show("Lösenorden matchar inte. Kontrollera att du har skrivit samma lösenord i båda fälten.");
                return;
            }

            _valdtränare.Namn = string.IsNullOrWhiteSpace(NamnTextBox.Text) ? _valdtränare.Namn : NamnTextBox.Text;
            _valdtränare.Specialisering = SpecialiseringComboBox.SelectedItem.ToString();
            _valdtränare.Lösenord = string.IsNullOrWhiteSpace(LösenordBox.Password) ? _valdtränare.Lösenord : LösenordBox.Password;

            _tränareController.UppdateraTränare(_valdtränare.TränareID, _valdtränare);
            MessageBox.Show("Tränare uppdaterad!");

            LaddaTränare();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TränarhanteringWindow tränarhanteringWindow = new TränarhanteringWindow(_tränareController, _säkerhetsController, _unitOfWork);
            tränarhanteringWindow.Show();
            this.Close();
        }
    }
}
