using AffärsLager;
using DataLager;
using EntitetsLager;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentationslager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SäkerhetsController _säkerhetsController;
        private readonly UnitOfWork _unitOfWork;
        private readonly TränareController _tränareController;

        public MainWindow()
        {
            InitializeComponent();

            ApplikationDbContext applikationDbContext = new ApplikationDbContext();

            applikationDbContext.Database.EnsureCreated();

            _unitOfWork = new UnitOfWork(applikationDbContext);
            _unitOfWork.Fill();

            _säkerhetsController = new SäkerhetsController(_unitOfWork);
            _tränareController = new TränareController(_unitOfWork);
        }

        private void LoggainButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginwindow = new LoginWindow(_säkerhetsController, _unitOfWork);
            loginwindow.Show();
            this.Close();
        }

        private void RegistreraButton_Click(object sender, RoutedEventArgs e)
        {
            RegistreraWindow registrerawindow = new RegistreraWindow(_säkerhetsController, _unitOfWork, _tränareController);
            registrerawindow.Show();
            this.Close();
        }

        private void AvslutaButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

