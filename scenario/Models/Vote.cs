using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Vote
    {
        public int ID { get; set; }

        public int VotingId { get; set; }
        public int? ThreadId { get; set; }
        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("VotingId")]
        public virtual Voting Voting { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public Vote()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}