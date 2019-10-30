using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
	public class Posts
	{
		public int PostId { get; set; }
		public int UserId { get; set; }
		public DateTime TimeSent { get; set; }
		public string Content { get; set; }

		public virtual Users User { get; set; }
        public virtual ICollection <Comments> Comments  {get; set; }

	}
}
