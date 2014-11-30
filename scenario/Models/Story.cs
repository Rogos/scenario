using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Story
    {
        public int ID { get; set; }

        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        public int? LeaderId { get; set; }

        [Display(Name = "Utworzone")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zaktualizowane")]
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Character> Characters { get; set; }

        [Display(Name = "Prowadzący")]
        [ForeignKey("LeaderId")]
        public virtual User Leader { get; set; }
    }
}