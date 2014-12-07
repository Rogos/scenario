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

        [Display(Name = "Autor")]
        public int? AuthorId { get; set; }

        [Display(Name = "Opowiadanie")]
        public int StoryId { get; set; }

        [Display(Name = "Początek wątku (czas)")]
        public string StartDate { get; set; }

        [Display(Name = "Koniec wątku (czas)")]
        public string StopDate { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Treść")]
        public string Text { get; set; }

        [Display(Name = "Wybrany")]
        public bool Selected { get; set; }

        [Display(Name = "Główny")]
        public bool Main { get; set; }

        [Display(Name = "Utworzony")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zaktualizowany")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Autor")]
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [Display(Name = "Opowiadanie")]
        [ForeignKey("StoryId")]
        public virtual Story Story { get; set; }

        [Display(Name = "Wątki poprzedzające")]
        public virtual ICollection<Thread> Parents { get; set; }

        [Display(Name = "Wątki późniejsze")]
        public virtual ICollection<Thread> Children { get; set; }

        [Display(Name = "Postacie")]
        public virtual ICollection<Character> Characters { get; set; }

        [Display(Name = "Głosowania")]
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