using AffärsLager;
using DataLager;
using EntitetsLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Presentationslager.Personal.Tränarhantering
{
    public partial class VisaallatränareWindow : Window
    {
        private readonly TränareController _tränareController;
        private readonly UnitOfWork _unitOfWork;
        private readonly SäkerhetsController _säkerhetsController;
        private List<Tränare> _tränares;

        public VisaallatränareWindow(TränareController tränareController, SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _säkerhetsController = säkerhetsController;
            LaddaSpecialiseringFilter();
            LaddaTränare();
        }

        private void LaddaSpecialiseringFilter()
        {
            var specialiseringar = _tränareController.HämtaSpecialisering();
            specialiseringar.Insert(0, "Alla");
            SpecialiseringComboBox.ItemsSource = specialiseringar;
            SpecialiseringComboBox.SelectedIndex = 0;
        }

        private void LaddaTränare()
        {
            try
            {
                _tränares = _tränareController.HämtaAllaTränare().ToList();
                string valdSpecialisering = SpecialiseringComboBox.SelectedItem?.ToString() ?? "";

                if (valdSpecialisering == "Alla")
                {
                    TränareDataGrid.ItemsSource = _tränares;
                }
                else
                {
                    TränareDataGrid.ItemsSource = _tränares
                        .Where(t => t.Specialisering != null && t.Specialisering.Equals(valdSpecialisering, StringComparison.OrdinalIgnoreCase))
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
            LaddaTränare();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TränarhanteringWindow tränarhanteringWindow = new TränarhanteringWindow(_tränareController, _säkerhetsController, _unitOfWork);
            tränarhanteringWindow.Show();
            this.Close();
        }
    }
}
