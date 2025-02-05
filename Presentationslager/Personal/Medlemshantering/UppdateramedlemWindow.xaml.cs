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
using AffärsLager;
using Azure;
using DataLager;
using EntitetsLager;

namespace Presentationslager.Personal.Medlemshantering
{
    /// <summary>
    /// Interaction logic for UppdateramedlemWindow.xaml
    /// </summary>
    public partial class UppdateramedlemWindow : Window
    {
        private readonly MedlemController _medlemController;
        private readonly UnitOfWork _unitOfWork;
        private List<Medlem> _medlemmar;
        private Medlem _valdmedlem;

        public UppdateramedlemWindow(MedlemController medlemController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _medlemController = medlemController;
            _unitOfWork = unitOfWork;
            LaddaMedlemmar();

            BetalstatusComboBox.Items.Add("Betald");
            BetalstatusComboBox.Items.Add("Obetald");
        }

        private void LaddaMedlemmar()
        {
            _medlemmar = _medlemController.HämtaAllaMedlemmar().ToList();
            MedlemmarDataGrid.ItemsSource = _medlemmar;
        }

        private void MedlemmarDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MedlemmarDataGrid.SelectedItem is Medlem valdMedlem)
            {
                _valdmedlem = valdMedlem;
                NamnTextBox.Text = valdMedlem.Namn;
                TelefonnummerTextBox.Text = valdMedlem.Telefonnummer;
                EpostTextBox.Text = valdMedlem.Epost;
                BetalstatusComboBox.SelectedItem = valdMedlem.Betalstatus ? "Betald" : "Obetald";


                if (valdMedlem.Födelse != DateTime.MinValue)
                {
                    FödelsePicker.SelectedDate = valdMedlem.Födelse;
                }

            }
        }

        private void SparaButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdmedlem == null)
            {
                MessageBox.Show("Välj en medlem att uppdatera");
                return;
            }

            _valdmedlem.Namn = string.IsNullOrWhiteSpace(NamnTextBox.Text) ? _valdmedlem.Namn : NamnTextBox.Text;
            _valdmedlem.Telefonnummer = string.IsNullOrWhiteSpace(TelefonnummerTextBox.Text) ? _valdmedlem.Telefonnummer : TelefonnummerTextBox.Text;
            _valdmedlem.Epost = string.IsNullOrWhiteSpace(EpostTextBox.Text) ? _valdmedlem.Epost : EpostTextBox.Text;

            if (FödelsePicker.SelectedDate.HasValue)
            {
                _valdmedlem.Födelse = FödelsePicker.SelectedDate.Value;
            }

            _valdmedlem.Betalstatus = BetalstatusComboBox.SelectedItem.ToString() == "Betald";

            _medlemController.UppdateraMedlem(_valdmedlem.MedlemID, _valdmedlem);
            MessageBox.Show("Medlem uppdaterad!");


            LaddaMedlemmar();

        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            MedlemshanteringWindow medlemshantering = new MedlemshanteringWindow(_medlemController, _unitOfWork);
            medlemshantering.Show();
            this.Close();
        }
    }
}
