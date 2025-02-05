using AffärsLager;
using DataLager;
using Presentationslager.Personal.Medlemshantering;
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

namespace Presentationslager
{
    /// <summary>
    /// Interaction logic for MedlemshanteringWindow.xaml
    /// </summary>
    public partial class MedlemshanteringWindow : Window
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly MedlemController _medlemController;

        public MedlemshanteringWindow(MedlemController medlemcontroller, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _medlemController = medlemcontroller;
        }

        private void LäggtillmedlemButton_Click(object sender, RoutedEventArgs e)
        {
            LäggtillmedlemWindow läggtillmedlemWindow = new LäggtillmedlemWindow(_medlemController, _unitOfWork);
            läggtillmedlemWindow.Show();
            this.Close();
        }

        private void UppdateraButton_Click(object sender, RoutedEventArgs e)
        {
            UppdateramedlemWindow uppdateramedlemWindow = new UppdateramedlemWindow(_medlemController, _unitOfWork);
            uppdateramedlemWindow.Show();
            this.Close();
        }

        private void TabortmedlemButton_Click(object sender, RoutedEventArgs e)
        {
            TabortmedlemWindow tabortmedlemWindow = new TabortmedlemWindow(_medlemController, _unitOfWork);
            tabortmedlemWindow.Show();
            this.Close();
        }

        private void VisamedlemstatusButton_Click(object sender, RoutedEventArgs e)
        {
            VisamedlemstatusWindow visamedlemstatusWindow = new VisamedlemstatusWindow(_medlemController, _unitOfWork);
            visamedlemstatusWindow.Show();
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
