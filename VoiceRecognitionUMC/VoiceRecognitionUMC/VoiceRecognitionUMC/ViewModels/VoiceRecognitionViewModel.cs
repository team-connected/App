using Prism.Navigation;
using VoiceRecognitionUMC.Dependency;
using Prism.Commands;
using Xamarin.Forms;
using System;
using VoiceRecognitionUMC.Persistence;
using VoiceRecognitionUMC.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace VoiceRecognitionUMC.ViewModels
{
    class VoiceRecognitionViewModel : BaseViewModel
    {
        #region MEMBERS
        private ISpeechToText _speechRecognitionInstance;
        private INavigationService _navigationService;
        private IMetricService _metricService;
        private List<string> metrics;
        private string userId;
        private Patient patient;
        private ObservableCollection<DeviceListItem> devices;
        #endregion

        #region COMMANDS
        public DelegateCommand StartRecordingCommand { get; private set; }
        public DelegateCommand ResetMetricCommand { get; private set; }
        public DelegateCommand FinishMetricCommand { get; private set; }
        #endregion

        #region CONSTRUCTOR
        public VoiceRecognitionViewModel(INavigationService navigationService) : base(navigationService)
        {
            StartRecordingCommand = new DelegateCommand(StartButtonClicked);
            ResetMetricCommand = new DelegateCommand(ResetMetric);
            FinishMetricCommand = new DelegateCommand(FinishMetric);
            _navigationService = navigationService;
            _metricService = new MetricService();
            metrics = new List<string>();

            DateTime now = DateTime.Now;
            Console.WriteLine(now.ToString("yyyy/MM/dd HH:mm:ss"));

            try
            {
                _speechRecognitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                var toastConfig = new ToastConfig("Er is iets mis gegaan bij het opnemen. Neem contact op wanneer dit blijft gebeuren");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.Firebrick);

                UserDialogs.Instance.Toast(toastConfig);
            }

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultReceived(args);
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultReceived(args);
            });
        }
        #endregion

        #region FUNCTIONS
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.userId = parameters.GetValue<string>("userId");
            this.devices = parameters.GetValue<ObservableCollection<DeviceListItem>>("deviceList");
            this.patient = parameters.GetValue<Patient>("patient");

            
            StartListening();
        }

        private void SpeechToTextFinalResultReceived(string args)
        {
            foreach (string keyWord in App.voiceRecognitionKeyWords)
            {
                if (args.ToLower().Contains(keyWord))
                {
                    string reworkedString = args.Substring(args.ToLower().IndexOf(keyWord));
                    string[] splittedVoiceText = Regex.Split(reworkedString, keyWord);

                    foreach (string line in splittedVoiceText)
                    {
                        if (line != "")
                        {
                            metrics.Add(line);
                        }
                    }

                    foreach (string s in metrics)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            

            StartListening();
        }

        private void StartListening()
        {
            try
            {
                _speechRecognitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                var toastConfig = new ToastConfig("Er is iets mis gegaan bij het opnemen. Neem contact op wanneer dit blijft gebeuren");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.Firebrick);

                UserDialogs.Instance.Toast(toastConfig);
            }
        }

        private void StartButtonClicked()
        {
            try
            {
                _speechRecognitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                var toastConfig = new ToastConfig("Er is iets mis gegaan bij het opnemen. Neem contact op wanneer dit blijft gebeuren");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.Firebrick);

                UserDialogs.Instance.Toast(toastConfig);
            }
        }

        private void ResetMetric()
        {
            metrics.Clear();
            metrics = new List<string>();

            StartListening();
        }

        private async void FinishMetric()
        {
            DateTime now = DateTime.Now;
            
            string deviceBloeddruk = "";
            string deviceGewicht = "";
            string deviceTemperatuur = "";

            foreach (DeviceListItem device in devices)
            {
                if (device.DeviceType.ToLower() == "bloeddrukmeters")
                {
                    deviceBloeddruk = device.DeviceId;
                }
                if (device.DeviceType.ToLower() == "thermometer")
                {
                    deviceTemperatuur = device.DeviceId;
                }
                if (device.DeviceType.ToLower() == "weegschaal")
                {
                    deviceGewicht = device.DeviceId;
                }
            }


            MetricCreate newMetric = new MetricCreate
            {
                device_bloeddruk = deviceBloeddruk,
                device_gewicht = deviceGewicht,
                device_temperatuur = deviceTemperatuur,
                metric_type = "",
                nurse_id = userId,
                timestamp = now.ToString("yyyy/MM/dd HH:mm:ss"),
                comment = "Voice Test"
            };

            UserDialogs.Instance.ShowLoading("Opslaan");

            var metricResponse = await _metricService.CreateMetric(newMetric, patient._id);

            foreach (string metric in metrics)
            {
                Metric updateMetric = new Metric
                {
                    raw_text = metric,
                    metric_id = metricResponse.createMetric
                };

                await _metricService.SaveMetric(updateMetric);
            }

            var navigationParams = new NavigationParameters
            {
                { "metricId", metricResponse.createMetric },
                {"patientId",  patient._id}
            };

            UserDialogs.Instance.HideLoading();
            await _navigationService.NavigateAsync("../MetricResult", navigationParams);
        }
        #endregion
    }
}

 