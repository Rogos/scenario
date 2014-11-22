 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Character
    {
        public int ID { get; set; }
        public int StoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Story Story { get; set; }
        //public virtual ICollection<CharacterRelation> CharacterRelations { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
    }
}