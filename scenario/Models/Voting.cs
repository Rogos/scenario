using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Voting
    {
        public int ID { get; set; }

        public int StoryId { get; set; }
        public string Description { get; set; }
        public bool Open { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("StoryId")]
        public virtual Story Story { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

        public Voting()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Threads = new List<Thread>();
            Votes = new List<Vote>();
        }
    }
}