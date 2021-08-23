using System;
namespace VotacionesApp.Models
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string Cedula { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Group { get; set; }

        public string Curso { get; set; }

        public string Photo { get; set; }
        
        public string FullName => $"{LastName} {FirstName}";

        public byte[] ImageArray { get; set; }

        private string _foto { get; } 

        public string PhotoFullPath
        {
            get
            {
                if (!string.IsNullOrEmpty(Photo))
                {
                    return string.Format(
                    "/{0}",
                    Photo.Substring(1));
                    //Aquí la url
                }

                return string.Empty;
            }
        }
    }
}
