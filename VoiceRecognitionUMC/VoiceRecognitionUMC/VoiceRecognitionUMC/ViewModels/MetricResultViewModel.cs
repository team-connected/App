﻿using Prism.Commands;
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
        private string patientId;
        private IPatientService _patientService;
        private string nurseName;
        private string userId;
        #endregion|

        #region COMMMANDS
        public DelegateCommand SaveCommand { get; private set; }
        #endregion

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
        public string NurseName
        {
            get { return this.nurseName; }
            set { SetProperty(ref this.nurseName, value); }
        }
        #endregion
    
        #region CONSTRUCTOR
        public MetricResultViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _metricService = new MetricService();
            _nurseService = new NurseService();
            _deviceService = new DeviceService();
            _patientService = new PatientService();

            SaveCommand = new DelegateCommand(Save);
        }
        #endregion

        #region FUNCTIONS
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("metricId"))
            {
                metricId = parameters.GetValue<string>("metricId");
                patientId = parameters.GetValue<string>("patientId");
                userId = parameters.GetValue<string>("userId");
            }
            FillList();
        }

        private async void FillList()
        {
            Metrics = new ObservableCollection<MetricListItem>();
            metric = await _metricService.GetMetric(metricId);
            var nurse = await _nurseService.GetNurseAsync(metric.nurse_id);
            var patient = await _patientService.GetPatient(patientId);

            NurseName = $"Ingelogd als: {nurse.firstname} {nurse.lastname}";

            if (metric.bloeddruk != "0")
            {

                var deviceName = await _deviceService.GetDeviceAsyncById(metric.device_bloeddruk);

                var listItem = new MetricListItem
                {
                    MetricType = "Bloeddruk",
                    MetricValue = $"{metric.bloeddruk} mmHg",
                    Device = deviceName.name,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    ID = metric._id,
                    PatientName = $"{patient.Firstname} {patient.Lastname}"
                };

                Metrics.Add(listItem);
            }
            if (metric.gewicht != "0")
            {
                var deviceName = await _deviceService.GetDeviceAsyncById(metric.device_gewicht);
                var listItem = new MetricListItem
                {
                    MetricType = "Gewicht",
                    MetricValue = $"{metric.gewicht} kg",
                    Device = deviceName.name,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    ID = metric._id,
                    PatientName = $"{patient.Firstname} {patient.Lastname}"
                };

                Metrics.Add(listItem);
            }
            if (metric.temperatuur != "0")
            {
                var deviceName = await _deviceService.GetDeviceAsyncById(metric.device_temperatuur);

                var listItem = new MetricListItem
                {
                    MetricType = "Temperatuur",
                    MetricValue = $"{metric.temperatuur} °C",
                    Device = deviceName.name,
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    ID = metric._id,
                    PatientName = $"{patient.Firstname} {patient.Lastname}"
                };

                Metrics.Add(listItem);
            }
            if (metric.pijnscore != "0")
            {
                var listItem = new MetricListItem
                {
                    MetricType = "Pijnscore",
                    MetricValue = $"{metric.pijnscore}",
                    Device = "",
                    NurseName = $"{nurse.firstname} {nurse.lastname}",
                    ID = metric._id,
                    PatientName = $"{patient.Firstname} {patient.Lastname}"
                };

                Metrics.Add(listItem);
            }
        }

        private void ItemSelected()
        {
            var metric = SelectedItem;

            var navigationParameters = new NavigationParameters
            {
                {"metric", metric },
                {"patientId", patientId }
            };

            _navigationService.NavigateAsync("EditMetric", navigationParameters);
        }

        public void Save()
        {
            _navigationService.NavigateAsync("../../../../Login");
        }
        #endregion
    }
}
