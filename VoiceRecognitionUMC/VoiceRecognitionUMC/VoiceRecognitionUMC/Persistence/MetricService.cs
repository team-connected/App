using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;
using Newtonsoft.Json;

namespace VoiceRecognitionUMC.Persistence
{
    class MetricService : IMetricService
    {
        HttpClient _client;

        public MetricService()
        {
            _client = new HttpClient();
        }

        public async Task SaveMetric(Metric metric)
        {
            var uri = new Uri("http://umc-api.maartenmol.nl:8000/api/v1/voice/");

            try
            {
                var json = JsonConvert.SerializeObject(metric);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Metric succesvol opgeslagen");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<MetricResponse> CreateMetric(MetricCreate metric, string patientId)
        {
            MetricResponse responseMessage = null;
            var uri = new Uri($"http://umc-api.maartenmol.nl:5000/api/v1/metric/patient={patientId}");

            try
            {
                var json = JsonConvert.SerializeObject(metric);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Metric aangemaakt");
                    var responseJson = await response.Content.ReadAsStringAsync();
                    responseMessage = JsonConvert.DeserializeObject<MetricResponse>(responseJson);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return responseMessage;
        }

        public async Task<GetMetric> GetMetric(string metricId)
        {
            List<GetMetric> metric = new List<GetMetric>();
            var uri = new Uri($"http://umc-api.maartenmol.nl:5000/api/v1/metric/_id={metricId}");

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    metric = JsonConvert.DeserializeObject<List<GetMetric>>(content);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return metric[0];
        }
    }
}
