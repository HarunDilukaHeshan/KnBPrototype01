using Knb.App.Simulator.GameData;
using System;
using System.Linq;
using System.Collections.Generic;
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
using Knb.UI.Wpf.Utilities.InputDialogBox;
using Knb.App.Controllers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Knb.App.Simulator.Shared.GameData;

namespace Knb.UI.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for SimulatorPage.xaml
    /// </summary>
    public partial class SimulatorPage : Page
    {
        protected DataFilesStorageController DataFilesStorageCtrl { get; }
        protected ModelFilesStorageController ModelFilesStorageController { get; }
        protected KnbOptions KnbOptions { get; }
        protected SimulatorController SimulatorController { get; }
        protected bool HasStarted { get; set; } = false;
        public SimulatorPage(
            SimulatorController simulatorController,
            IOptions<KnbOptions> knbOptions,
            DataFilesStorageController dataFilesStorageController, 
            ModelFilesStorageController modelFilesStorageController)
        {
            DataFilesStorageCtrl = dataFilesStorageController ?? throw new ArgumentNullException();
            ModelFilesStorageController = modelFilesStorageController ?? throw new ArgumentNullException();
            KnbOptions = knbOptions.Value ?? throw new ArgumentNullException();
            SimulatorController = simulatorController ?? throw new ArgumentNullException();

            InitializeComponent();
            Init();            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFileInfoAsync();
        }

        private void Init()
        {                        
            cmb_noOfTimes.ItemsSource = Enum.GetValues(typeof(NoOfTimes)).Cast<int>().Select(e => e.ToString("N0"));
            cmb_noOfPlayers.ItemsSource = Enum.GetValues(typeof(NoOfPlayers)).Cast<int>().ToArray();
            cmb_noOfCardPacks.ItemsSource = Enum.GetValues(typeof(NoOfCardPacks)).Cast<int>().ToArray();
            cmb_noOfTimesPerPeriod.ItemsSource = Enum.GetValues(typeof(NoOfTimesPerPeriod)).Cast<int>().Select(e => e.ToString("N0"));

            cmb_noOfTimes.SelectedItem = ((int)KnbOptions.NoOfTimes).ToString("N0");
            cmb_noOfPlayers.SelectedItem = ((int)KnbOptions.NoOfPlayers);
            cmb_noOfCardPacks.SelectedItem = ((int)KnbOptions.NoOfCardPacks);
            cmb_noOfTimesPerPeriod.SelectedItem = ((int)KnbOptions.NoOfTimes).ToString("N0");

            SimulatorController.OnPeriodChange += OnPeriodChangeHandler;
            SimulatorController.OnPlayFinished += OnPlayFinishedHandler;
            SimulatorController.OnSimulatorStopped += OnSimulatorStoppedHandler;
        }
        
        private async Task LoadFileInfoAsync()
        {
            var fileInfoArr = await DataFilesStorageCtrl.GetDataFilesInfoAsync();
            var modelFilesInfoArr = await ModelFilesStorageController.GetModelFilesInfoAsync();

            var lst = modelFilesInfoArr.Select(e => e.FileName).ToList();
            lst.Add("DefaultSelector");

            cmb_dataFileName.ItemsSource = fileInfoArr.Select(e => e.FileName).ToArray();
            cmb_modelFileName.ItemsSource = lst;
            cmb_modelFileName.SelectedIndex = lst.Count - 1;
        }

        private async void CreateDataFile(object sender, RoutedEventArgs e)
        {
            var result = InputDialogBox.Show("Please enter the name of the new data file!", "My title");

            if (result != null && result.ClickedBtn == InputDialogBoxButton.Okay
                && !string.IsNullOrWhiteSpace(result.InputString))
            {
                await DataFilesStorageCtrl.CreateDataFileAsync(result.InputString);
                await LoadFileInfoAsync();
            }
        }

        private async void Btn_startSimulator_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = ((Button)sender);
                if (HasStarted)
                {
                    SimulatorController.Stop();
                    btn.IsEnabled = false;
                }
                else
                {
                    HasStarted = true;
                    btn.Content = "Stop";
                    SetIsEnableProperty(false);

                    await StartSimulator();

                    HasStarted = false;
                    btn.Content = "Start";
                    SetIsEnableProperty(true);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Please carefully fill all the reaquired fields",
                    "Knb",
                    MessageBoxButton.OK);
                HasStarted = false;
                Btn_startSimulator.Content = "Start";
                SetIsEnableProperty(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected problem occured", "Error", MessageBoxButton.OK);
            }
        }

        private async Task StartSimulator()
        {
            var noOfTimes = GetEnumVal<NoOfTimes>(cmb_noOfTimes.SelectedValue);
            var noOfPlayers = GetEnumVal<NoOfPlayers>(cmb_noOfPlayers.SelectedValue);
            var noOfCardPacks = GetEnumVal<NoOfCardPacks>(cmb_noOfCardPacks.SelectedValue);
            var noOfTimesPerPeriod = GetEnumVal<NoOfTimesPerPeriod>(cmb_noOfTimesPerPeriod.SelectedValue);
            var dataFileName = cmb_dataFileName.SelectedValue ?? throw new InvalidOperationException("Data fileName cannot be null");
            var modelFileName = cmb_modelFileName.SelectedValue ?? "";
            modelFileName = (modelFileName.ToString() == "DefaultSelector") ? "" : modelFileName.ToString();
            
            Pb_gameData.Value = 0;
            Lbl_gameDataPb.Content = "Generating...";
            HasStarted = true;
            SetIsEnableProperty(false);

            await SimulatorController.Start(options =>
            {
                options.NoOfTimes = noOfTimes;
                options.NoOfPlayers = noOfPlayers;
                options.NoOfCardPacks = noOfCardPacks;
                options.NoOfTimesPerPeriod = noOfTimesPerPeriod;

                options.FileName = dataFileName.ToString();
                options.MlModelFileName = modelFileName.ToString();
            });
        }

        private TEnum GetEnumVal<TEnum>(object value)
            where TEnum : Enum
        {
            try
            {
                var strVal = value.ToString().Replace(",", "");
                return (TEnum)Enum.Parse(typeof(TEnum), strVal);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Invalid enum value");
            }
        }

        private void OnPeriodChangeHandler(int period, int noOfPeriods)
        {
            Dispatcher.Invoke(() =>
            {
                Pb_gameData.Maximum = noOfPeriods;
                Pb_gameData.Value = period + 1;
            });
        }

        private void OnPlayFinishedHandler(NoOfTimes noOfTimes, NoOfTimesPerPeriod noOfTimesPerPeriod)
        {
            Dispatcher.Invoke(() =>
            {
                Lbl_gameDataPb.Content = "Completed.";
                SetIsEnableProperty(true);
                HasStarted = false;
            });
        }

        private void OnSimulatorStoppedHandler()
        {
            Dispatcher.Invoke(() => {
                Lbl_gameDataPb.Content = "Stopped.";
                HasStarted = false;
                Btn_startSimulator.IsEnabled = true;
                SetIsEnableProperty(true);
            });
        }

        private void SetIsEnableProperty(bool isEnable)
        {
            cmb_noOfTimes.IsEnabled = isEnable;
            cmb_noOfPlayers.IsEnabled = isEnable;
            cmb_noOfCardPacks.IsEnabled = isEnable;
            cmb_noOfTimesPerPeriod.IsEnabled = isEnable;
            cmb_dataFileName.IsEnabled = isEnable;
            cmb_modelFileName.IsEnabled = isEnable;
            Btn_AddDataFile.IsEnabled = isEnable;
        }
    }
}
