using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class VotingPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private VotingResponse _voting;
        private UserResponse _user;

        public VotingPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public VotingResponse Voting
        {
            get => _voting;
            set => SetProperty(ref _voting, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("voting"))
            {
                Voting = parameters.GetValue<VotingResponse>("voting");
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                Title = "Candidatos";
            }
        }
    }
}
