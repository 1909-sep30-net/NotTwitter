﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace NotTwitter.API.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Username { get; set; }
     //   public string Password { get; set; }
        public string Email { get; set; }

        public int Gender { get; set; }

        public List<FriendViewModel> Friends { get; set; } = new List<FriendViewModel>();
        public List<PostModel> Posts { get; set; } = new List<PostModel>();
    }
}
