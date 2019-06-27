using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.Persistence
{
    class DeviceService : IDeviceService
    {
        private HttpClient _client;

        public DeviceService()
        {
            _client = new HttpClient();
        }
        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            List<Device> device = new List<Device>();

            var uri = new Uri($"http://umc-api.maartenmol.nl:5000/api/v1/device/sn={deviceId}");

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    device = JsonConvert.DeserializeObject<List<Device>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return device[0];
        }
    }
}
