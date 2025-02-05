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
    /// Interaction logic for UtrustningsrapportWindow.xaml
    /// </summary>
    public partial class UtrustningsrapportWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemTräningspassController _medlemTräningspassController;
        private readonly UtrustningController _utrustningController;
        public UtrustningsrapportWindow(TräningspassController träningspassController, TränareController tränareController, UnitOfWork unitOfWork, MedlemTräningspassController medlemTräningspassController, UtrustningController utrustningController)
        {
            InitializeComponent();
            _träningspassController = träningspassController;
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _medlemTräningspassController = medlemTräningspassController;
            _utrustningController = utrustningController;
            LaddaRapport();
        }

        private void LaddaRapport()
        {
            var saknadutrustning = _utrustningController.HämtaSaknadUtrustning().ToList();
            var trasigutrustning = _utrustningController.HämtaTrasigUtrustning().ToList();

            TrasigListBox.ItemsSource = trasigutrustning;
            SaknadListBox.ItemsSource = saknadutrustning;
        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            RapporterWindow rapporterWindow = new RapporterWindow(_träningspassController, _tränareController, _unitOfWork, _medlemTräningspassController, _utrustningController);
            rapporterWindow.Show();
            this.Close();
        }
    }
}
