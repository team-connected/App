using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VoiceRecognitionUMC.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VoiceRecognitionUMC.Persistence
{
    class PatientService : IPatientService
    {
        HttpClient _client;

        public List<Patient> Patients { get; private set; }

        public PatientService()
        {
            _client = new HttpClient();
        }

        public async Task<Patient> GetPatient(string patientId)
        {
            Patients = new List<Patient>();

            var uri = new Uri($"http://umc-api.maartenmol.nl:5000/api/v1/patient/_id={patientId}");

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Patients = JsonConvert.DeserializeObject<List<Patient>>(content);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Patients[0];
        }
    }
}
