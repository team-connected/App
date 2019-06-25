using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceRecognitionUMC.Model;
using VoiceRecognitionUMC.Persistence;

namespace VoiceRecognitionUMC.ViewModels
{
    class EditMetricViewmodel : BaseViewModel
    {
        #region MEMBERS
        private string nurseName;
        private string deviceName;
        private string metricType;
        private string metricValue;
        private INavigationService _navigationService;
        private MetricListItem metric;
        private GetMetric updatedMetric;
        private IMetricService _metricService;
        #endregion

        #region COMMANDS
        public DelegateCommand SaveCommand { get; private set; }
        #endregion

        #region PROPERTIES
        public string NurseName
        {
            get { return this.nurseName; }
            set { SetProperty(ref this.nurseName, value); }
        }
        public string DeviceName
        {
            get { return this.deviceName; }
            set { SetProperty(ref this.deviceName, value); }
        }
        public string MetricType
        {
            get { return this.metricType; }
            set { SetProperty(ref this.metricType, value); }
        }
        public string MetricValue
        {
            get { return this.metricValue; }
            set { SetProperty(ref this.metricValue, value); }
        }
        #endregion

        #region CONSTRUCTOR
        public EditMetricViewmodel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _metricService = new MetricService();

            SaveCommand = new DelegateCommand(SaveMetric);
        }
        #endregion

        #region FUNCTIONS
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            metric = parameters.GetValue<MetricListItem>("metric");
            FillLabels();
        }

        private async void FillLabels()
        {
            NurseName = metric.NurseName;
            DeviceName = metric.Device;
            MetricType = metric.MetricType;
            MetricValue = metric.MetricValue;

            updatedMetric = await _metricService.GetMetric(metric.ID);
        }
        
        private void SaveMetric()
        {
            Object updatedMetric = null;
            if (metric.MetricType.ToLower() == "bloeddruk")
            {
                updatedMetric = new UpdateBlooddruk
                {
                    blooddruk = MetricValue
                };

            }
            if (metric.MetricType.ToLower() == "gewicht")
            {
                updatedMetric = new UpdateGewicht
                {
                    gewicht = MetricValue
                };
            }
            if (metric.MetricType.ToLower() == "temperatuur")
            {
                updatedMetric = new UpdateTemperatuur
                {
                    temperatuur = MetricValue
                };
            }

            _metricService.UpdateMetric(metric.ID, updatedMetric);
            var navigationParameters = new NavigationParameters
            {
                {"metricId", metric.ID }
            };

            _navigationService.NavigateAsync("../MetricResult", navigationParameters);
        }
        #endregion
    }
}
