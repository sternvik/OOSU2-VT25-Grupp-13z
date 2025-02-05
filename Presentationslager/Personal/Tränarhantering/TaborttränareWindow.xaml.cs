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

namespace Presentationslager.Personal.Tränarhantering
{
    /// <summary>
    /// Interaction logic for TaborttränareWindow.xaml
    /// </summary>
    public partial class TaborttränareWindow : Window
    {

        private readonly TränareController _tränareController;
        private readonly UnitOfWork _unitOfWork;
        private readonly SäkerhetsController _säkerhetsController;
        private List<Tränare> _tränares;
        private Tränare _valdtränare;

        public TaborttränareWindow(TränareController tränareController, SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _säkerhetsController = säkerhetsController;
            LaddaTänare();
        }
        private void LaddaTänare()
        {
            _tränares = _unitOfWork.TränareRepository.GetAll().ToList();
            TränareDataGrid.ItemsSource = _tränares;
        }

        private void TränareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TränareDataGrid.SelectedItem is Tränare valdTränare)
            {
                _valdtränare = valdTränare;
                ValdtränareTextBox.Text = valdTränare.Namn;
            }
        }

        private void TabortButton_Click(object sender, RoutedEventArgs e)
        {
            if (_valdtränare == null)
            {
                MessageBox.Show("Du måste välja en tränare att ta bort!");
                return;
            }

            _tränareController.TaBortTränare(_valdtränare.TränareID);
            MessageBox.Show("Tränare Borttagen!");
            LaddaTänare();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            TränarhanteringWindow tränarhanteringWindow = new TränarhanteringWindow(_tränareController, _säkerhetsController, _unitOfWork);
            tränarhanteringWindow.Show();
            this.Close();
        }
    }
}
