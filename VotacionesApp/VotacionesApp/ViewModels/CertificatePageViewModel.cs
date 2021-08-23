using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VotacionesApp.ViewModels
{
    public class CertificatePageViewModel : ViewModelBase
    {
        private Candidate _voting;
        private DelegateCommand _homeCommand;
        private DelegateCommand _certificadoCommand;
        private readonly INavigationService _navigationService;

        public CertificatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Certificado";
            _navigationService = navigationService;
        }

        public Candidate Voting
        {
            get => _voting;
            set => SetProperty(ref _voting, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("candidatevoting"))
            {
                Voting = parameters.GetValue<Candidate>("candidatevoting");
            }
        }

        public DelegateCommand CertificadoCommand => _certificadoCommand ?? (_certificadoCommand = new DelegateCommand(CertificadoAsync));

        public DelegateCommand HomeCommand => _homeCommand ?? (_homeCommand = new DelegateCommand(HomeAsync));

        private void HomeAsync()
        {
            _navigationService.NavigateAsync("/VotingMasterDetailPage/NavigationPage/HomePage");
        }

        private async void CertificadoAsync()
        {
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            var url = "https://votacionesuejosemejia.tk/Votings/ShowCertificate1/";
            await Launcher.OpenAsync(new Uri($"{url}{Voting.VotingId}?id2={user.UserId}"));
        }
    }
}
