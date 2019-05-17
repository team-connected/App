﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using VoiceRecognitionUMC.Dependency;
using VoiceRecognitionUMC.Droid.Voice;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation))]
namespace VoiceRecognitionUMC.Droid.Voice
{
    class SpeechToTextImplementation : ISpeechToText
    {
        private readonly int VOICE = 10;
        private Activity _activity;
        
        public SpeechToTextImplementation()
        {
            _activity = CrossCurrentActivity.Current.Activity;
        }

        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec == "android.hardware.microphone")
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Spreek Nu");

                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                _activity.StartActivityForResult(voiceIntent, VOICE);
            }
        }

        public void StopSpeechToText()
        {

        }
    }
}