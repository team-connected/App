using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Model
{
    class MetricCreate
    {
        public string metric_type { get; set; }
        public DateTime timestamp { get; set; }
        public string device_id { get; set; }
        public string nurse_id { get; set; }
        public List<string> value { get; set; }
        public string comment { get; set; }
    }
}
