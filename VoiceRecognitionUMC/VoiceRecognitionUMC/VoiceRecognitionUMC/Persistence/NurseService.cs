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
    class NurseService : INurseService
    {
        HttpClient _client;

        public NurseService()
        {
            _client = new HttpClient();
        }

        public async Task<Nurse> GetNurseAsync(string nurseId)
        {
            List<Nurse> nurse = new List<Nurse>();

            var uri = new Uri($"http://umc-api.maartenmol.nl:5000/api/v1/nurse/_id={nurseId}");

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    nurse = JsonConvert.DeserializeObject<List<Nurse>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return nurse[0];
        }
    }
}
