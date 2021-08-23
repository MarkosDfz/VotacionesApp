using System;
using Newtonsoft.Json;
using Prism.Navigation;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;
using Xamarin.Forms;

namespace VotacionesApp.Views
{
    public partial class VotingPage : ContentPage
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public VotingPage(INavigationService navigationService, IApiService apiService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _apiService = apiService;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = (TappedEventArgs)e;

            var obj = args.Parameter;
            var c = (Candidate)obj;

            var answer = await App.Current.MainPage.DisplayAlert("Confirmar",
            $"¿Está seguro de votar por: {c.User.FullName}?",
            "Si", "No");

            if (!answer)
            {
                return;
            }

            grid.IsVisible = true;
            animation.IsVisible = true;

            var url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                grid.IsVisible = false;
                animation.IsVisible = false;
                await App.Current.MainPage.DisplayAlert("Error", "No tiene conexión a internet.", "Aceptar");
                return;
            }

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var voteRequest = new VoteRequest
            {
                CandidateId = c.CandidateId,
                UserId = user.UserId,
                VotingId = c.VotingId
            };

            var response = await _apiService.PostAsync(url, "api", "/Votings/VoteForCandidate", voteRequest, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                grid.IsVisible = false;
                animation.IsVisible = false;
                await App.Current.MainPage.DisplayAlert("Error", "Error al votar", "Aceptar");
                return;
            }

            grid.IsVisible = false;
            animation.IsVisible = false;

            await App.Current.MainPage.DisplayAlert("Completado", "Tu voto a sido registrado con éxito", "Aceptar");

            var parameters = new NavigationParameters
            {
                { "candidatevoting", c }
            };

            await _navigationService.NavigateAsync("/NavigationPage/CertificatePage", parameters);
        }
    }
}
