using Prism.Navigation;
using VoiceRecognitionUMC.Dependency;
using Prism.Commands;
using Xamarin.Forms;
using System;
using VoiceRecognitionUMC.Persistence;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.ViewModels
{
    class VoiceRecognitionViewModel : BaseViewModel
    {
        #region MEMBERS
        private ISpeechToText _speechRecognitionInstance;
        private INavigationService _navigationService;
        private string _recognizedText;
        private bool _enabled = true;
        private IMetricService _metricService;
        #endregion

        #region PROPERTIES
        public string RecognizedText
        {
            get { return this._recognizedText; }
            set { SetProperty(ref _recognizedText, value); }
        }
        public bool Enabled
        {
            get { return this._enabled; }
            set { SetProperty(ref _enabled, value); }
        }
        #endregion

        #region COMMANDS
        public DelegateCommand StartRecordingCommand { get; private set; }
        #endregion

        #region CONSTRUCTOR
        public VoiceRecognitionViewModel(INavigationService navigationService) : base(navigationService)
        {
            StartRecordingCommand = new DelegateCommand(StartButtonClicked);
            _metricService = new MetricService();

            try
            {
                _speechRecognitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                RecognizedText = ex.Message;
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
        private void SpeechToTextFinalResultReceived(string args)
        {
            RecognizedText = args;
        }

        private async void StartButtonClicked()
        {
            /*try
            {
                _speechRecognitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                RecognizedText = ex.Message;
            }*/
            //_speechRecognitionInstance.StartSpeechToText();

            var newMetric = new MetricCreate();
            newMetric.timestamp = DateTime.Now;
            newMetric.device_id = "59ce02a3e87d42f3b467c161502c824e";
            newMetric.nurse_id = "72ddbd32092346a38ed89309aa06d3e5";
            newMetric.comment = "Test";

            var newMetricResponse = await _metricService.CreateMetric(newMetric, "ac3d09e21c974ab1a054ff2f74e9d42e");

            var updateMetric = new Metric();
            updateMetric.metric_id = newMetricResponse.createMetric;
            updateMetric.nurse_id = "72ddbd32092346a38ed89309aa06d3e5";
            updateMetric.patient_id = "ac3d09e21c974ab1a054ff2f74e9d42e";
            updateMetric.raw_text = "registreer bloeddruk is 120 over 80";

            await _metricService.SaveMetric(updateMetric);

        }
        #endregion
    }
}

 