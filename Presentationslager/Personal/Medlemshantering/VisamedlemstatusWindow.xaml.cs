using AffärsLager;
using DataLager;
using EntitetsLager;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    /// Interaction logic for VisamedlemstatusWindow.xaml
    /// </summary>
    public partial class VisamedlemstatusWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly MedlemController _medlemController;
        public VisamedlemstatusWindow(MedlemController medlemController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _medlemController = medlemController;
            _unitOfWork = unitOfWork;
            LaddaStatus();
            LaddaMedlemmar();
        }

        private void LaddaStatus()
        {
            BetalstatusComboBox.Items.Add("Alla");
            BetalstatusComboBox.Items.Add("Betald");
            BetalstatusComboBox.Items.Add("Obetald");
            BetalstatusComboBox.SelectedIndex = 0;
        }

        private void LaddaMedlemmar()
        {
            List<Medlem> allaMedlemmar = _medlemController.HämtaAllaMedlemmar().ToList();
            string valdStatus = BetalstatusComboBox.SelectedItem.ToString();

            if (valdStatus == "Betald")
            {
                MedlemmarDataGrid.ItemsSource = allaMedlemmar.Where(m => m.Betalstatus).ToList();
            }
            else if (valdStatus == "Obetald")
            {
                MedlemmarDataGrid.ItemsSource = allaMedlemmar.Where(m => !m.Betalstatus).ToList();
            }
            else
            {
                MedlemmarDataGrid.ItemsSource = allaMedlemmar;
            }

        }

        private void BetalstatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
