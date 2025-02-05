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

namespace Presentationslager.Personal.Utlåningshatering
{
    /// <summary>
    /// Interaction logic for SkapautlåningWindow.xaml
    /// </summary>
    public partial class SkapautlåningWindow : Window
    {
        private readonly UtlåningController _utlåningController;
        private readonly UnitOfWork _unitOfWork;
        private readonly UtrustningController _utrustningController;
        private readonly MedlemController _medlemController;
        private List<Medlem> allaMedlemmar;
        private List<Utrustning> allUtrustning;
        private Medlem _valdmedlem;
        private Utrustning _valdutrustning;

        public SkapautlåningWindow(UtlåningController utlåningController, UnitOfWork unitOfWork,
            UtrustningController utrustningController, MedlemController medlemController)
        {
            InitializeComponent();
            _utlåningController = utlåningController;
            _unitOfWork = unitOfWork;
            _medlemController = medlemController;
            _utrustningController = utrustningController;

            LaddaMedlemmar();
            LaddaTillgängligUtrustning();

        }

        private void LaddaMedlemmar()
        {
            allaMedlemmar = _medlemController.HämtaAllaMedlemmar().ToList();
            MedlemmarDataGrid.ItemsSource = allaMedlemmar;
        }

        private void MedlemmarDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MedlemmarDataGrid.SelectedItem is Medlem valdMedlem)
            {
                _valdmedlem = valdMedlem;
                ValdmedlemTextBox.Text = valdMedlem.Namn;
            }
        }

        private void LaddaTillgängligUtrustning()
        {
            allUtrustning = _utrustningController.HämtaTillgängligUtrustning().ToList();
            UtrustningDataGrid.ItemsSource = allUtrustning;
        }

        private void UtrustningDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UtrustningDataGrid.SelectedItem is Utrustning valdUtrustning)
            {
                _valdutrustning = valdUtrustning;
                ValdutrustningTextBox.Text = _valdutrustning.Namn;
            }
        }

        private void SparaLånButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MedlemmarDataGrid.SelectedItem is not Medlem valdMedlem)
                {
                    MessageBox.Show("Vänligen välj en medlem.");
                    return;
                }
                if (UtrustningDataGrid.SelectedItem is not Utrustning valdUtrustning)
                {
                    MessageBox.Show("Vänligen välj en utrustning.");
                    return;
                }
                if (Utlåningsdatumpicker.SelectedDate is not DateTime utlåningsdatum)
                {
                    MessageBox.Show("Vänligen välj ett utlåningsdatum.");
                    return;
                }

                DateTime? återlämningsdatum = Återlämningsdatumpicker.SelectedDate;

                _utlåningController.RegistreraUtlåning(valdMedlem.MedlemID, valdUtrustning.UtrustningID, utlåningsdatum, återlämningsdatum);
                MessageBox.Show($"Utrustningen '{valdUtrustning.Namn}' har lånats ut till {valdMedlem.Namn}.", "Utlåning registrerad", MessageBoxButton.OK, MessageBoxImage.Information);
                LaddaTillgängligUtrustning();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fel");
            }
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            UtlåningshanteringWindow utlåningshanteringWindow = new UtlåningshanteringWindow(_utlåningController, _unitOfWork, _utrustningController, _medlemController);
            utlåningshanteringWindow.Show();
            this.Close();
        }
    }
}
