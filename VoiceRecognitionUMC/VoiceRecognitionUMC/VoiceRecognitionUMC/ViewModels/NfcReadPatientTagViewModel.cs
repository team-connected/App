using Poz1.NFCForms.Abstract;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcReadPatientTagViewModel : BaseViewModel
    {
        private readonly INfcForms device;
        private string nfcText;

        public string NFCText
        {
            get { return this.nfcText; }
            set { SetProperty(ref this.nfcText, value); }
        }

        public NfcReadPatientTagViewModel(INavigationService navigationService) : base(navigationService)
        {
            device = DependencyService.Get<INfcForms>();

            device.NewTag += ReadTag;
        }

        void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            NFCText = encoding.Substring(7, encoding.Length - 7);
            LookUpPatient(NFCText);
        }

        void LookUpPatient(string nfcText)
        {

        }
    }
}
