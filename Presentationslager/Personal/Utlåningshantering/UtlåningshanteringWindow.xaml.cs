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

namespace Presentationslager.Personal.Utlåningshatering
{
    /// <summary>
    /// Interaction logic for UtlåningshanteringWindow.xaml
    /// </summary>
    public partial class UtlåningshanteringWindow : Window
    {
        private readonly UtlåningController _utlåningController;
        private readonly UnitOfWork _unitOfWork;
        private readonly UtrustningController _utrustningController;
        private readonly MedlemController _medlemController;

        public UtlåningshanteringWindow(UtlåningController utlåningController, UnitOfWork unitOfWork,
            UtrustningController utrustningController, MedlemController medlemController)
        {
            InitializeComponent();
            _utlåningController = utlåningController;
            _unitOfWork = unitOfWork;
            _medlemController = medlemController;
            _utrustningController = utrustningController;

        }

        private void SkapautlåningButton_Click(object sender, RoutedEventArgs e)
        {
            SkapautlåningWindow skapautlåningWindow = new SkapautlåningWindow(_utlåningController, _unitOfWork, _utrustningController, _medlemController);
            skapautlåningWindow.Show();
            this.Close();

        }

        private void ÅterlämnaUtlåningButton_Click(object sender, RoutedEventArgs e)
        {
            ÅterlämnautlåningWindow återlämnautlåningWindow = new ÅterlämnautlåningWindow(_utlåningController, _unitOfWork, _utrustningController, _medlemController);
            återlämnautlåningWindow.Show();
            this.Close();
        }

        private void VisaallaButton_Click(object sender, RoutedEventArgs e)
        {
            VisaallautlåningarWindow visaallautlåningarWindow = new VisaallautlåningarWindow(_utlåningController, _unitOfWork, _utrustningController, _medlemController);
            visaallautlåningarWindow.Show();
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
