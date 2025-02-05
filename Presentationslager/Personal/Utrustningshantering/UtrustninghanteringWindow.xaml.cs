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

namespace Presentationslager.Personal.Utrustningshantering
{
    /// <summary>
    /// Interaction logic for UtrustningshanteringWindow.xaml
    /// </summary>
    public partial class UtrustningshanteringWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UtrustningController _utrustningController;

        public UtrustningshanteringWindow(UtrustningController utrustningController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _utrustningController = utrustningController;
        }

        private void RegistreraButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrerautrustningWindow registrerautrustningWindow = new RegistrerautrustningWindow(_utrustningController, _unitOfWork);
            registrerautrustningWindow.Show();
            this.Close();
        }

        private void UppdateraButton_Click(object sender, RoutedEventArgs e)
        {
            UppdaterautrustningWindow uppdaterautrustningWindow = new UppdaterautrustningWindow(_utrustningController, _unitOfWork);
            uppdaterautrustningWindow.Show();
            this.Close();
        }

        private void TabortButton_Click(object sender, RoutedEventArgs e)
        {
            TabortutrustningWindow tabortutrustningWindow = new TabortutrustningWindow(_utrustningController, _unitOfWork);
            tabortutrustningWindow.Show();
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
