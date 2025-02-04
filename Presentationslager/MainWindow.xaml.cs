using DataLager;
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
        private readonly UnitOfWork _unitOfWork;

        public MainWindow()
        {
            InitializeComponent();

            ApplikationDbContext applikationDbContext = new ApplikationDbContext();

            applikationDbContext.Database.EnsureCreated();

            _unitOfWork = new UnitOfWork(applikationDbContext);
            _unitOfWork.Fill();
        }
    }
}