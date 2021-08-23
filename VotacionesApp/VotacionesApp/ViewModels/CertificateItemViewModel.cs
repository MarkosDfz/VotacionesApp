using System;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using Xamarin.Essentials;

namespace VotacionesApp.ViewModels
{
    public class CertificateItemViewModel : CertificateResponse
    {
        private DelegateCommand _selectCertificateCommand;

        public CertificateItemViewModel(INavigationService navigationService)
        {
        }

        public DelegateCommand SelectCertificateCommand => _selectCertificateCommand ?? (_selectCertificateCommand = new DelegateCommand(SelectCertificateAsync));

        private async void SelectCertificateAsync()
        {
            var parameters = new NavigationParameters
            {
                { "certificado", this }
            };

            var a = parameters.GetValue<CertificateResponse>("certificado");

            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            var url = "https://votacionesuejosemejia.tk/Votings/ShowCertificate1/";
            await Launcher.OpenAsync(new Uri($"{url}{a.VotingId}?id2={user.UserId}"));
        }
    }
}
