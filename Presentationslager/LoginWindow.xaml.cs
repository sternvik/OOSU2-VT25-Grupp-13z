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

namespace Presentationslager
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly SäkerhetsController _säkerhetsController;
        private readonly UnitOfWork _unitOfWork;

        public LoginWindow(SäkerhetsController säkerhetsController, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _säkerhetsController = säkerhetsController;
            _unitOfWork = unitOfWork;
        }

        private void LoggainButton_Click(object sender, RoutedEventArgs e)
        {
            string användarnamn = AnvändarenamnTextBox.Text;
            string lösenord = LösenordBox.Password;

            bool ärAutentiserad = _säkerhetsController.LoggaIn(användarnamn, lösenord);

            if (ärAutentiserad)
            {
                MessageBox.Show("Inloggning lyckades!");
                PersonalMenyWindow personalMenyWindow = new PersonalMenyWindow(_unitOfWork);
                personalMenyWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösnord!");
            }

        }

        private void TillbakaButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
