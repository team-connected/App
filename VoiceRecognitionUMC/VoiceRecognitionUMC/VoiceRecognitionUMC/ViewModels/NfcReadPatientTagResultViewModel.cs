using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcReadPatientTagResultViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private string patientName;
        private string email;
        private string address;
        private Patient scannedPatient;
        private INavigationParameters allParameters;

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand ProceedToNextPageCommand { get; private set; }

        public string PatientName
        {
            get { return this.patientName; }
            set { SetProperty(ref this.patientName, value); }
        }

        public string Email
        {
            get { return this.email; }
            set { SetProperty(ref this.email, value); }
        }
        public string Address
        {
            get { return this.address; }
            set { SetProperty(ref this.address, value); }
        }

        public NfcReadPatientTagResultViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            ProceedToNextPageCommand = new DelegateCommand(GoToVoiceRecognitionPage);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            allParameters = parameters;
            if (parameters.ContainsKey("patient"))
            {
                scannedPatient = parameters.GetValue<Patient>("patient");
                
                PatientName = $"{scannedPatient.Firstname} {scannedPatient.Lastname}";
                Email = scannedPatient.Email;
                Address = $"{scannedPatient.Street}\n{scannedPatient.City}\n{scannedPatient.Location}";
            }
        }

        private async void GoToVoiceRecognitionPage()
        {
            await _navigationService.NavigateAsync("VoiceRecognition", allParameters);
        }
    }
}
