using System;
using System.Text.RegularExpressions;

namespace VotacionesApp.Helpers
{
    public class Utilities
    {
        public static bool IsValidPassword(string password)
        {
            return Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}").Success;
        }
    }
}
