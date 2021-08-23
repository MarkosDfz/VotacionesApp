using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class ResultsPageViewModel : ViewModelBase
    {
        public string _filter;
        private bool _isRunning;
        private readonly IApiService _apiService;
        private DelegateCommand<string> _textChangedCommand;
        private readonly INavigationService _navigationService;
        public ObservableCollection<ResultItemViewModel> _results;

        public ResultsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Resultados";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadResultsAsync();
        }

        public DelegateCommand<string> TextChangedCommand => _textChangedCommand ?? (_textChangedCommand = new DelegateCommand<string>(TextChanged));

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public ObservableCollection<ResultItemViewModel> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }


        private void TextChanged(string obj)
        {
            if (Filter != "")
            {
                var suggestions = Results.Where(c => c.Description.ToLower().Contains(_filter.ToLower()));

                Results = new ObservableCollection<ResultItemViewModel>(suggestions.Select(r => new ResultItemViewModel(_navigationService)
                {
                    Candidates = r.Candidates,
                    DateTimeEnd = r.DateTimeEnd,
                    DateTimeStart = r.DateTimeStart,
                    Description = r.Description,
                    IsForAllUsers = r.IsForAllUsers,
                    QuantityVotes = r.QuantityVotes,
                    Remarks = r.Remarks,
                    State = r.State,
                    VotingId = r.VotingId,
                    Winner = r.Winner
                }).ToList());
            }
            else
            {
                LoadResultsAsync();
            }
        }

        private async void LoadResultsAsync()
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

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetResultsAsync<VotingResponse>(url, "api", "/Votings", "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Error al actualizar el usuario", "Aceptar");
                return;
            }

            IsRunning = false;

            var results = (List<VotingResponse>)response.Result;

            Results = new ObservableCollection<ResultItemViewModel>(results.Select(r => new ResultItemViewModel(_navigationService)
            {
                Candidates    = r.Candidates,
                DateTimeEnd   = r.DateTimeEnd,
                DateTimeStart = r.DateTimeStart,
                Description   = r.Description,
                IsForAllUsers = r.IsForAllUsers,
                QuantityVotes = r.QuantityVotes,
                Remarks       = r.Remarks,
                State         = r.State,
                VotingId      = r.VotingId,
                Winner        = r.Winner
            }).ToList());
        }
    }
}
