using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _tutorialCommand;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;

        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Login";
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public bool IsRemember { get; set; }

        public string Cedula { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand TutorialCommand => _tutorialCommand ?? (_tutorialCommand = new DelegateCommand(TutorialAsync));

        private async void TutorialAsync()
        {
            await _navigationService.NavigateAsync("TutorialPage");
        }

        private async void LoginAsync()
        {
            var isValid = await ValidateDataAsync();

            if (!isValid)
            {
                return;
            }

            IsRunning = true;

            var url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "No tiene conexión a internet.", "Aceptar");
                return;
            }

            var request = new TokenRequest
            {
                cedula = Cedula,
                password = Password
            };

            var response = await _apiService.GetTokenAsync(url,Cedula,Password);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", "Cédula o Contraseña incorrecta", "Aceptar");
                Password = string.Empty;
                return;
            }

            var token = response.Result;

            var response2 = await _apiService.GetUserAsync(url, "api", "/Users/Login", "bearer", token.Token, request);

            if (!response2.IsSuccess)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", "Hay un problema con su usuario, contacte con soporte", "Aceptar");
                Password = string.Empty;
                return;
            }

            var user = response2.Result;

            Settings.User = JsonConvert.SerializeObject(user);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.Secure = JsonConvert.SerializeObject(request);
            Settings.IsRemembered = IsRemember;

            IsRunning = false;

            Password = string.Empty;

            await _navigationService.NavigateAsync("/VotingMasterDetailPage/NavigationPage/HomePage");

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Cedula))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar su número de cédula", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar su contraseña", "Aceptar");
                return false;
            }

            return true;
        }
    }
}
