using System;

namespace ZdravoCorp.Models
{
    [Serializable]
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public User() { }
        public User(string username, string password, string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {
            Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
        }

        // override ToString() method to display user's full name
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}

