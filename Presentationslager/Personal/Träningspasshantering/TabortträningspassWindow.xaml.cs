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

namespace Presentationslager.Personal.Träningspasshantering
{
    /// <summary>
    /// Interaction logic for TabortträningspassWindow.xaml
    /// </summary>
    public partial class TabortträningspassWindow : Window
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemController _medlemController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private List<Träningspass> _träningspass;
        private Träningspass _valdträningspass;

        public TabortträningspassWindow(TräningspassController träningspassController, UnitOfWork unitOfWork, TränareController tränareController, MedlemController medlemController, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _medlemController = medlemController;
            _träningspassController = träningspassController;
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

        private void SpecialiseringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaddaTräningspass();
        }

        private void TräningspassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TräningspassDataGrid.SelectedItem is Träningspass valdTräningspass)
            {
                _valdträningspass = valdTräningspass;
                ValdTräningspassTextBox.Text = valdTräningspass.Aktivitet;
            }
        }

        private void TabortButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdträningspass == null)
            {
                MessageBox.Show("Vänligen välj ett träningspass att ta bort");
                return;
            }

            _träningspassController.TaBortTräningspass(_valdträningspass.TräningspassID);
            MessageBox.Show("Träningspass borttaget");

            LaddaTräningspass();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TräningspasshanteringWindow träningspasshanteringWindow = new TräningspasshanteringWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            träningspasshanteringWindow.Show();
            this.Close();
        }
    }
}
