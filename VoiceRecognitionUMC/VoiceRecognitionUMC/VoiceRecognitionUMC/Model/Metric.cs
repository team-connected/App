using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Model
{
    class Metric
    {
        public string Metric_Id { get; set; }
        public string Patient_Id { get; set; }
        public string Nurse_Id { get; set; }
        public string Raw_Text { get; set; }
    }
}
