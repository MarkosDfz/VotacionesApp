using System;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;
using VotacionesApp.ViewModels;
using VotacionesApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VotacionesApp
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider
                      .RegisterLicense("MTg4NTgzQDMxMzcyZTM0MmUzMEdtOS9OZlFXWlNrQjZpM2ZXeXRiZTJCNTI3Tk1RR0NhZzZDTjNIT0dnY2s9");
            InitializeComponent();

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (Settings.IsRemembered && token?.Expiration > DateTime.Now)
            {
                await NavigationService.NavigateAsync("/VotingMasterDetailPage/NavigationPage/HomePage");
            }
            else
            {
                Settings.IsActivated = false;
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IFingerprintService, FingerprintService>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<TutorialPage, TutorialPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<VotingPage, VotingPageViewModel>();
            containerRegistry.RegisterForNavigation<VotingsPage, VotingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ResultsPage, ResultsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<VotingMasterDetailPage, VotingMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<CertificatePage, CertificatePageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ResultPage, ResultPageViewModel>();
            containerRegistry.RegisterForNavigation<CertificatesPage, CertificatesPageViewModel>();
            containerRegistry.RegisterForNavigation<SecurityPage, SecurityPageViewModel>();
        }
    }
}
