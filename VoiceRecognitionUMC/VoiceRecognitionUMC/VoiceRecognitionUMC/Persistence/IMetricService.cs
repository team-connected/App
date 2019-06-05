using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.Persistence
{
    interface IMetricService
    {
        Task SaveMetric(Metric metric);
        Task<MetricResponse> CreateMetric(MetricCreate metric, string patientId);
    }
}
