using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VotacionesApp.ViewModels
{
    public class TutorialPageViewModel : ViewModelBase
    {
        public TutorialPageViewModel(INavigationService navigationService) : base (navigationService)
        {
            Title = "Tutorial";
        }
    }
}
