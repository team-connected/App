using Acr.UserDialogs;
using Poz1.NFCForms.Abstract;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;
using VoiceRecognitionUMC.Persistence;
using Xamarin.Forms;

namespace VoiceRecognitionUMC.ViewModels
{
    class NfcReadDeviceTagViewModel : BaseViewModel
    {
        private readonly INfcForms NfcDevice;
        private IDeviceService deviceService;
        private INavigationService _navigationService;
        private ObservableCollection<DeviceListItem> deviceListItems;
        private DeviceListItem selectedItem;
        public DelegateCommand ProceedToNextPageCommand { get; private set; }
        private INavigationParameters allParameters;

        public ObservableCollection<DeviceListItem> DeviceListItems
        {
            get { return this.deviceListItems; }
            set { SetProperty(ref this.deviceListItems, value); }
        }

        public DeviceListItem SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                SetProperty(ref this.selectedItem, value);
                if (SelectedItem != null)
                {
                    DeleteSelectedItem();
                }
            }
        }


        public NfcReadDeviceTagViewModel(INavigationService navigationService) : base(navigationService)
        {
            deviceListItems = new ObservableCollection<DeviceListItem>();
            _navigationService = navigationService;
            deviceService = new DeviceService();
            NfcDevice = DependencyService.Get<INfcForms>();
            ProceedToNextPageCommand = new DelegateCommand(GoToScanPatientNfcTagPage);

            NfcDevice.NewTag += ReadTag;
        }

        public async void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            var sn = encoding.Substring(7, encoding.Length - 7);
            await LookUpSerialNumber(sn);
        }

        private async Task LookUpSerialNumber(string sn)
        {
            var toastConfig = new ToastConfig("Tag gescand");
            toastConfig.SetDuration(1500);
            toastConfig.SetBackgroundColor(System.Drawing.Color.White);
            toastConfig.SetMessageTextColor(System.Drawing.Color.Green);

            UserDialogs.Instance.Toast(toastConfig);

            try
            {
                var foundDevice = await deviceService.GetDeviceAsync(sn);
                if (IsUnique(foundDevice.sn))
                {
                    DeviceListItems.Add(new DeviceListItem
                    {
                        DeviceName = foundDevice.name,
                        SerialNumber = foundDevice.sn,
                        DeviceType = foundDevice.type,
                        DeviceId = foundDevice._id
                    });
                }
                else
                {
                    toastConfig = new ToastConfig("Dit apparaat is al een keer gescand");
                    toastConfig.SetDuration(1500);
                    toastConfig.SetBackgroundColor(System.Drawing.Color.White);
                    toastConfig.SetMessageTextColor(System.Drawing.Color.Red);

                    UserDialogs.Instance.Toast(toastConfig);
                }

            }
            catch (ArgumentOutOfRangeException ex)
            {
                toastConfig = new ToastConfig("Er is iets misgegaan. Mogelijk is dit geen geldige tag.");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.Firebrick);
                toastConfig.SetMessageTextColor(System.Drawing.Color.White);

                UserDialogs.Instance.Toast(toastConfig);

                Console.WriteLine(ex.Message);
            }
        }

        private bool IsUnique(string sn)
        {
            int count = 0;
            foreach(DeviceListItem dli in DeviceListItems)
            {
                if (sn.Equals(dli.SerialNumber))
                {
                    count += 1;
                }
            }
            return count == 0;
        }

        private void DeleteSelectedItem()
        {
            //in production
        }

        private async void GoToScanPatientNfcTagPage()
        {
            if (DeviceListItems.Count > 0)
            {
                allParameters.Add("deviceList", DeviceListItems);
                
                await _navigationService.NavigateAsync("NfcReadPatientTagPage", allParameters);
            } else
            {
                var toastConfig = new ToastConfig("Scan de tag van minimaal 1 apparaat");
                toastConfig.SetDuration(5000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.White);
                toastConfig.SetMessageTextColor(System.Drawing.Color.Red);

                UserDialogs.Instance.Toast(toastConfig);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            allParameters = parameters;
        }
    }
}
