using Newtonsoft.Json;
using Plugin.Fingerprint.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isVisibleSiVoto;
        private bool _isVisibleNoVoto;
        private UserResponse _user;
        private static HomePageViewModel _instance;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFingerprintService _fingerprintService;
        private DelegateCommand _goVoteCommand;

        public HomePageViewModel(INavigationService navigationService, IApiService apiService,
                                 IFingerprintService fingerprintService) : base (navigationService)
        {
            _instance = this;
            Title = "Inicio";
            LoadUser();
            _navigationService = navigationService;
            _apiService = apiService;
            _fingerprintService = fingerprintService;
            LoadVote();
        }
        public DelegateCommand GoVoteCommand => _goVoteCommand ?? (_goVoteCommand = new DelegateCommand(GoVoteAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsVisibleSiVoto
        {
            get => _isVisibleSiVoto;
            set => SetProperty(ref _isVisibleSiVoto, value);
        }

        public bool IsVisibleNoVoto
        {
            get => _isVisibleNoVoto;
            set => SetProperty(ref _isVisibleNoVoto, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public static HomePageViewModel GetInstance()
        {
            return _instance;
        }

        public async Task UpdateUserAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var secure = JsonConvert.DeserializeObject<TokenRequest>(Settings.Secure);

            var request = new TokenRequest
            {
                cedula = User.Cedula,
                password = secure.password
            };

            var response = await _apiService.GetUserAsync(url, "api", "/Users/Login", "bearer", token.Token, request);

            if (response.IsSuccess)
            {
                var user = response.Result;
                Settings.User = JsonConvert.SerializeObject(user);
                _user = user;
                LoadUser();
            }
        }

        private async void GoVoteAsync()
        {
            var activate = Settings.IsActivated;

            if (activate == true)
            {
                var result = await _fingerprintService.AuthenticateAsync("Desbloquear Votaciones");

                switch (result.Status)
                {
                    case FingerprintAuthenticationResultStatus.Succeeded:
                        await _navigationService.NavigateAsync("VotingsPage");
                        break;
                    case FingerprintAuthenticationResultStatus.NotAvailable:
                        await App.Current.MainPage.DisplayAlert("Error", "Su dispositivo no es compatible con el desbloqueo dactilar", "Aceptar");
                        break;
                    default:
                        await App.Current.MainPage.DisplayAlert("Error", "Autenticación incorrecta", "Aceptar");
                        break;
                }
            }

            if (activate == false)
            {
                await _navigationService.NavigateAsync("VotingsPage");
            }
        }

        private async void LoadVote()
        {
            IsRunning = true;
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            var response = await _apiService.GetVotingsAsync<VotingResponse>(url, "api", $"/Votings/{_user.UserId}", "bearer", token.Token);
            var votings = (List<VotingResponse>)response.Result;
            if (votings.Count == 0)
            {
                IsVisibleSiVoto = false;
                IsVisibleNoVoto = true;
                IsRunning = false;
            }
            else
            {
                IsVisibleSiVoto = true;
                IsVisibleNoVoto = false;
                IsRunning = false;
            }
        }

        private void LoadUser()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
        }
    }
}
