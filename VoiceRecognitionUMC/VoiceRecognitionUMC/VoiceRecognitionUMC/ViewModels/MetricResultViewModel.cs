using Prism.Commands;
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
        private GetMetric metric;
        private MetricListItem selectedItem;
        private INavigationService _navigationService;
        #endregion|

        #region PROPERTIES
        public ObservableCollection<MetricListItem> Metrics
        {
            get { return this.metrics; }
            set { SetProperty(ref this.metrics, value); }
        }
        public MetricListItem SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                SetProperty(ref this.selectedItem, value);
                if (SelectedItem != null)
                {
                    ItemSelected();
                }
            }
        }
        #endregion
    
        #region CONSTRUCTOR
        public MetricResultViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _metricService = new MetricService();
            _nurseService = new NurseService();
            _deviceService = new DeviceService();
        }
        #endregion

        #region FUNCTIONS
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("metricId"))
            {
                metricId = parameters.GetValue<string>("metricId");
            }
            FillList();
        }

        private async void FillList()
        {
            Metrics = new ObservableCollection<MetricListItem>();
            metric = await _metricService.GetMetric(metricId);
            var nurse = await _nurseService.GetNurseAsync(metric.nurse_id);

            if (metric.bloeddruk != "0")
            {

                var deviceName = await _deviceService.GetDeviceAsync(metric.device_bloeddruk);

                var listItem = new MetricListItem
                {
                    MetricType = "Bloeddruk",
                    MetricValue = metric.bloeddruk,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = deviceName.name,
                    ID = metric._id
                };

                Metrics.Add(listItem);
            }
            if (metric.gewicht != "0")
            {
                var deviceName = await _deviceService.GetDeviceAsync(metric.device_gewicht);
                var listItem = new MetricListItem
                {
                    MetricType = "Gewicht",
                    MetricValue = metric.gewicht,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = deviceName.name,
                    ID = metric._id
                };

                Metrics.Add(listItem);
            }
            if (metric.temperatuur != "0")
            {
                var deviceName = await _deviceService.GetDeviceAsync(metric.device_temperatuur);

                var listItem = new MetricListItem
                {
                    MetricType = "Temperatuur",
                    MetricValue = metric.temperatuur,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    Device = deviceName.name,
                    ID = metric._id
                };

                Metrics.Add(listItem);
            }
        }

        private void ItemSelected()
        {
            var metric = SelectedItem;

            var navigationParameters = new NavigationParameters
            {
                {"metric", metric }
            };

            _navigationService.NavigateAsync("EditMetric", navigationParameters);
        }
        #endregion
    }
}
