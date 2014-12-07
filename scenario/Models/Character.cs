using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class Character
    {
        public int ID { get; set; }

        [Display(Name = "Opowiadanie")]
        public int StoryID { get; set; }

        [Display(Name = "Autor")]
        public int? AuthorId { get; set; }

        [Required]
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

        [Display(Name = "Autor")]
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [ForeignKey("Character1ID")]
        public virtual ICollection<CharacterRelation> CharacterRelations { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }

        public Character()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Selected = false;
        }
    }
}