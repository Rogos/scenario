using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Story
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? LeaderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        [ForeignKey("LeaderId")]
        public virtual User Leader { get; set; }
    }
}