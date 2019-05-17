using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Xamarin.Forms;
using Prism.Services;

namespace VoiceRecognitionUMC.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        #region MEMBERS
        private INavigationService _navigationService;
        #endregion

        #region COMMANDS
        public DelegateCommand LoginCommand { get; private set; }
        #endregion

        #region CONSTRUCTOR
        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(Login);
            _navigationService = navigationService;
        }
        #endregion

        #region FUNCTIONS
        private void Login()
        {
            _navigationService.NavigateAsync("../VoiceRecognition");
        }
        #endregion
    }
}
