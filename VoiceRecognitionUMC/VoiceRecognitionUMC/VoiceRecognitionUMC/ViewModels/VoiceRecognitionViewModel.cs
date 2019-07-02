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
using System.Linq;
using System.Collections.ObjectModel;

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
            this.patient = parameters.GetValue<Patient>("patient");
            this.devices = parameters.GetValue<ObservableCollection<DeviceListItem>>("deviceList");

            Speak("U kunt beginnen met de meting");

            StartListening();
        }

        private void SpeechToTextFinalResultReceived(string args)
        {
            if (App.voiceRecognitionStopWords.Any(w => args.ToLower().Contains(w)))
            {
                GetUserAgreement();
            }
            else if (App.voiceRecogntionAcceptWords.Any(w => args.ToLower().Contains(w)))
            {
                FinishMetric();
            }
            else if (App.voiceRecogntionCancelWords.Any(w => args.ToLower().Contains(w)))
            {
                CancelFinishMetric();
            }
            else if (App.metricNumber.Any(w => args.ToLower().Contains(w)))
            {
                if (args.ToLower().Contains("een") || args.ToLower().Contains("1"))
                {
                    RemoveAndRestartMetric(1);
                }
                if (args.ToLower().Contains("twee") || args.ToLower().Contains("2"))
                {
                    RemoveAndRestartMetric(2);
                }
                if (args.ToLower().Contains("drie") || args.ToLower().Contains("3"))
                {
                    RemoveAndRestartMetric(3);
                }
                
            }
            else
            {
                GetMetricsFromSpokenText(args);
            }
        }

        private void GetUserAgreement()
        {
            Speak("Ik heb de volgende metingen gehoord");
            System.Threading.Thread.Sleep(2000);
            foreach (string metric in metrics)
            {
                Speak(metric);
                System.Threading.Thread.Sleep(3000);
            }
            Speak("Is dit correct en meting afronden?");
            System.Threading.Thread.Sleep(2000);

            StartListening();
        }

        private void CancelFinishMetric()
        {
            Speak("Spreek alstublieft het nummer van de foutieve meting of spreek een nieuwe meting in");
            System.Threading.Thread.Sleep(4000);

            StartListening();
        }

        private void RemoveAndRestartMetric(int number)
        {
            try
            {
                metrics.RemoveAt(number - 1);
            }
            catch { }
            
            System.Threading.Thread.Sleep(1000);
            Speak("Spreek de meting opnieuw");

            StartListening();
        } 

        private void GetMetricsFromSpokenText(string spokenText)
        {
            foreach (string keyWord in App.voiceRecognitionKeyWords)
            {
                if (spokenText.ToLower().Contains(keyWord))
                {
                    string reworkedString = spokenText.Substring(spokenText.ToLower().IndexOf(keyWord));
                    string[] splittedVoiceText = Regex.Split(reworkedString, keyWord);

                    foreach (string line in splittedVoiceText)
                    {
                        string newLine;
                        try
                        {
                            if (line.Contains("temperatuur"))
                            {
                                string toBeSearched = "is    graden";
                                newLine = string.Format("{0}", line.Remove(line.IndexOf("is") + toBeSearched.Length));
                                metrics.Add(newLine);
                            }
                            if (line.Contains("bloeddruk"))
                            {
                                string toBeSearched = "is     over   ";
                                newLine = string.Format("{0}", line.Remove(line.IndexOf("is") + toBeSearched.Length));
                                metrics.Add(newLine);
                            }
                            if (line.Contains("gewicht") && line.Contains("kg"))
                            {
                                string toBeSearched = "is    kg";
                                newLine = string.Format("{0}", line.Remove(line.IndexOf("is") + toBeSearched.Length));
                                metrics.Add(newLine);
                            }
                            if (line.Contains("gewicht") && line.Contains("kilo"))
                            {
                                string toBeSearched = "is    kilo";
                                newLine = string.Format("{0}", line.Remove(line.IndexOf("is") + toBeSearched.Length));
                                metrics.Add(newLine);
                            }
                        }
                        catch
                        {
                            if (line != "")
                            {
                                metrics.Add(line);
                            }
                        }
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

        private void Speak(string textToSpeak)
        {
            DependencyService.Get<ITextToSpeech>().Speak(textToSpeak);
        }
        #endregion
    }
}

 