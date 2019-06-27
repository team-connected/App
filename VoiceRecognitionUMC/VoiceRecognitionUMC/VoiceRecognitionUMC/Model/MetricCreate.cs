using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Model
{
    class MetricCreate
    {
        public string metric_type { get; set; }
        public string timestamp { get; set; }
        public string device_bloeddruk { get; set; }
        public string device_gewicht { get; set; }
        public string device_temperatuur { get; set; }
        public string nurse_id { get; set; }
        public string comment { get; set; }
    }
}
