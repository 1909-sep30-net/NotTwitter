﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NotTwitter.DataAccess.Entities
{
	public class Posts
	{
        public Posts()
        {
            Comments = new HashSet<Comments>();
        }
		public int PostId { get; set; }
		public int UserId { get; set; }
		public DateTime TimeSent { get; set; }
		public string Content { get; set; }

		[Range(0, int.MaxValue)]
		public virtual Users User { get; set; }
        public virtual ICollection <Comments> Comments  { get; set; }

	}
}
