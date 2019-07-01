using Acr.UserDialogs;
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
        private INavigationParameters allParameters;

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
            var toastConfig = new ToastConfig("Tag gescand");
            toastConfig.SetDuration(1500);
            toastConfig.SetBackgroundColor(System.Drawing.Color.White);
            toastConfig.SetMessageTextColor(System.Drawing.Color.Blue);

            UserDialogs.Instance.Toast(toastConfig);
            try
            {

                var foundPatient = await patientService.GetPatient(nfcText);

                allParameters.Add("patient", foundPatient);

                await _navigationService.NavigateAsync("../NfcReadPatientTagResultPage", allParameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                toastConfig = new ToastConfig("Er is iets misgegaan. Mogelijk is dit geen geldige tag.");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.Firebrick);
                toastConfig.SetMessageTextColor(System.Drawing.Color.White);

                UserDialogs.Instance.Toast(toastConfig);

                Console.WriteLine(ex.Message);
            }

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            allParameters = parameters;
        }
    }
}
