using Knb.UI.Wpf.Pages;
using Microsoft.Extensions.Logging;
using Serilog;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Knb.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Frame MainContainer { get; }
        protected Page SimulatorPage { get; }
        protected Page TrainerPage { get; }
        protected Page FilesPage { get; }
        protected ILogger<MainWindow> Logger { get; }
        protected Button DisabledBtn { get; set; }
        protected IList<Button> DisabledBtnList { get; } = new List<Button>();
        public MainWindow(
            ILogger<MainWindow> logger, 
            SimulatorPage simulatorPage, 
            TrainerPage trainerPage, 
            FilesPage filesPage)
        {            
            logger.LogError("This is for testing");
            
            InitializeComponent();

            Logger = logger ?? throw new ArgumentNullException();
            MainContainer = Frm_mainContainer ?? throw new ArgumentNullException();
            SimulatorPage = simulatorPage ?? throw new ArgumentNullException();
            TrainerPage = trainerPage ?? throw new ArgumentNullException();
            FilesPage = filesPage ?? throw new ArgumentNullException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainContainer.Content = SimulatorPage;
            Btn_simulator.IsEnabled = false;
            DisabledBtn = Btn_simulator;
        }

        private void Btn_trainer_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            Btn_simulator.IsEnabled = true;
            //Btn_files.IsEnabled = true;
            MainContainer.Content = TrainerPage;
        }

        private void Btn_simulator_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            Btn_trainer.IsEnabled = true;
            //Btn_files.IsEnabled = true;
            MainContainer.Content = SimulatorPage;
        }

        private void Btn_files_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            Btn_simulator.IsEnabled = true;
            Btn_trainer.IsEnabled = true;
            MainContainer.Content = FilesPage;
        }
    }
}
