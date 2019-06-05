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
        private IPatientService _patientService;
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
            _patientService = new PatientService();

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
            _speechRecognitionInstance.StartSpeechToText();

        }
        #endregion
    }
}

 