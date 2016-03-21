using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Business.Models
{
    [Table("History")]
    public class History
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required]
        public string Guid { get; set; }

        [Required]
        public int PresentationId { get; set; }
        
        [Required]
        [MaxLength]
        public string Raw { get; set; }
       
        [Required]
        public DateTime CreateTime { get; set; }
        
        [Required]
        public DateTime LastUpdateTime { get; set; }
    }
}
