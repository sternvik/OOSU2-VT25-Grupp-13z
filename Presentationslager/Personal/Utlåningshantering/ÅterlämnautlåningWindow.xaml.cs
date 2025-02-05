using AffärsLager;
using DataLager;
using EntitetsLager;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ÅterlämnautlåningWindow.xaml
    /// </summary>
    public partial class ÅterlämnautlåningWindow : Window
    {
        private readonly UtlåningController _utlåningController;
        private readonly UnitOfWork _unitOfWork;
        private readonly MedlemController _medlemController;
        private readonly UtrustningController _utrustningController;
        private List<Utlåning> _utlåningar;
        private Utlåning _valdUtlåning; //behövs denna? 

        public ÅterlämnautlåningWindow(UtlåningController utlåningController, UnitOfWork unitOfWork, UtrustningController trustningController, MedlemController medlemController)
        {
            InitializeComponent();
            _utlåningController = utlåningController;
            _unitOfWork = unitOfWork;
            _medlemController = medlemController;
            _utrustningController = trustningController;
            LaddaAktivautlåningar();
        }

        private void LaddaAktivautlåningar()
        {
            _utlåningar = _utlåningController.HämtaAllaAktivaUtlåningar().Where(u => u.Återlämningsdatum == null).ToList();
            AktivutlåningDataGrid.ItemsSource = _utlåningar;
        }

        private void AktivutlåningDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AktivutlåningDataGrid.SelectedItem is Utlåning utlåning)
            {
                var medlem = _medlemController.HämtaAllaMedlemmar().FirstOrDefault(m => m.MedlemID == utlåning.MedlemID);
                var utrustning = _utrustningController.HämtaTillgängligUtrustning().FirstOrDefault(u => u.UtrustningID == utlåning.UtrustningID);

                ValdmedlemTextBox.Text = medlem.Namn;
                ValdutrustningTextBox.Text = utrustning.Namn;
            }
        }

        private void SparaåterlämningButton_Clic(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AktivutlåningDataGrid.SelectedItem is not Utlåning valdUtlåning)
                {
                    MessageBox.Show("Vänligen välj en utlåning.");
                    return;
                }
                if (Återlämningdatumpicker.SelectedDate is not DateTime Återlämningsdatum)
                {
                    MessageBox.Show("Vänligen välj ett återlämningsdatum.");
                    return;
                }

                _utlåningController.RegistreraÅterlämning(valdUtlåning.MedlemID, valdUtlåning.UtrustningID, Återlämningsdatum);
                MessageBox.Show("Utlåning Återlämnad");
                LaddaAktivautlåningar();
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
