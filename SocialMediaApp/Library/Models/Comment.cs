using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Comment
    {
        // private backing fields
        private string _content;

        /// <summary>
        /// This comment's ID. 0 if new comment.
        /// </summary>
        public int CommentId { get; set; }

        /* Commenting these out-- Object Oriented relationship means that a User has comments/posts, not the other way around; 
         * that would be reflected in Entities instead
         * 
        /// <summary>
        /// The post that this comment is posted to.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// The user that this comment was written by
        /// </summary>
        public User User { get; set; }
        */
        
        /// <summary>
        /// The text content of the comment.
        /// </summary>
        /// <remarks>
        /// Removes any special characters (?) that could potentially break code
        /// </remarks>
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

        /// <summary>
        /// Time that the comment was written.
        /// </summary>
        public DateTime TimeSent { get; set; }
    }
}
