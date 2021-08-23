using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using VotacionesApp.Helpers;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class SecurityPageViewModel : ViewModelBase
    {
        private bool _isActivate;
        private string _textBool;
        private string _btnColor;
        private readonly IFingerprintService _fingerprintService;
        private DelegateCommand _isActivateBoolCommand;

        public SecurityPageViewModel(INavigationService navigationService, IFingerprintService fingerprintService) : base(navigationService)
        {
            Title = "Seguridad";
            _fingerprintService = fingerprintService;
            if (Settings.IsActivated == true)
            {
                IsActivate = true;
                TextBool = "Activado";
                BtnColor = "#C00C3A";
            }
            else
            {
                IsActivate = false;
                TextBool = "Desactivado";
                BtnColor = "#0F347F";
            }
        }

        public DelegateCommand IsActivateBoolCommand => _isActivateBoolCommand ?? (_isActivateBoolCommand = new DelegateCommand(IsActivateBoolAsync));

        public bool IsActivate
        {
            get => _isActivate;
            set => SetProperty(ref _isActivate, value);
        }

        public string TextBool
        {
            get => _textBool;
            set => SetProperty(ref _textBool, value);
        }

        public string BtnColor
        {
            get => _btnColor;
            set => SetProperty(ref _btnColor, value);
        }

        private async void IsActivateBoolAsync()
        {
            if (IsActivate == true)
            {
                var result = await _fingerprintService.AuthenticateAsync("Desactivar Protección");

                switch (result.Status)
                {
                    case FingerprintAuthenticationResultStatus.Succeeded:
                        Settings.IsActivated = false;
                        IsActivate = false;
                        TextBool = "Desactivado";
                        BtnColor = "#0F347F";
                        break;
                    case FingerprintAuthenticationResultStatus.FallbackRequested:
                        await App.Current.MainPage.DisplayAlert("Error", "La autenticación por pin esta deshabilitada", "Aceptar");
                        break;
                    case FingerprintAuthenticationResultStatus.NotAvailable:
                        await App.Current.MainPage.DisplayAlert("Error", "Su dispositivo no es compatible con el desbloqueo biométrico", "Aceptar");
                        TextBool = "Desactivado";
                        BtnColor = "#0F347F";
                        Settings.IsActivated = false;
                        IsActivate = false;
                        break;
                    default:
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo deactivar la protección", "Aceptar");
                        break;
                }

                return;
            }
            if (IsActivate == false)
            {
                var isAvailable = await _fingerprintService.IsAvailableAsync();

                if (isAvailable == true)
                {
                    var answer = await App.Current.MainPage.DisplayAlert("Confirmar",
                            $"¿Está seguro de activar la protección del voto?", "Si", "No");

                    if (!answer)
                    {
                        return;
                    }

                    Settings.IsActivated = true;
                    IsActivate = true;
                    TextBool = "Activado";
                    BtnColor = "#C00C3A";
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Su dispositivo no es compatible con el desbloqueo dactilar", "Aceptar");
                    TextBool = "Desactivado";
                    BtnColor = "#0F347F";
                    Settings.IsActivated = false;
                    IsActivate = false;
                }
            }
        }
    }
}
