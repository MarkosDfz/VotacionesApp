using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;
using Xamarin.Essentials;

namespace VotacionesApp.ViewModels
{
    public class CertificatesPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isVisible;
        private ObservableCollection<CertificateItemViewModel> _certificados;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
       
        public CertificatesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Certificados";
            LoadCertificatesAsync();
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ObservableCollection<CertificateItemViewModel> Certificados
        {
            get => _certificados;
            set => SetProperty(ref _certificados, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private async void LoadCertificatesAsync()
        {
            IsRunning = true;

            var url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "No tiene conexión a internet.", "Aceptar");
                return;
            }

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var response = await _apiService.GetListAsync<CertificateResponse>(url, "api", $"/Votings/Certificados/{user.UserId}", "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Error al actualizar el usuario", "Aceptar");
                return;
            }
            
            var cer = (List<CertificateResponse>)response.Result;

            if (cer.Count == 0)
            {
                IsVisible = true;
            }

            IsRunning = false;

            Certificados = new ObservableCollection<CertificateItemViewModel>(cer.Select(c => new CertificateItemViewModel(_navigationService)
            {
                DateTimeEnd = c.DateTimeEnd,
                DateTimeStart = c.DateTimeStart,
                Description = c.Description,
                Remarks = c.Remarks,
                VotingId = c.VotingId
            }).ToList());
        }
    }
}
