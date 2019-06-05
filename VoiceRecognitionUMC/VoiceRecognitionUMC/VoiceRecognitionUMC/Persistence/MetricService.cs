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
            var uri = new Uri("http://145.89.207.78:8000/api/v1/voice/");

            /*try
            {
                //var json = JsonConvert.
            }*/
        }
    }
}
