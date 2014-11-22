using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class VotingVariant
    {
        public int ID { get; set; }

        public int VotingId { get; set; }
        public int ThreadId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("VotingId")]
        public virtual Voting Voting { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Voting Thread { get; set; }

        public VotingVariant()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}