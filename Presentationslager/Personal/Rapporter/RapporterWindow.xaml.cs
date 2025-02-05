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

namespace Presentationslager.Personal.Rapporter
{
    /// <summary>
    /// Interaction logic for RapporterWindow.xaml
    /// </summary>
    public partial class RapporterWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        public RapporterWindow(TräningspassController träningspassController, TränareController tränareController, UnitOfWork unitOfWork, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _träningspassController = träningspassController;
            _tränareController = tränareController;
            _medlemTräningspassController = medlemTräningspassController;
        }

        private void UtrustningsrapportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TräningspassrapportButton_Click(object sender, RoutedEventArgs e)
        {
            TräningspassrapportWindow träningspassrapportWindow = new TräningspassrapportWindow(_träningspassController, _tränareController, _unitOfWork, _medlemTräningspassController);
            träningspassrapportWindow.Show();
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
