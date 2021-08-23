using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class ChangePasswordPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private string _newPassword;
        private string _currentPassword;
        private string _confirmPassword;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _saveCommand;

        public ChangePasswordPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Cambiar Contraseña";
        }

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        private async void SaveAsync()
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

            var request = new ChangePasswordRequest
            {
                OldPassword = CurrentPassword,
                NewPassword = NewPassword,
            };

            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.PutAsync(url, "api", $"/Users/ChangePassword/{user.UserId}", request, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Error al cambiar su contraseña", "Aceptar");
                return;
            }

            IsRunning = false;

            var newp = new TokenRequest
            {
                cedula   = user.Cedula,
                password = NewPassword
            };

            Settings.Secure = JsonConvert.SerializeObject(newp);

            await App.Current.MainPage.DisplayAlert("Completado", "La contraseña se ha actualizado satisfactoriamente", "Aceptar");

            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(CurrentPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar la contraseña actual", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar la nueva contraseña", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar la confirmación de la contraseña", "Aceptar");
                return false;
            }

            var secure = JsonConvert.DeserializeObject<TokenRequest>(Settings.Secure);

            if (!CurrentPassword.Equals(secure.password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "La contraseña actual es incorrecta", "Aceptar");

                CurrentPassword = string.Empty;
                NewPassword     = string.Empty;
                ConfirmPassword = string.Empty;
                return false;
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "La nueva contraseña y la confirmación no coinciden!", "Aceptar");

                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                return false;
            }

            if (!Utilities.IsValidPassword(NewPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error",
                "Tu contraseña de ser de mínimo 8 y máximo 20 caracteres, adicional debe contener una letra en mayúscula y un símbolo alfanumérico",
                "Aceptar");
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                return false;
            }

            return true;
        }
    }
}
