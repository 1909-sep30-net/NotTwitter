using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
    public class FriendViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
