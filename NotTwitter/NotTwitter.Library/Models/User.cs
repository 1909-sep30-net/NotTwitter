using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NotTwitter.Library.Models
{
    public class User
    {
        // private backing fields for the properties
        private string _firstName;
        private string _lastName;
        private int _gender;
        private string _email;
        private string _password;

        /// <summary>
        /// ID that uniquely identifies the user, 0 if unset
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirstName 
        {
            get
            {
                return _firstName;
            }

            set
            {
                // Throw exception if input is null or empty string
                ValidateName(value);

                // Check expression for alphabetic characters only
                if (Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                {
                    // Always capitalize first letter and lowercase the rest
                    _firstName = CapitalizeName(value);
                }

            }
        }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName 
        {
            get
            {
                return _lastName;
            }
            set
            {
                // Throw exception if input is null or empty string
                ValidateName(value);

                // Check expression for alphabetic characters only
                if (Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                {
                    // Always capitalize first letter and lowercase the rest
                    _lastName = CapitalizeName(value);
                }
            }
        }

        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email 
        { 
            get
            {
                return _email;
            }
            set
            {
                try
                {
                    System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(value);
                }
                catch (FormatException ex)
                {
                    throw ex;
                }
                _email = value;
            }
        }

        /// <summary>
        /// User's gender.
        /// </summary>
        /// <remarks>
        /// 0 : Male
        /// 1 : Female
        /// 2 : Other
        /// </remarks>
        public int Gender 
        {
            get
            {
                return _gender;
            }

            set
            {
                if (value < 0 || value > 2)
                {
                    throw new ArgumentOutOfRangeException("Gender ranges 0-2", nameof(value));
                }

                _gender = value;
            }
        }

        /// <summary>
        /// Username that user displays or logs in with
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password that user uses to log into their account
        /// </summary>
        public string Password 
        {
            get
            {
                return _password;
            }
            set
            {
                if (value.Length < 8) 
                {
                    throw new ArgumentException("Password must be 8 characters or longer.", nameof(value));
                }
                _password = value;
            }
        }

        public DateTime DateCreated { get; set; }
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<User> Friends { get; set; } = new HashSet<User>();


        /// <summary>
        /// Error handling for inserting a name
        /// </summary>
        /// <param name="value"></param>
        private void ValidateName(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Name cannot be null", nameof(value));
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("Name cannot be empty string.", nameof(value));
            }

        }

        /// <summary>
        /// Capitalizes a string
        /// </summary>
        /// <param name="value">String to be capitalized</param>
        /// <returns>Capitalized string</returns>
        private string CapitalizeName(string value)
        {
            return value[0].ToString().ToUpper() + value.Substring(1).ToLower();
        }
    }
}
