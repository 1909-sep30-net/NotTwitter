using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Friendship
    {
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
