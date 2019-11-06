using System;

namespace NotTwitter.Library.Models
{
    public class Friendship
    {
		public int User1ID { get; set; }
		public int User2ID { get; set; }
		/// <summary>
		/// User involved in the relationship; signified as the user sending a the friend request.
		/// </summary>
		public User User1 { get; set; }

        /// <summary>
        /// User involved in the relationship; signified as the recipient of a request.
        /// </summary>
        public User User2 { get; set; }
        
        /// <summary>
        /// Time that a friend request was first sent.
        /// </summary>
        /// <remarks>
        /// Can be used for showing how long ago a user attempted to friend request.
        /// </remarks>
        public DateTime TimeRequestSent { get; set; }

        /// <summary>
        /// Time that a friend request was finally confirmed.
        /// </summary>
        /// <remarks>
        /// Can be used for showing how long ago 
        /// </remarks>
        public DateTime TimeRequestConfirmed { get; set; }
    }
}
