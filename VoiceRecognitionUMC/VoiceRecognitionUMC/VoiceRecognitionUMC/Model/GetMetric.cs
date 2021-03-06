﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Model
{
    class GetMetric
    {
        public string _id { get; set; }
        public string bloeddruk { get; set; }
        public string gewicht { get; set; }
        public string temperatuur { get; set; }
        public string pijnscore { get; set; }
        public string device_bloeddruk { get; set; }
        public string device_gewicht { get; set; }
        public string device_temperatuur { get; set; }
        public DateTime timestamp { get; set; }
        public string nurse_id { get; set; }
        public string comment { get; set; }
    }
}
