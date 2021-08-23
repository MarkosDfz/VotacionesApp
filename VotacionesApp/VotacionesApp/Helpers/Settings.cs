using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace VotacionesApp.Helpers
{
    public static class Settings
    {
        private const string _user = "User";
        private const string _token = "Token";
        private const string _secure = "Secure";
        private const string _isRemembered = "IsRemembered";
        private static readonly bool _boolDefault = false;
        private const string _isActivated = "IsActivated";
        private static readonly bool _boolDefault2 = false;

        private static readonly string _settingsDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string User
        {
            get => AppSettings.GetValueOrDefault(_user, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static string Secure
        {
            get => AppSettings.GetValueOrDefault(_secure, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_secure, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

        public static bool IsActivated
        {
            get => AppSettings.GetValueOrDefault(_isActivated, _boolDefault2);
            set => AppSettings.AddOrUpdateValue(_isActivated, value);
        }
    }
}

