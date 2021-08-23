using Newtonsoft.Json;
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
    public class VotingsPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private UserResponse _user;
        private TokenResponse _token;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<VotingItemViewModel> _votings;

        public VotingsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Votaciones";
            LoadVotingsAsync();
        }

        public ObservableCollection<VotingItemViewModel> Votings
        {
            get => _votings;
            set => SetProperty(ref _votings, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private async void LoadVotingsAsync()
        {
            IsRunning = true;

            var connection = await _apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "No tiene conexión a internet.", "Aceptar");
                return;
            }

            var url = App.Current.Resources["UrlAPI"].ToString();

            _token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var response = await _apiService.GetVotingsAsync<VotingResponse>(url, "api", $"/Votings/{_user.UserId}", "bearer", _token.Token);

            IsRunning = false;

            var votings = (List<VotingResponse>)response.Result;
            Votings = new ObservableCollection<VotingItemViewModel>(votings.Select(v => new VotingItemViewModel(_navigationService)
            {
                Candidates    = v.Candidates,
                DateTimeEnd   = v.DateTimeEnd,
                DateTimeStart = v.DateTimeStart,
                Description   = v.Description,
                IsForAllUsers = v.IsForAllUsers,
                QuantityVotes = v.QuantityVotes,
                Remarks       = v.Remarks,
                State         = v.State,
                VotingId      = v.VotingId,
                Winner        = v.Winner
            }).ToList());
        }
    }
}
