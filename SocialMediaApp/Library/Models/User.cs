using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.Models
{
    public class User
    {
        // private backing fields for the properties
        private string _firstName;
        private string _lastName;
        private int _gender;
        private string _email;

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
                    _firstName = value[0].ToString().ToUpper() + value.Substring(1).ToLower();
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
                    _lastName = value[0].ToString().ToUpper() + value.Substring(1).ToLower();
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
                if (Regex.IsMatch(value, @"^([a-zA-Z0-9_-.]+)@(([[0-9]{1,3}" +
                            @".[0-9]{1,3}.[0-9]{1,3}.)|(([a-zA-Z0-9-]+" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$" +
                            @"^([a-zA-Z0-9_-.]+)@(([[0-9]{1,3}" +
                            @".[0-9]{1,3}.[0-9]{1,3}.)|(([a-zA-Z0-9-]+" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$"))
                {
                    _email = value;
                }
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
        public string Password { get; set; }

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
    }
}
