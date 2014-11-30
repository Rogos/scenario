using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Voting
    {
        public int ID { get; set; }

        public int StoryId { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Otwarte")]
        public bool Open { get; set; }

        [Display(Name = "Utworzone")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zaktualizowane")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Opowiadanie")]
        [ForeignKey("StoryId")]
        public virtual Story Story { get; set; }

        [Display(Name = "Wątki kandydujące")]
        public virtual ICollection<Thread> Threads { get; set; }

        [Display(Name = "Głosy")]
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