using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace MasrafDeneme.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    
    }
}
