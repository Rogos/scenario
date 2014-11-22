using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class ThreadParent
    {
        public int ID { get; set; }

        public int ThreadId { get; set; }
        public int ParentId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        [ForeignKey("ParentId")]
        public virtual Thread Parent { get; set; }

        public ThreadParent()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}