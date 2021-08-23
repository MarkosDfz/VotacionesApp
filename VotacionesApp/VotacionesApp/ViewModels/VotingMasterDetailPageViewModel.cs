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

namespace VotacionesApp.ViewModels
{
    public class VotingMasterDetailPageViewModel : ViewModelBase
    {
        private UserResponse _user;
        private string _name;
        private string _photoFullPath;
        private readonly INavigationService _navigationService;

        public VotingMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string PhotoFullPath
        {
            get => _photoFullPath;
            set => SetProperty(ref _photoFullPath, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private void LoadUser()
        {
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            PhotoFullPath = User.PhotoFullPath;
            Name = $"Hola {User.FirstName}!";
        }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "homemenu",
                    PageName = "HomePage",
                    Title = "Inicio"
                },

                new Menu
                {
                    Icon = "resultsmenu",
                    PageName = "ResultsPage",
                    Title = "Resultados"
                },

                new Menu
                {
                    Icon = "certificatemenu",
                    PageName = "CertificatesPage",
                    Title = "Certificados"
                },

                new Menu
                {
                    Icon = "securitymenu",
                    PageName = "SecurityPage",
                    Title = "Seguridad"
                },

                new Menu
                {
                    Icon = "profilemenu",
                    PageName = "ProfilePage",
                    Title = "Mi Perfil"
                },

                new Menu
                {
                    Icon = "exitmenu",
                    PageName = "LoginPage",
                    Title = "Cerrar Sesión"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}