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

namespace Presentationslager.Personal.Medlemshantering
{
    /// <summary>
    /// Interaction logic for TabortmedlemWindow.xaml
    /// </summary>
    public partial class TabortmedlemWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly MedlemController _medlemController;
        private List<Medlem> _medlemmar;
        private Medlem _valdmedlem;

        public TabortmedlemWindow(MedlemController medlemController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _medlemController = medlemController;
            _unitOfWork = unitOfWork;
            LaddaMedlemmar();
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
                ValdmedlemTextBox.Text = valdMedlem.Namn;
            }
        }

        private void TabortButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdmedlem == null)
            {
                MessageBox.Show("Välj en medlem att ta bort!");
                return;
            }

            _medlemController.TaBortMedlem(_valdmedlem.MedlemID);
            MessageBox.Show("Medlem Borttagen!");

            LaddaMedlemmar();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            MedlemshanteringWindow medlemshanteringWindow = new MedlemshanteringWindow(_medlemController, _unitOfWork);
            medlemshanteringWindow.Show();
            this.Close();
        }

    }
}
