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

namespace Presentationslager.Personal.Rapporter
{
    /// <summary>
    /// Interaction logic for TräningspassrapportWindow.xaml
    /// </summary>
    public partial class TräningspassrapportWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private List<Träningspass> _träningspass;
        private Träningspass _valdträningspass;
        public TräningspassrapportWindow(TräningspassController träningspassController, TränareController tränareController, UnitOfWork unitOfWork, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _träningspassController = träningspassController;
            _tränareController = tränareController;
            _medlemTräningspassController = medlemTräningspassController;
            LaddaSpecialiseringFilter();
            LaddaTräningspass();
        }

        private void LaddaSpecialiseringFilter()
        {
            var specialiseringar = _tränareController.HämtaSpecialisering();
            specialiseringar.Insert(0, "Alla");
            SpecialiseringComboBox.ItemsSource = specialiseringar;
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void LaddaTräningspass()
        {
            try
            {
                _träningspass = _träningspassController.HämtaAllaTräningspass().ToList();
                string valdSpecialisering = SpecialiseringComboBox.SelectedItem?.ToString() ?? "";

                if (valdSpecialisering == "Alla")
                {
                    TräningspassDataGrid.ItemsSource = _träningspass;
                }
                else
                {
                    TräningspassDataGrid.ItemsSource = _träningspass
                        .Where(t => t.Aktivitet != null && t.Aktivitet.Equals(valdSpecialisering, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel inträffade vid laddning av tränare");
            }
        }

        private void LaddaDeltagare()
        {
            if (_valdträningspass == null) return;

            var deltagare = _medlemTräningspassController.HämtaDeltagareFörTräningspass(_valdträningspass.TräningspassID);
            DeltagareListBox.ItemsSource = deltagare;
            DeltagareListBox.DisplayMemberPath = "Namn";
        }

        private void TräningspassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TräningspassDataGrid.SelectedItem is Träningspass träningspass)
            {
                _valdträningspass = träningspass;
                LaddaDeltagare();

                TränareTextBox.Text = träningspass.Tränare?.Namn ?? "Ingen tränare";
                DatumTextBox.Text = träningspass.Datum.ToString("yyyy-MM-dd");
                TidTextBox.Text = träningspass.Tid.ToString(@"hh\:mm");
                PlatsTextBox.Text = träningspass.Plats;
            }
        }

        private void SpecialiseringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaddaTräningspass();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            RapporterWindow rapporterWindow = new RapporterWindow(_träningspassController, _tränareController, _unitOfWork, _medlemTräningspassController);
            rapporterWindow.Show();
            this.Close();
        }
    }
}
