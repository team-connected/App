using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using VoiceRecognitionUMC.Views;
using VoiceRecognitionUMC.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VoiceRecognitionUMC
{
    public partial class App : PrismApplication
    {
        public static readonly IList<string> voiceRecognitionKeyWords = new ReadOnlyCollection<string>(new List<string>
        {
            "registreer", "registreren", "registreert"
        });
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/NfcReadNurseTagPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<VoiceRecognition, VoiceRecognitionViewModel>();
            containerRegistry.RegisterForNavigation<MetricResult, MetricResultViewModel>();
<<<<<<< HEAD
            containerRegistry.RegisterForNavigation<NFCHandler, NfcHandlerViewModel>();
            containerRegistry.RegisterForNavigation<NfcReadDeviceTagPage, NfcReadDeviceTagViewModel>();
            containerRegistry.RegisterForNavigation<NfcReadNurseTagPage, NfcReadNurseTagViewModel>();
            containerRegistry.RegisterForNavigation<NfcReadPatientTagPage, NfcReadPatientTagViewModel>();
=======
            containerRegistry.RegisterForNavigation<EditMetric, EditMetricViewmodel>();
>>>>>>> origin/master
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
