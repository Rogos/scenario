using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{

    public class Thread
    {
        public int ID { get; set; }

        public int? AuthorId { get; set; }
        public int? StoryId { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Main { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        [ForeignKey("StoryId")]
        public virtual Story Story { get; set; }

        public virtual ICollection<Thread> Parents { get; set; }
        public virtual ICollection<Thread> Children { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }

        public Thread()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Parents = new List<Thread>();
            Children = new List<Thread>();
            Characters = new List<Character>();
            Votings = new List<Voting>();
            Selected = false;
        }
    }
}