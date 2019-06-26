using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Prism.Navigation;
using VoiceRecognitionUMC.Dependency;
using Xamarin.Forms;
using Poz1.NFCForms.Abstract;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcHandlerViewModel : BaseViewModel
    {
        private ITagHandler tagHandler;
        private readonly INfcForms device;

        public NfcHandlerViewModel(INavigationService navigationService) : base(navigationService)
        {
            DependencyService.Register<ITagHandler>();
            tagHandler = DependencyService.Get<ITagHandler>();
            device = DependencyService.Get<INfcForms>();
        }

        void ReadTag(object sender, NfcFormsTag e)
        {
            tagHandler.StartReadNewTag();
        }

        void ManageTag(object sender, NfcFormsTag e)
        {

        }
    }
}
