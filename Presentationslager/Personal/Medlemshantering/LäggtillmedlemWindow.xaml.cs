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
using DataLager;
using EntitetsLager;

namespace Presentationslager.Personal.Medlemshantering
{
    /// <summary>
    /// Interaction logic for LäggtillmedlemWindow.xaml
    /// </summary>
    public partial class LäggtillmedlemWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly MedlemController _medlemController;

        public LäggtillmedlemWindow(MedlemController medlemController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _medlemController = medlemController;
        }

        private void LäggtillmedlemButton_Click(object sender, RoutedEventArgs e)
        {
            string namn = NamnTextbox.Text;
            DateTime födelsedatum = FödelsedatumPick.SelectedDate.Value;
            string telefonummer = TelefonnummerTextBox.Text;
            string epost = EpostTextBox.Text;

            if (string.IsNullOrWhiteSpace(namn) || födelsedatum == null ||
                string.IsNullOrWhiteSpace(telefonummer) || string.IsNullOrWhiteSpace(epost))
            {
                MessageBox.Show("Alla fält måste fyllas i!");
                return;
            }

            string resultat = _medlemController.LäggTillMedlem(namn, födelsedatum, telefonummer, epost);

            MessageBox.Show(resultat);
            if (resultat == "Medlem har lagts till!")
            {
                return;
            }
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            MedlemshanteringWindow medlemshanteringWindow = new MedlemshanteringWindow(_medlemController, _unitOfWork);
            medlemshanteringWindow.Show();
            this.Close();
        }
    }
}
