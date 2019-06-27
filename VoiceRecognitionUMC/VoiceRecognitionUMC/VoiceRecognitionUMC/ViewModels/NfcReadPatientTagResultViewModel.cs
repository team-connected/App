using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcReadPatientTagResultViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private string patientName;

        public string PatientName
        {
            get { return this.patientName; }
            set { SetProperty(ref this.patientName, value); }
        }

        public NfcReadPatientTagResultViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("patientFullName"))
            {
                PatientName = parameters.GetValue<string>("patientFullName");
                Debug.WriteLine(PatientName);
            }
        }
    }
}
