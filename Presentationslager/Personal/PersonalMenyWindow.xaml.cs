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
using AffärsLager;
using DataLager;
using EntitetsLager;
using Presentationslager.Personal.Rapporter;
using Presentationslager.Personal.Tränarhantering;
using Presentationslager.Personal.Träningspasshantering;
using Presentationslager.Personal.Utlåningshatering;
using Presentationslager.Personal.Utrustningshantering;

namespace Presentationslager
{
    /// <summary>
    /// Interaction logic for PersonalMenyWindow.xaml
    /// </summary>
    public partial class PersonalMenyWindow : Window
    {
        private readonly MedlemController medlemsController;
        private readonly UtrustningController utrustningController;
        private readonly UtlåningController utlåningController;
        private readonly TräningspassController träningspassController;
        private readonly UnitOfWork _unitOfWork;
        private readonly SäkerhetsController säkerhetsController;
        private readonly TränareController tränareController;
        private readonly MedlemTräningspassController medlemTräningspassController;


        public PersonalMenyWindow(UnitOfWork unitOfWork)
        {

            InitializeComponent();
            _unitOfWork = unitOfWork;

            medlemsController = new MedlemController(_unitOfWork);
            utrustningController = new UtrustningController(_unitOfWork);
            utlåningController = new UtlåningController(_unitOfWork);
            träningspassController = new TräningspassController(_unitOfWork);
            säkerhetsController = new SäkerhetsController(_unitOfWork);
            tränareController = new TränareController(_unitOfWork);
            medlemTräningspassController = new MedlemTräningspassController (_unitOfWork);
        }

        private void MedlemshanteringButton_Click(object sender, RoutedEventArgs e)
        {
            MedlemshanteringWindow medlemshanteringWindow = new MedlemshanteringWindow(medlemsController, _unitOfWork);
            medlemshanteringWindow.Show();
            this.Close();
        }

        private void UtrustningshanteringButton_Click(object sender, RoutedEventArgs e)
        {
            UtrustningshanteringWindow utrustningshanteringWindow = new UtrustningshanteringWindow(utrustningController, _unitOfWork);
            utrustningshanteringWindow.Show();
            this.Close();
        }

        private void TränarhanteringButton_Click(object sender, RoutedEventArgs e)
        {
            TränarhanteringWindow tränarhanteringWindow = new TränarhanteringWindow(tränareController, säkerhetsController, _unitOfWork);
            tränarhanteringWindow.Show();
            this.Close();
        }

        private void UtlåningshanteringButton_Click(object sender, RoutedEventArgs e)
        {
            UtlåningshanteringWindow utlåningshanteringWindow = new UtlåningshanteringWindow(utlåningController, _unitOfWork, utrustningController, medlemsController);
            utlåningshanteringWindow.Show();
            this.Close();
        }

        private void TräningspasshanteringButton_Click(object sender, RoutedEventArgs e)
        {
            TräningspasshanteringWindow träningspasshanteringWindow = new TräningspasshanteringWindow(träningspassController, _unitOfWork, tränareController, medlemsController, medlemTräningspassController);
            träningspasshanteringWindow.Show();
            this.Close();
        }

        private void RapporterButton_Click(object sender, RoutedEventArgs e)
        {
            RapporterWindow rapporterWindow = new RapporterWindow(träningspassController, tränareController, _unitOfWork, medlemTräningspassController, utrustningController);
            rapporterWindow.Show();
            this.Close();
        }

        private void AvslutaButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
