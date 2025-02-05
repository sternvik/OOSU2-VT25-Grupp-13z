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
    /// Interaction logic for HanteradeltagareWindow.xaml
    /// </summary>
    public partial class HanteradeltagareWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemController _medlemController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private List<Träningspass> _träningspass;
        private Träningspass _valdträningspass;

        public HanteradeltagareWindow(TräningspassController träningspassController, UnitOfWork unitOfWork, TränareController tränareController, MedlemController medlemController, MedlemTräningspassController medlemTräningspassController)
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

        private void LaddaDeltagare()
        {
            if (_valdträningspass == null) return;

            var deltagare = _medlemTräningspassController.HämtaDeltagareFörTräningspass(_valdträningspass.TräningspassID);
            DeltagareListBox.ItemsSource = deltagare;
            DeltagareListBox.DisplayMemberPath = "Namn";


        }

        private void LaddaTillgängligaMedlemmar()
        {
            if (_valdträningspass == null) return;

            var allaMedlemmar = _medlemController.HämtaAllaMedlemmar();
            var redanDeltagare = _medlemTräningspassController.HämtaDeltagareFörTräningspass(_valdträningspass.TräningspassID);

            var tillgänligaMedlemmar = allaMedlemmar.Where(m => !redanDeltagare.Any(d => d.MedlemID == m.MedlemID));

            MedlemComboBox.ItemsSource = tillgänligaMedlemmar;
            MedlemComboBox.DisplayMemberPath = "Namn";
            MedlemComboBox.SelectedValuePath = "MedlemID";
        }

        private void SpecialiseringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaddaTräningspass();
        }

        private void TräningspassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TräningspassDataGrid.SelectedItem is Träningspass träningspass)
            {
                _valdträningspass = träningspass;
                LaddaDeltagare();
                LaddaTillgängligaMedlemmar();
            }
        }

        private void TaBortDeltagareButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdträningspass == null || DeltagareListBox.SelectedItem == null) return;

            var valdDeltagare = (Medlem)DeltagareListBox.SelectedItem;
            _medlemTräningspassController.TaBortMedlemFrånPass(valdDeltagare.MedlemID, _valdträningspass.TräningspassID);
            MessageBox.Show("Deltagare borttagen");
            LaddaDeltagare();
            LaddaTillgängligaMedlemmar();
        }

        private void LäggtillDeltagareButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdträningspass == null || MedlemComboBox.SelectedItem == null) return;

            var valdMedlem = (Medlem)MedlemComboBox.SelectedItem;
            _medlemTräningspassController.LäggTillMedlempåTräningspass(valdMedlem.MedlemID, _valdträningspass.TräningspassID);
            MessageBox.Show("Deltagare tillagd");
            LaddaDeltagare();
            LaddaTillgängligaMedlemmar();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TräningspasshanteringWindow träningspasshanteringWindow = new TräningspasshanteringWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            träningspasshanteringWindow.Show();
            this.Close();
        }
    }
}
