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
    /// Interaction logic for VisaallautlåningarWindow.xaml
    /// </summary>
    public partial class VisaallautlåningarWindow : Window
    {
        private readonly UtlåningController _utlåningController;
        private readonly UnitOfWork _unitOfWork;
        private readonly UtrustningController _utrustningController;
        private readonly MedlemController _medlemController;
        public VisaallautlåningarWindow(UtlåningController utlåningController, UnitOfWork unitOfWork,
            UtrustningController utrustningController, MedlemController medlemController)
        {
            InitializeComponent();
            _utlåningController = utlåningController;
            _unitOfWork = unitOfWork;
            _medlemController = medlemController;
            _utrustningController = utrustningController;
            LaddaStatus();
        }

        private void LaddaStatus()
        {
            UtlåningstatusComboBox.Items.Add("Alla");
            UtlåningstatusComboBox.Items.Add("Aktiva");
            UtlåningstatusComboBox.Items.Add("Historik");
            UtlåningstatusComboBox.SelectedIndex = 0;
        }
        private void UtlåningstatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaddaUtlåningar();
        }

        private void LaddaUtlåningar()
        {
            List<Utlåning> allaUtlåningar = _utlåningController.HämtaALlaUtlåningar().ToList();
            List<Utlåning> aktivaUtlåningar = _utlåningController.HämtaAllaAktivaUtlåningar().ToList();
            List<Utlåning> arkiveradeUtlåningar = _utlåningController.HämtaArkiveradeUtlånignar().ToList();
            string valdStatus = UtlåningstatusComboBox.SelectedItem.ToString();

            List<Utlåning> filtreradeUtlåningar;

            if (valdStatus == "Aktiva")
            {
                filtreradeUtlåningar = aktivaUtlåningar;
            }
            else if (valdStatus == "Historik")
            {
                filtreradeUtlåningar = arkiveradeUtlåningar;
            }
            else
            {
                filtreradeUtlåningar = allaUtlåningar;
            }

            UtlåningarDataGrid.ItemsSource = filtreradeUtlåningar;
        }

        private void UtlåningarDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UtlåningarDataGrid.SelectedItem is Utlåning utlåning)
            {
                var medlem = _medlemController.HämtaAllaMedlemmar().FirstOrDefault(m => m.MedlemID == utlåning.MedlemID);
                var utrustning = _utrustningController.HämtaTillgängligUtrustning().FirstOrDefault(u => u.UtrustningID == utlåning.UtrustningID);

                ValdmedlemTextBox.Text = medlem.Namn;
                ValdutrustningTextBox.Text = utrustning.Namn;
                UtlåningsdatumTextBox.Text = utlåning.UtLåningsdatum.ToString();


                if (utlåning.Återlämningsdatum != null)
                {
                    ÅterlämningsdatumTextBox.Text = utlåning.Återlämningsdatum.ToString();
                }
                else
                {
                    ÅterlämningsdatumTextBox.Text = "Ej återlämnad";
                }
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
