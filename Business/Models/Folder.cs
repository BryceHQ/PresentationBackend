using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Business.Models
{
    [Table("Folder")]
    public class Folder
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Guid { get; set; }

        [Required]
        public int Parent { get; set; }
      
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }
      
        [Required]
        public DateTime LastUpdateTime { get; set; }
    }
}
