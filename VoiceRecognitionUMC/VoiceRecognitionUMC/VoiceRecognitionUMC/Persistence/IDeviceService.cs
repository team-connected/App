using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.Model;

namespace VoiceRecognitionUMC.Persistence
{
    interface IDeviceService
    {
        Task<Device> GetDeviceAsyncById(string deviceId);
        Task<Device> GetDeviceAsync(string deviceId);

    }
}
