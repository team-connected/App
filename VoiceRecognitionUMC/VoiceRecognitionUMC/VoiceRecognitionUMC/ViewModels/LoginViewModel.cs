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
        private string username;
        private string userId;
        #endregion

        #region COMMANDS
        public DelegateCommand LoginCommand { get; private set; }
        #endregion

        #region PROPERTIES
        public string Username
        {
            get { return this.username; }
            set { SetProperty(ref this.username, value); }
        }
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

            if (!String.IsNullOrEmpty(Username))
            {
                switch (Username.ToLower())
                {
                    case "maarten":
                        userId = "31cd6c914fea4ee7b572dc76617960f2";
                        break;
                    case "haydn":
                        userId = "ad834eeec4a64851b431d2ea8062db80";
                        break;
                    case "sam":
                        userId = "4a65bbf937ee4fffaa1d3b74c13c9546";
                        break;
                    case "shaniah":
                        userId = "8e1b22f92f69425cb84679e9e81bd4e5";
                        break;
                    case "jeroen":
                        userId = "82f541d8b97146398ff14c2d6cd6c449";
                        break;
                }
            }
            else
            {
                userId = "nv7sg3vrxrwr8y8vugamdztxzrhwemhj";
            }
            

            var navigationParams = new NavigationParameters
            {
                { "userId", userId }
            };

            _navigationService.NavigateAsync("../NfcReadDeviceTagPage", navigationParams);
        }
        #endregion
    }
}
