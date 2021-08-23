using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using VotacionesApp.Models;

namespace VotacionesApp.ViewModels
{
    public class VotingItemViewModel : VotingResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectVotingCommand;

        public VotingItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectVotingCommand => _selectVotingCommand ?? (_selectVotingCommand = new DelegateCommand(SelectVotingAsync));

        private async void SelectVotingAsync()
        {
            var parameters = new NavigationParameters
            {
                { "voting", this }
            };

            await _navigationService.NavigateAsync("VotingPage", parameters);
        }
    }
}
