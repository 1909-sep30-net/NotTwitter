﻿using System;


namespace NotTwitter.DataAccess.Entities
{
	public class Comments
	{
		public int CommentId { get; set; }

		public string Content { get; set; }
		public DateTime TimeSent { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public virtual Users User { get; set; }
		public virtual Posts Post { get; set; }
	}
}
