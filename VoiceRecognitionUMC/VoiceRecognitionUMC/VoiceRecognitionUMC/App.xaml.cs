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

        public static readonly IList<string> voiceRecognitionStopWords = new ReadOnlyCollection<string>(new List<string>
        {
            "stop", " stop meting"
        });

        public static readonly IList<string> voiceRecogntionAcceptWords = new ReadOnlyCollection<string>(new List<string>
        {
            "ja"
        });

        public static readonly IList<string> voiceRecogntionCancelWords = new ReadOnlyCollection<string>(new List<string>
        {
            "nee"
        });

        public static readonly IList<string> metricNumber = new ReadOnlyCollection<string>(new List<string>
        {
            "meting 1", "meting een", "meting 2", "meting twee", "meting 3", "meting drie"
        });

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/Login");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<VoiceRecognition, VoiceRecognitionViewModel>();
            containerRegistry.RegisterForNavigation<MetricResult, MetricResultViewModel>();
            containerRegistry.RegisterForNavigation<EditMetric, EditMetricViewmodel>();
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
