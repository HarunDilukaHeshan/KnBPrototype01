using Knb.App.Controllers;
using System;
using System.Linq;
using System.Collections.Generic;
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
using System.IO;
using Knb.UI.Wpf.Utilities.InputDialogBox;
using Knb.App.Trainer.Components;
using Knb.App.Trainer.Shared;

namespace Knb.UI.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for TrainerPage.xaml
    /// </summary>
    public partial class TrainerPage : Page
    {
        protected DataFilesStorageController DataFilesStorageCtrl { get; }
        protected ModelFilesStorageController ModelFilesStorageCtrl { get; }
        protected PDataFilesStorageController PDataFilesStorageCtrl { get; }
        protected TrainerController TrainerCtrl { get; }
        protected bool HasStarted { get; set; } = false;
        public TrainerPage(
            TrainerController trainerCtrl,
            DataFilesStorageController dataFilesStorageCtrl, 
            ModelFilesStorageController modelFilesStorageCtrl, 
            PDataFilesStorageController pDataFilesStorageCtrl)
        {
            DataFilesStorageCtrl = dataFilesStorageCtrl ?? throw new ArgumentNullException();
            ModelFilesStorageCtrl = modelFilesStorageCtrl ?? throw new ArgumentNullException();
            PDataFilesStorageCtrl = pDataFilesStorageCtrl ?? throw new ArgumentNullException();
            TrainerCtrl = trainerCtrl ?? throw new ArgumentNullException();

            InitializeComponent();
        }

        private async Task Init()
        {
            await LoadFileInfoAsync();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await Init();
        }

        private async void Btn_startTrainer_Click(object sender, RoutedEventArgs e)
        {
            if (HasStarted)
            {
                TrainerCtrl.Stop();
                ((Button)sender).IsEnabled = false;
            }
            else
            {
                ((Button)sender).Content = "Stop";
                await StartAsync();
                ((Button)sender).Content = "Start";
            }
        }

        private void EnableControls(bool isEnabled = true)
        {
            Cmb_dataFileName.IsEnabled = isEnabled;
            Cmb_processedDataFileName.IsEnabled = isEnabled;
            Cmb_testDataFileName.IsEnabled = isEnabled;
            Cmb_trainingAlg.IsEnabled = isEnabled;
            Btn_addNewPDataFile.IsEnabled = isEnabled;
            Txt_modelFileName.IsEnabled = isEnabled;
        }

        private async Task StartAsync()
        {
            var dataFileName = Cmb_dataFileName.SelectedValue.ToString();
            var pDataFileName = Cmb_processedDataFileName.SelectedValue.ToString();
            var modelFileName = Txt_modelFileName.Text;
            var testPDataFileName = Cmb_testDataFileName.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(dataFileName) || string.IsNullOrWhiteSpace(pDataFileName)
                || string.IsNullOrWhiteSpace(modelFileName) || string.IsNullOrWhiteSpace(testPDataFileName))
                throw new InvalidOperationException("Carefully select all the required values");

            try
            {
                EnableControls(false);
                if (ModelFilesStorageCtrl.Exists(modelFileName)) throw new InvalidOperationException("FAE");
                if (!DataFilesStorageCtrl.Exists(dataFileName)) throw new InvalidOperationException("DFDNE");
                if (!DataFilesStorageCtrl.Exists(testPDataFileName)) throw new InvalidOperationException("TDFDNE");

                if (dataFileName == testPDataFileName) throw new InvalidOperationException("SameName");

                TrainerCtrl.OnPhaseChangedEvent += OnTrainerPhaseChanged;
                TrainerCtrl.OnPreprocessorChanged += OnPreprocessorChanged;

                await TrainerCtrl.Start(options =>
                {
                    options.DataFileName = dataFileName;
                    options.ProcessedDataFileName = pDataFileName;
                    options.MLModelFileName = modelFileName;
                    options.TestDataFileName = testPDataFileName;
                });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "FAE")
                {
                    MessageBox.Show(
                        "ML file already exists. Use a different name",
                        "File already exists",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                }
                else if (ex.Message == "DFDNE")
                {
                    MessageBox.Show(
                        "Data file does not exists",
                        "File not found",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                }
                else if (ex.Message == "TDFDNE")
                {
                    MessageBox.Show(
                        "Test data file does not exists",
                        "File not found",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                }
                else if (ex.Message == "SameName")
                {
                    MessageBox.Show(
                        "Data file and the test data file cannot have the same name",
                        "File not found",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                }
                else
                {
                    MessageBox.Show("Unexpected error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void Btn_addNewPDataFile_Click(object sender, RoutedEventArgs e)
        {
            var result = InputDialogBox.Show("Please enter the name of the new data file!", "My title");

            if (result != null && result.ClickedBtn == InputDialogBoxButton.Okay
                && !string.IsNullOrWhiteSpace(result.InputString))
            {
                await PDataFilesStorageCtrl.CreatePDataFile(result.InputString);
                await LoadFileInfoAsync();
            }
        }

        private async Task LoadFileInfoAsync()
        {
            var dataFileInfoArr = await DataFilesStorageCtrl.GetDataFilesInfoAsync();
            var pDataFilesInfoArr = await PDataFilesStorageCtrl.GetFilesInfoAsync();

            Cmb_dataFileName.ItemsSource = dataFileInfoArr.Select(e => e.FileName).ToArray();
            Cmb_testDataFileName.ItemsSource = dataFileInfoArr.Select(e => e.FileName).ToArray();
            Cmb_processedDataFileName.ItemsSource = pDataFilesInfoArr.Select(e => e.FileName).ToArray();
        }

        protected void OnTrainerPhaseChanged(TrainerPhases phase)
        {
            Dispatcher.Invoke(() =>
            {
                if (phase == TrainerPhases.Stopped)
                {
                    HasStarted = false;
                    Btn_startTrainer.IsEnabled = true;
                    Btn_startTrainer.Content = "Start";
                }                

                Lbl_pLabel.Content = phase.ToString();
            });
        }

        protected void OnPreprocessorChanged(PreprocessorTracker tracker)
        {
            Dispatcher.Invoke(() =>
            {
                Pb_trainerPb.Value = tracker.Completed;
                Pb_trainerPb.Maximum = tracker.TaskSize;
            });
        }
    }
}
