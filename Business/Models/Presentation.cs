using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Business.Models
{

    [Table("Presentation")]
    public class Presentation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required]
        public string Guid { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int FolderId { get; set; }
       
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength]
        public string Raw { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }
       
        [Required]
        public DateTime LastUpdateTime { get; set; }
    }
}