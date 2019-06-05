using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Model
{
    class Metric
    {
        public string metric_id { get; set; }
        public string patient_id { get; set; }
        public string nurse_id { get; set; }
        public string raw_text { get; set; }
    }
}
