using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using VotacionesApp.Helpers;
using VotacionesApp.Models;
using VotacionesApp.Services;

namespace VotacionesApp.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private MediaFile _file;
        private bool _isRunning;
        private UserResponse _user;
        private ImageSource _imageSource;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;
        private DelegateCommand _changeImageCommand;

        public ProfilePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Mi Perfil";
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            ImageSource = User.PhotoFullPath;
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePassworAsync));

        private async void ChangePassworAsync()
        {
            await _navigationService.NavigateAsync("ChangePasswordPage");
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "¿De dónde desea tomar la foto?",
                "Cancelar",
                null,
                "De la Galería",
                "De la Cámara");

            if (source == "Cancelar")
            {
                _file = null;
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            if (source == "De la Cámara")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Medium,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            }

            if (_file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void SaveAsync()
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

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = FilesHelper.ReadFully(_file.GetStream());
                _file.Dispose();
            }

            var userResponse = new UserResponse
            {
                Cedula = User.Cedula,
                Curso = User.Curso,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Group = User.LastName,
                ImageArray = imageArray,
                UserId = User.UserId,
                Photo = User.Photo
            };

            var response = await _apiService.PutAsync(url, "api", $"/Users/{User.UserId}", userResponse, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Error al actualizar el usuario", "Aceptar");
                return;
            }

            await HomePageViewModel.GetInstance().UpdateUserAsync();

            IsRunning = false;

            await App.Current.MainPage.DisplayAlert("Completado", "Usuario actualizado satisfactoriamente", "Aceptar");

            await _navigationService.NavigateAsync("/VotingMasterDetailPage/NavigationPage/HomePage");
        }
    }
}