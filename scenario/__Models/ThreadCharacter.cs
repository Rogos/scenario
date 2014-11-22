using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class ThreadCharacter
    {
        public int ID { get; set; }
        
        public int ThreadId { get; set; }
        public int CharacterId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public ThreadCharacter()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}