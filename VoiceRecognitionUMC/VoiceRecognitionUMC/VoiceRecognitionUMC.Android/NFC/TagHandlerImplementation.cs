using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Poz1.NFCForms.Abstract;
using Poz1.NFCForms.Droid;
using VoiceRecognitionUMC.Dependency;
using VoiceRecognitionUMC.Droid.NFC;

[assembly: Xamarin.Forms.Dependency(typeof(TagHandlerImplementation))]
namespace VoiceRecognitionUMC.Droid.NFC
{
    
    class TagHandlerImplementation : ITagHandler
    {
        public NfcAdapter NFCdevice;
        public NfcForms x;
        private Activity _activity;

        public TagHandlerImplementation()
        {
            NfcManager NfcManager = (NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);
            NFCdevice = NfcManager.DefaultAdapter;

            Xamarin.Forms.DependencyService.Register<INfcForms, NfcForms>();
            x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;

            _activity = CrossCurrentActivity.Current.Activity;
        }

        public void StartReadNewTag()
        {
            if (NFCdevice != null)
            {
                var intent = new Intent(_activity, GetType()).AddFlags(ActivityFlags.SingleTop);
                NFCdevice.EnableForegroundDispatch
                (
                    _activity,
                    PendingIntent.GetActivity(_activity, 0, intent, 0),
                    new[] { new IntentFilter(NfcAdapter.ActionTechDiscovered) },
                    new String[][] {new string[] {
                            NFCTechs.Ndef,
                        },
                        new string[] {
                            NFCTechs.MifareClassic,
                        },
                    }
                );
            }
        }

        
    }
}