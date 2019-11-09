using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
    public class UserPostModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(1,ErrorMessage = "First Name cannot be empty.")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MinLength(1, ErrorMessage = "Last Name cannot be empty.")]
        [MaxLength(50,ErrorMessage ="Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Display Name is required.")]
        [MinLength(1, ErrorMessage = "Display Name cannot be empty.")]
        [MaxLength()]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Range(0,2)]
        public int Gender { get; set; }
    }
}
