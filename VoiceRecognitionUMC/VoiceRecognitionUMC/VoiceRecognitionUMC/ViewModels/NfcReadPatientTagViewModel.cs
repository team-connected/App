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
        private string patientNaam;
        private IPatientService patientService;

        public string PatientNaam
        {
            get { return this.patientNaam; }
            set { SetProperty(ref this.patientNaam, value); }
        }

        public NfcReadPatientTagViewModel(INavigationService navigationService) : base(navigationService)
        {
            device = DependencyService.Get<INfcForms>();
            patientService = new PatientService();

            device.NewTag += ReadTag;
        }

        void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            var nfcText = encoding.Substring(7, encoding.Length - 7);
            LookUpPatient(nfcText);
        }

        void LookUpPatient(string nfcText)
        {
            try
            {
                var foundPatient = await patientService.RefreshPatients            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
