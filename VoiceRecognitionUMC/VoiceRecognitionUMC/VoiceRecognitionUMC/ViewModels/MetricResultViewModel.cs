using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VoiceRecognitionUMC.Model;
using VoiceRecognitionUMC.Persistence;

namespace VoiceRecognitionUMC.ViewModels
{
    class MetricResultViewModel : BaseViewModel 
    {
        #region MEMBERS
        private IMetricService _metricService;
        private ObservableCollection<MetricListItem> metrics;
        private INurseService _nurseService;
        private IDeviceService _deviceService;
        private string metricId;
        #endregion

        #region PROPERTIES
        public ObservableCollection<MetricListItem> Metrics
        {
            get { return this.metrics; }
            set { SetProperty(ref this.metrics, value); }
        }
        #endregion
    
        #region CONSTRUCTOR
        public MetricResultViewModel(INavigationService navigationService) : base(navigationService)
        {
            _metricService = new MetricService();
            _nurseService = new NurseService();
            _deviceService = new DeviceService();
        }
        #endregion

        #region FUNCTIONS
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            metricId = parameters.GetValue<string>("metricId");
            FillList();
        }

        private async void FillList()
        {
            Metrics = new ObservableCollection<MetricListItem>();
            var metric = await _metricService.GetMetric(metricId);
            var nurse = await _nurseService.GetNurseAsync(metric.nurse_id);
            var device = await _deviceService.GetDeviceAsync(metric.device_id);

            if (metric.bloeddruk != "0")
            {
                var listItem = new MetricListItem
                {
                    MetricType = "Bloeddruk",
                    MetricValue = metric.bloeddruk,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = device.name
                };

                Metrics.Add(listItem);
            }
            if (metric.gewicht != "0")
            {
                var listItem = new MetricListItem
                {
                    MetricType = "Gewicht",
                    MetricValue = metric.gewicht,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = device.name
                };

                Metrics.Add(listItem);
            }
            if (metric.temperatuur != "0")
            {
                var listItem = new MetricListItem
                {
                    MetricType = "Temperatuur",
                    MetricValue = metric.temperatuur,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = device.name
                };

                Metrics.Add(listItem);
            }
        }
        #endregion
    }
}
