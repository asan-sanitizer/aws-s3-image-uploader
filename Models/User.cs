using System.ComponentModel.DataAnnotations;

namespace Lab03.Models
{
    public class User
    {
        private int userId { get; set; }
        private string name { get; set; }
        [Required]
        private string username { get; set; }
        private string password { get; set; }
        [EmailAddress]
        private string email_address { get; set; }

        public User(int id, string name, string username, string password, string emailAddress)
        {
            userId = id;
            this.username = username;
            this.name = name ; 
            this.password = password; 
            this.email_address= emailAddress;
        }
    }
}