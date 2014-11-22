using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace scenario.Models
{
    public class CharacterRelation
    {
        [Key]
        public int ID { get; set; }
        public int Character1ID { get; set; }
        public string Description { get; set; }
        public int Character2ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("Character1ID")]
        public virtual Character Character1 { get; set; }
        [ForeignKey("Character2ID")]
        public virtual Character Character2 { get; set; }
    }
}