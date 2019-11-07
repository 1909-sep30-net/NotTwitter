using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.DataAccess.Entities
{
    public class Friendships
    {
        public int User1ID { get; set; }
        public int User2ID { get; set; }
        public DateTime TimeRequestConfirmed { get; set; } 

        public virtual Users User1 { get; set; }
        public virtual Users User2 { get; set; }

    }
}
