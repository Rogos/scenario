using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    [Table("CharacterRelations")]
    public class CharacterRelation
    {
        [Key]
        public int ID { get; set; }

        public int Character1ID { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        public int? Character2ID { get; set; }

        [Display(Name = "Utworzone")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Zaktualizowane")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Postać 1")]
        [ForeignKey("Character1ID")]
        public virtual Character Character1 { get; set; }

        [Display(Name = "Postać 2")]
        [ForeignKey("Character2ID")]
        public virtual Character Character2 { get; set; }
    }
}