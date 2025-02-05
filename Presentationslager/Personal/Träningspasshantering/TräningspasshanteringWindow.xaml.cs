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

namespace Presentationslager.Personal.Träningspasshantering
{
    /// <summary>
    /// Interaction logic for TräningspasshanteringWindow.xaml
    /// </summary>
    public partial class TräningspasshanteringWindow : Window
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly TräningspassController _träningspassController;
        private readonly TränareController _tränareController;
        private readonly MedlemController _medlemController;
        private readonly MedlemTräningspassController _medlemTräningspassController;

        public TräningspasshanteringWindow(TräningspassController träningspassController, UnitOfWork unitOfWork, TränareController tränareController, MedlemController medlemController, MedlemTräningspassController medlemTräningspassController)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _tränareController = tränareController;
            _medlemController = medlemController;
            _träningspassController = träningspassController;
            _medlemTräningspassController = medlemTräningspassController;
        }

        private void SkapaträningspassButton_Click(object sender, RoutedEventArgs e)
        {
            SkapaträningspassWindow skapaträningspassWindow = new SkapaträningspassWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            skapaträningspassWindow.Show();
            this.Close();
        }

        private void RedigeraträningspassButton_Click(object sender, RoutedEventArgs e)
        {
            RedigeraträningspassWindow redigeraträningspassWindow = new RedigeraträningspassWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            redigeraträningspassWindow.Show();
            this.Close();
        }

        private void TabortträningspassButton_Click(object sender, RoutedEventArgs e)
        {
            TabortträningspassWindow tabortträningspassWindow = new TabortträningspassWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            tabortträningspassWindow.Show();
            this.Close();
        }

        private void HanteraDeltagareButton_Click(object sender, RoutedEventArgs e)
        {
            HanteradeltagareWindow hanteradeltagareWindow = new HanteradeltagareWindow(_träningspassController, _unitOfWork, _tränareController, _medlemController, _medlemTräningspassController);
            hanteradeltagareWindow.Show();
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
