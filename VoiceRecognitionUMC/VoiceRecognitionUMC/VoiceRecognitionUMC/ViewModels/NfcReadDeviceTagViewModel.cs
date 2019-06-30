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

        public ObservableCollection<DeviceListItem> DeviceListItems
        {
            get { return this.deviceListItems; }
            set { SetProperty(ref this.deviceListItems, value); }
        }


        public NfcReadDeviceTagViewModel(INavigationService navigationService) : base(navigationService)
        {
            deviceListItems = new ObservableCollection<DeviceListItem>();
            
            deviceService = new DeviceService();
            NfcDevice = DependencyService.Get<INfcForms>();

            NfcDevice.NewTag += ReadTag;
        }

        public void ReadTag(object sender, NfcFormsTag e)
        {
            var encoding = Encoding.UTF8.GetString(e.NdefMessage.ToByteArray());
            var sn = encoding.Substring(7, encoding.Length - 7);
            LookUpSerialNumber(sn);
        }

        async void LookUpSerialNumber(string sn)
        {
            var toastConfig = new ToastConfig("Tag gescand");
            toastConfig.SetDuration(3000);
            toastConfig.SetBackgroundColor(System.Drawing.Color.White);
            toastConfig.SetMessageTextColor(System.Drawing.Color.Blue);

            UserDialogs.Instance.Toast(toastConfig);
            try
            {
                var foundDevice = await deviceService.GetDeviceAsync(sn);
                if (isUnique(foundDevice.sn))
                {
                    DeviceListItems.Add(new DeviceListItem
                    {
                        DeviceName = foundDevice.name,
                        SerialNumber = foundDevice.sn,
                        DeviceType = foundDevice.type
                    });
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

        private bool isUnique(string sn)
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

    }
}
