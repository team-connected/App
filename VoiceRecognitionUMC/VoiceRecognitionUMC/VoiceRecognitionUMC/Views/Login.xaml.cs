using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognitionUMC.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceRecognitionUMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            Entry_First.ReturnCommand = new Command(() => Entry_Second.Focus());
        }
    }
}