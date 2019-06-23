using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.Persistence
{
    interface INurseService
    {
        Task<Nurse> GetNurseAsync(string nurseId);
    }
}
