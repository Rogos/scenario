using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Character
    {
        public int ID { get; set; }

        public int StoryID { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Wybrana")]
        public bool Selected { get; set; }

        [Display(Name = "Utworzona")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zaktualizowana")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Opowiadanie")]
        public virtual Story Story { get; set; }
        //public virtual ICollection<CharacterRelation> CharacterRelations { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
    }
}