using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognitionUMC.Dependency
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
}
