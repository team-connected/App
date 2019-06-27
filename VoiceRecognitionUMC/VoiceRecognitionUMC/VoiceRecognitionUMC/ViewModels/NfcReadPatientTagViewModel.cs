using Poz1.NFCForms.Abstract;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceRecognitionUMC.Persistence;
using Xamarin.Forms;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcReadPatientTagViewModel : BaseViewModel
    {
        private readonly INfcForms device;
        private IPatientService patientService;
        private INavigationService _navigationService;
        private string tagFound;

        public string TagFound
        {
            get { return this.tagFound; }
            set { SetProperty(ref this.tagFound, value); }
        }

        public NfcReadPatientTagViewModel(INavigationService navigationService) : base(navigationService)
        {
            device = DependencyService.Get<INfcForms>();
            patientService = new PatientService();
            _navigationService = navigationService;
            device.NewTag += ReadTag;
        }

        void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            var nfcText = encoding.Substring(7, encoding.Length - 7);
            LookUpPatient(nfcText);
        }

        async void LookUpPatient(string nfcText)
        {
            try
            {
                TagFound = "Halsband gescand. Een moment aub...";

                var foundPatient = await patientService.GetPatient(nfcText);
                var patientName = $"{foundPatient.Firstname} {foundPatient.Lastname}";

                var navigationParameters = new NavigationParameters
                {
                    {"patientFullName", patientName }
                };

                await _navigationService.NavigateAsync("../NfcReadPatientTagResultPage", navigationParameters);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
