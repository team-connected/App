using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Prism.Navigation;
using VoiceRecognitionUMC.Dependency;
using Xamarin.Forms;
using Poz1.NFCForms.Abstract;
using NdefLibrary.Ndef;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcHandlerViewModel : BaseViewModel
    {
        private readonly INfcForms device;
        private string nfcText;

        public string NFCText
        {
            get { return this.nfcText; }
            set { SetProperty(ref this.nfcText, value); }
        }

        public NfcHandlerViewModel(INavigationService navigationService) : base(navigationService)
        {
            device = DependencyService.Get<INfcForms>();

            device.NewTag += ReadTag;
        }

        void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            NFCText = encoding.Substring(7, encoding.Length - 7);

        }
    }
}
