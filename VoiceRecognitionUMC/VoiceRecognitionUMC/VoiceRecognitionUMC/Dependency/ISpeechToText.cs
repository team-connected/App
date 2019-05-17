using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Dependency
{
    public interface ISpeechToText
    {
        void StartSpeechToText();
        void StopSpeechToText();
    }
}
