using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
	public class FriendshipModel
	{
	
		[Required]
		public User User1 { get; set; }
		[Required]
		public User User2 { get; set; }
		public DateTime TimeRequestConfirmed { get; set; }

	}
}
