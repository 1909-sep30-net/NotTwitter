using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Post
    {
        // private backing fields
        private string _content;

        /// <summary>
        /// ID of the post.
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// The user that this post was written by.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Text content of the post.
        /// </summary>
        public string Content 
        {
            get
            {
                return _content;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Comment cannot be null.", nameof(value));
                }
                if (value == "")
                {
                    throw new ArgumentOutOfRangeException("Comment cannot be empty string.", nameof(value));
                }
                if (value.Length > 281)
                {
                    throw new ArgumentOutOfRangeException("Comment cannot be longer than 281 characters.", nameof(value));
                }

                _content = value;
            }
        }

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        /// <summary>
        /// Time that the post was submitted.
        /// </summary>
        public DateTime TimeSent { get; set; }
    }
}
