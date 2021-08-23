using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VotacionesApp.Models;

namespace VotacionesApp.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {
        private VotingResponse _result;
        private ObservableCollection<Candidate> _candidates;

        public ResultPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public VotingResponse Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ObservableCollection<Candidate> Candidates
        {
            get => _candidates;
            set => SetProperty(ref _candidates, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("result"))
            {
                Result = parameters.GetValue<VotingResponse>("result");
                Candidates = new ObservableCollection<Candidate>(Result.Candidates.OrderByDescending(c => c.QuantityVotes));
                Title = "Detalles";
            }
        }
    }
}