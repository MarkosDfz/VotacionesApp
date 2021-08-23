using System;
using Prism.Commands;
using Prism.Navigation;
using VotacionesApp.Models;

namespace VotacionesApp.ViewModels
{
    public class ResultItemViewModel : VotingResponse
    {
        private DelegateCommand _selectResultCommand;

        private readonly INavigationService _navigationService;

        public ResultItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectResultCommand => _selectResultCommand ?? (_selectResultCommand = new DelegateCommand(SelectResultAsync));

        private async void SelectResultAsync()
        {
            var parameters = new NavigationParameters
            {
                { "result", this }
            };

            await _navigationService.NavigateAsync("ResultPage", parameters);
        }
    }
}
