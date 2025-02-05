using AffärsLager;
using DataLager;
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
    /// Interaction logic for TränarhanteringWindow.xaml
    /// </summary>
    public partial class TränarhanteringWindow : Window
    {
        private readonly TränareController _tränareController;
        private readonly SäkerhetsController _säkerhetsController;
        private readonly UnitOfWork _unitOfWork;

        public TränarhanteringWindow(TränareController tränareController, SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _tränareController = tränareController;
            _säkerhetsController = säkerhetsController;
            _unitOfWork = unitOfWork;
        }

        private void LäggtilltränareButton_Click(object sender, RoutedEventArgs e)
        {
            LäggtilltränareWindow läggtilltränareWindow = new LäggtilltränareWindow(_tränareController, _säkerhetsController, _unitOfWork);
            läggtilltränareWindow.Show();
            this.Close();
        }

        private void UppdateratränareButton_Click(object sender, RoutedEventArgs e)
        {
            UppdateratränareWindow uppdateratränareWindow = new UppdateratränareWindow(_tränareController, _säkerhetsController, _unitOfWork);
            uppdateratränareWindow.Show();
            this.Close();

        }

        private void TaborttränareButton_Click(object sender, RoutedEventArgs e)
        {
            TaborttränareWindow taborttränareWindow = new TaborttränareWindow(_tränareController, _säkerhetsController, _unitOfWork);
            taborttränareWindow.Show();
            this.Close();
        }

        private void VisaallatränareButton_Click(object sender, RoutedEventArgs e)
        {
            VisaallatränareWindow visaallatränareWindow = new VisaallatränareWindow(_tränareController, _säkerhetsController, _unitOfWork);
            visaallatränareWindow.Show();
            this.Close();
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalMenyWindow personalMenyWindow = new PersonalMenyWindow(_unitOfWork);
            personalMenyWindow.Show();
            this.Close();
        }
    }
}
