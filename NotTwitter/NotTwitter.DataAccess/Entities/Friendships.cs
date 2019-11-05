using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.DataAccess.Entities
{
    public class Friendships
    {
        public int User1ID { get; set; }
        public int User2ID { get; set; }
        public DateTime TimeRequestSent { get; set; }
        public DateTime TimeRequestConfirmed { get; set; } 

        [ForeignKey("User1ID")]
        public virtual Users User1 { get; set; }
        [ForeignKey("User2ID")]
        public virtual Users User2 { get; set; }

    }
}
