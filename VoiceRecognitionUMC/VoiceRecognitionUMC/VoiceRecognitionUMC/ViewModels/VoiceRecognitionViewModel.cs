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

namespace VoiceRecognitionUMC.ViewModels
{
    class VoiceRecognitionViewModel : BaseViewModel
    {
        #region MEMBERS
        private ISpeechToText _speechRecognitionInstance;
        private INavigationService _navigationService;
        private IMetricService _metricService;
        private List<string> metrics;
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

            StartListening();
        }
        #endregion

        #region FUNCTIONS
        private void SpeechToTextFinalResultReceived(string args)
        {
            if (args.ToLower().Contains("registreer"))
            {
                string reworkedString = args.Substring(args.ToLower().IndexOf("registreer"));
                string[] splittedVoiceText = Regex.Split(reworkedString, "registreer");

                foreach (string line in splittedVoiceText)
                {
                    string metricLine = "registreer" + line;
                    if (metricLine != "registreer")
                    {
                        metrics.Add(metricLine);
                    }
                }

                foreach (string s in metrics)
                {
                    Console.WriteLine(s);
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
            MetricCreate newMetric = new MetricCreate
            {
                device_id = "1391109f9910481c933970c5db966358",
                metric_type = "",
                nurse_id = "nv7sg3vrxrwr8y8vugamdztxzrhwemhj",
                timestamp = DateTime.Now,
                comment = "Voice Test"
            };

            var metricResponse = await _metricService.CreateMetric(newMetric, "43smekrqprd5ptjt1z5pwb9nxliz81nu");

            foreach (string metric in metrics)
            {
                Metric updateMetric = new Metric
                {
                    raw_text = metric,
                    patient_id = "43smekrqprd5ptjt1z5pwb9nxliz81nu",
                    nurse_id = "nv7sg3vrxrwr8y8vugamdztxzrhwemhj",
                    metric_id = metricResponse.createMetric
                };

                await _metricService.SaveMetric(updateMetric);
            }

            var navigationParams = new NavigationParameters
            {
                { "metricId", metricResponse.createMetric }
            };

            await _navigationService.NavigateAsync("../MetricResult", navigationParams);
        }
        #endregion
    }
}

 